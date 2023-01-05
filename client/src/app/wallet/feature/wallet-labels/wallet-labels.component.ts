import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { filter, finalize, merge, Subject, switchMap, tap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { TuiDialogService } from '@taiga-ui/core';
import { LabelApiService } from '../../../core/api/label-api.service';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { UpdateLabelComponent } from '../../../label/feature/update-label/update-label.component';
import { CreateLabelComponent } from '../../../label/feature/create-label/create-label.component';
import { Label } from '@shared/data-access/models/label';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { ApiParams } from '../../../core/api/generic-api.service';

@Component({
  selector: 'app-wallet-labels',
  templateUrl: './wallet-labels.component.html',
  styleUrls: ['./wallet-labels.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletLabelsComponent {
  private readonly _walletId: Wallet['id'] =
    this._activatedRoute.snapshot.params['id'];

  readonly ui = {
    columns: ['label', 'actions'] as const,
    actions: {
      edit$: new Subject<Label['id']>(),
      delete$: new Subject<Label['id']>(),
      create$: new Subject<void>(),
    },
    deleteActionAt$: new Subject<Label['id'] | null>(),
  };

  readonly filters = this._fb.filters({
    search: [''],
  });

  readonly data = new ApiDataHandler(
    (p: ApiParams) =>
      this._labelService.getAll({ ...p, walletId_eq: this._walletId }),
    this.filters
  );

  readonly dialogs$ = merge(
    this.ui.actions.edit$.pipe(
      switchMap(id =>
        this._dialog.open(
          new PolymorpheusComponent(UpdateLabelComponent, this._injector),
          {
            label: 'Edit label',
            data: {
              id,
            },
          }
        )
      )
    ),
    this.ui.actions.create$.pipe(
      switchMap(() =>
        this._dialog.open(
          new PolymorpheusComponent(CreateLabelComponent, this._injector),
          {
            label: 'Create label',
            data: {
              walletId: this._walletId,
            },
          }
        )
      )
    ),
    this.ui.actions.delete$.pipe(
      tap(id => this.ui.deleteActionAt$.next(id)),
      switchMap(id =>
        this._labelService.delete(id).pipe(
          filter(success => success),
          finalize(() => this.ui.deleteActionAt$.next(null))
        )
      )
    )
  ).pipe(tap(() => this.data.resetPage()));

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _labelService: LabelApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {}

  trackById = (index: number, item: { id: Label['id'] }) => item.id;
}
