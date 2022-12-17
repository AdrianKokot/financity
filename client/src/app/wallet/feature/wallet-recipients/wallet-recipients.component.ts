import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
  ViewChild,
} from '@angular/core';
import { FormBuilder } from '@angular/forms';
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
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { ActivatedRoute } from '@angular/router';
import { CreateRecipientComponent } from '../../../recipient/feature/create-recipient/create-recipient.component';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { RecipientApiService } from '../../../core/api/recipient-api.service';
import { TuiAlertService, TuiDialogService } from '@taiga-ui/core';
import {
  Recipient,
  RecipientListItem,
} from '@shared/data-access/models/recipient.model';
import { UpdateRecipientComponent } from '../../../recipient/feature/update-recipient/update-recipient.component';

@Component({
  selector: 'app-wallet-recipients',
  templateUrl: './wallet-recipients.component.html',
  styleUrls: ['./wallet-recipients.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletRecipientsComponent {
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
    private _recipientApiService: RecipientApiService,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService,
    @Inject(Injector) private _injector: Injector,
    private _dialog: TuiDialogService
  ) {}

  readonly columns = ['name', 'actions'];

  openCreateDialog(): void {
    this.walletId$
      .pipe(
        switchMap(walletId => {
          return this._dialog.open<Recipient>(
            new PolymorpheusComponent(CreateRecipientComponent, this._injector),
            {
              label: 'Create recipient',
              data: {
                walletId,
              },
            }
          );
        })
      )
      .subscribe(cat => {
        this._newRecipient$.next(cat);
      });
  }

  openEditDialog(id: Recipient['id']): void {
    this._dialog
      .open<Recipient>(
        new PolymorpheusComponent(UpdateRecipientComponent, this._injector),
        {
          label: 'Edit recipient',
          data: {
            id,
          },
        }
      )
      .subscribe(category => this._modifiedRecipient$.next(category));
  }

  deleteRecipient(id: Recipient['id']): void {
    this._recipientApiService
      .delete(id)
      .pipe(filter(success => success))
      .subscribe(() => {
        this._deletedRecipient$.next({ id });
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
      this._recipientApiService
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

  private _modifiedRecipient$ = new Subject<Recipient>();
  private _deletedRecipient$ = new Subject<Pick<Recipient, 'id'>>();
  private _newRecipient$ = new Subject<Recipient>();

  readonly request$ = merge(
    this.labels$.pipe(
      filter((x): x is RecipientListItem[] => x !== null),
      map(items => (acc: RecipientListItem[]) => [...acc, ...items])
    ),
    this._modifiedRecipient$.pipe(
      map(cat => (acc: RecipientListItem[]) => {
        const index = acc.findIndex(x => x.id === cat.id);

        if (index !== -1) {
          acc[index] = cat;
          return [...acc];
        }

        return null;
      }),
      filter(
        (x): x is (acc: RecipientListItem[]) => RecipientListItem[] =>
          x !== null
      )
    ),
    this._deletedRecipient$.pipe(
      map(({ id }) => (acc: RecipientListItem[]) => {
        const index = acc.findIndex(x => x.id === id);

        if (index !== -1) {
          acc.splice(index, 1);
          return [...acc];
        }

        return null;
      }),
      filter(
        (x): x is (acc: RecipientListItem[]) => RecipientListItem[] =>
          x !== null
      )
    ),
    this._newRecipient$.pipe(
      map(
        item => (acc: RecipientListItem[]) =>
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
      scan(
        (acc: RecipientListItem[], fn) => fn(acc),
        [] as RecipientListItem[]
      ),
      share()
    );

  readonly loading$ = this.labels$.pipe(map(value => !value));
  readonly data$ = this.request$.pipe(startWith([] as RecipientListItem[]));
}
