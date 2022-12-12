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
import { CategoryApiService } from '../../../core/api/category-api.service';
import {
  Category,
  CategoryListItem,
} from '@shared/data-access/models/category.model';
import { CreateCategoryComponent } from '../../../category/feature/create-category/create-category.component';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { UpdateCategoryComponent } from 'src/app/category/feature/update-category/update-category.component';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';

@Component({
  selector: 'app-wallet-categories',
  templateUrl: './wallet-categories.component.html',
  styleUrls: ['./wallet-categories.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletCategoriesComponent {
  walletId$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id),
    shareReplay(1)
  );

  form = this._fb.nonNullable.group({
    search: [''],
  });

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

  constructor(
    private _fb: FormBuilder,
    private _walletService: WalletApiService,
    private _activatedRoute: ActivatedRoute,
    private _categoryService: CategoryApiService,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService,
    @Inject(Injector) private _injector: Injector,
    private _dialog: TuiDialogService
  ) {}

  readonly columns = ['category', 'transactionType', 'actions'];

  openCreateDialog(): void {
    this.walletId$
      .pipe(
        switchMap(walletId => {
          return this._dialog.open<Category>(
            new PolymorpheusComponent(CreateCategoryComponent, this._injector),
            {
              label: 'Create category',
              data: {
                walletId,
              },
            }
          );
        })
      )
      .subscribe(cat => {
        this._newCategory$.next(cat);
      });
  }

  openEditDialog(id: Category['id']): void {
    this._dialog
      .open<Category>(
        new PolymorpheusComponent(UpdateCategoryComponent, this._injector),
        {
          label: 'Edit category',
          data: {
            id,
          },
        }
      )
      .subscribe(category => this._modifiedCategory$.next(category));
  }

  deleteCategory(id: Category['id']): void {
    this._categoryService
      .delete(id)
      .pipe(filter(success => success))
      .subscribe(() => {
        this._deletedCategory$.next({ id });
      });
  }

  // trackByIdx = (index: number, item: CategoryListItem) => JSON.stringify(item);

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

  categories$ = this.page$.pipe(
    withLatestFrom(this.walletId$, this.filters$),
    exhaustMap(([page, walletId, filters]) =>
      this._categoryService
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

  private _modifiedCategory$ = new Subject<Category>();
  private _deletedCategory$ = new Subject<Pick<Category, 'id'>>();
  private _newCategory$ = new Subject<Category>();

  readonly request$ = merge(
    this.categories$.pipe(
      filter((x): x is CategoryListItem[] => x !== null),
      map(items => (acc: CategoryListItem[]) => [...acc, ...items])
    ),
    this._modifiedCategory$.pipe(
      map(cat => (acc: CategoryListItem[]) => {
        const index = acc.findIndex(x => x.id === cat.id);

        if (index !== -1) {
          acc[index] = cat;
          return [...acc];
        }

        return null;
      }),
      filter(
        (x): x is (acc: CategoryListItem[]) => CategoryListItem[] => x !== null
      )
    ),
    this._deletedCategory$.pipe(
      map(({ id }) => (acc: CategoryListItem[]) => {
        const index = acc.findIndex(x => x.id === id);

        if (index !== -1) {
          acc.splice(index, 1);
          return [...acc];
        }

        return null;
      }),
      filter(
        (x): x is (acc: CategoryListItem[]) => CategoryListItem[] => x !== null
      )
    ),
    this._newCategory$.pipe(
      map(
        item => (acc: CategoryListItem[]) =>
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
      scan((acc: CategoryListItem[], fn) => fn(acc), [] as CategoryListItem[]),
      share()
    );

  readonly loading$ = this.categories$.pipe(map(value => !value));
  readonly data$ = this.request$.pipe(startWith([] as CategoryListItem[]));
}
