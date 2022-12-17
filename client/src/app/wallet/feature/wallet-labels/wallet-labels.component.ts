import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
  ViewChild,
} from '@angular/core';
import {
  BehaviorSubject,
  debounceTime,
  distinctUntilKeyChanged,
  exhaustMap,
  filter,
  map,
  merge,
  scan,
  share,
  shareReplay,
  startWith,
  Subject,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { ActivatedRoute } from '@angular/router';
import { TuiAlertService, TuiDialogService } from '@taiga-ui/core';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { LabelApiService } from '../../../core/api/label-api.service';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { UpdateLabelComponent } from '../../../label/feature/update-label/update-label.component';
import { CreateLabelComponent } from '../../../label/feature/create-label/create-label.component';
import { Label, LabelListItem } from '@shared/data-access/models/label';

@Component({
  selector: 'app-wallet-labels',
  templateUrl: './wallet-labels.component.html',
  styleUrls: ['./wallet-labels.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletLabelsComponent {
  form = this._fb.nonNullable.group({
    search: [''],
  });

  walletId$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id),
    shareReplay(1)
  );

  constructor(
    private _fb: FormBuilder,
    private _walletService: WalletApiService,
    private _activatedRoute: ActivatedRoute,
    private _labelService: LabelApiService,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService,
    @Inject(Injector) private _injector: Injector,
    private _dialog: TuiDialogService
  ) {}

  readonly columns = ['label', 'actions'];

  openCreateDialog(): void {
    this.walletId$
      .pipe(
        switchMap(walletId => {
          return this._dialog.open<Label>(
            new PolymorpheusComponent(CreateLabelComponent, this._injector),
            {
              label: 'Create label',
              data: {
                walletId,
              },
            }
          );
        })
      )
      .subscribe(cat => {
        this._newLabel$.next(cat);
      });
  }

  openEditDialog(id: Label['id']): void {
    this._dialog
      .open<Label>(
        new PolymorpheusComponent(UpdateLabelComponent, this._injector),
        {
          label: 'Edit label',
          data: {
            id,
          },
        }
      )
      .subscribe(category => this._modifiedLabel$.next(category));
  }

  deleteLabel(id: Label['id']): void {
    this._labelService
      .delete(id)
      .pipe(filter(success => success))
      .subscribe(() => {
        this._deletedLabel$.next({ id });
      });
  }

  log() {
    if (this.gotAllResults) return;
    const { end } = this.viewport.getRenderedRange();
    const total = this.viewport.getDataLength();

    if (end === total) {
      this.page$.next(Math.floor(total / this._pageSize) + 1);
    }
  }

  @ViewChild(CdkVirtualScrollViewport) viewport!: CdkVirtualScrollViewport;

  page$ = new BehaviorSubject<number>(1);
  private _pageSize = 250;

  filters$ = this.form.valueChanges.pipe(
    debounceTime(300),
    map(() => this.form.getRawValue()),
    map(({ search }) => {
      return { search: search.trim() };
    }),
    distinctUntilKeyChanged('search'),
    share(),
    startWith({})
  );

  labels$ = this.page$.pipe(
    withLatestFrom(this.walletId$, this.filters$),
    exhaustMap(([page, walletId, filters]) =>
      this._labelService
        .getList(walletId, {
          page,
          pageSize: this._pageSize,
          filters,
        })
        .pipe(startWith(null))
    ),
    shareReplay(1)
  );

  gotAllResults = false;

  private _modifiedLabel$ = new Subject<Label>();
  private _deletedLabel$ = new Subject<Pick<Label, 'id'>>();
  private _newLabel$ = new Subject<Label>();

  readonly request$ = merge(
    this.labels$.pipe(
      filter((x): x is LabelListItem[] => x !== null),
      map(items => (acc: LabelListItem[]) => [...acc, ...items])
    ),
    this._modifiedLabel$.pipe(
      map(cat => (acc: LabelListItem[]) => {
        const index = acc.findIndex(x => x.id === cat.id);

        if (index !== -1) {
          acc[index] = cat;
          return [...acc];
        }

        return null;
      }),
      filter((x): x is (acc: LabelListItem[]) => LabelListItem[] => x !== null)
    ),
    this._deletedLabel$.pipe(
      map(({ id }) => (acc: LabelListItem[]) => {
        const index = acc.findIndex(x => x.id === id);

        if (index !== -1) {
          acc.splice(index, 1);
          return [...acc];
        }

        return null;
      }),
      filter((x): x is (acc: LabelListItem[]) => LabelListItem[] => x !== null)
    ),
    this._newLabel$.pipe(
      map(
        item => (acc: LabelListItem[]) =>
          [...acc, item].sort((a, b) => a.name.localeCompare(b.name))
      )
    ),
    this.filters$.pipe(
      map(() => () => []),
      tap(() => this.page$.next(1))
    )
  )
    //   combineLatest([
    //   this.sorter$,
    //   this.direction$,
    //   this.page$,
    //   this.size$,
    //   tuiControlValue<number>(this.minAge),
    // ])
    .pipe(
      // zero time debounce for a case when both key and direction change
      scan((acc: LabelListItem[], fn) => fn(acc), [] as LabelListItem[]),
      share()
    );

  readonly loading$ = this.labels$.pipe(map(value => !value));
  readonly data$ = this.request$.pipe(startWith([] as LabelListItem[]));
}
