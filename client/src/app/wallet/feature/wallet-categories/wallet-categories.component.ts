import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { BehaviorSubject, filter, map, shareReplay, switchMap } from 'rxjs';
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

@Component({
  selector: 'app-wallet-categories',
  templateUrl: './wallet-categories.component.html',
  styleUrls: ['./wallet-categories.component.scss'],
  // encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletCategoriesComponent {
  walletId$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id),
    shareReplay(1)
  );

  // wallet$ = this._activatedRoute.params.pipe(
  //   filter((params): params is { id: string } => 'id' in params),
  //   map(params => params.id),
  //   switchMap(walletId => this._walletService.get(walletId))
  // );
  currentlyEditedId$ = new BehaviorSubject<CategoryListItem['id'] | null>(null);
  poll$ = new BehaviorSubject<void>(undefined);

  categories$ = this.poll$.pipe(
    switchMap(() => this.walletId$),
    switchMap(walletId => this._categoryService.getList(walletId))
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

  readonly columns = ['appearance', 'name', 'actions'];

  remove(id: Category['id']): void {}

  save(): void {
    this.currentlyEditedId$.next(null);
  }

  edit(id: Category['id']): void {
    this.currentlyEditedId$.next(id);
  }

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
      .subscribe(category => {
        this.poll$.next();
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
      .subscribe(category => this.poll$.next());
  }

  deleteCategory(id: Category['id']): void {
    this._categoryService
      .delete(id)
      .pipe(filter(success => success))
      .subscribe(() => {
        this.poll$.next();
      });
  }
}
