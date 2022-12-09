import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { BehaviorSubject, filter, map, switchMap } from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { ActivatedRoute } from '@angular/router';
import { TuiAlertService } from '@taiga-ui/core';
import { CategoryApiService } from '../../../core/api/category-api.service';
import {
  Category,
  CategoryListItem,
} from '@shared/data-access/models/category.model';

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
    map(params => params.id)
  );

  // wallet$ = this._activatedRoute.params.pipe(
  //   filter((params): params is { id: string } => 'id' in params),
  //   map(params => params.id),
  //   switchMap(walletId => this._walletService.get(walletId))
  // );
  currentlyEditedId$ = new BehaviorSubject<CategoryListItem['id'] | null>(null);

  categories$ = this.walletId$.pipe(
    switchMap(walletId => this._categoryService.getList(walletId))
  );

  constructor(
    private _fb: FormBuilder,
    private _walletService: WalletApiService,
    private _activatedRoute: ActivatedRoute,
    private _categoryService: CategoryApiService,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService
  ) {}

  readonly columns = ['appearance', 'name', 'actions'];

  remove(id: Category['id']): void {}

  save(): void {
    this.currentlyEditedId$.next(null);
  }

  edit(id: Category['id']): void {
    this.currentlyEditedId$.next(id);
  }
}
