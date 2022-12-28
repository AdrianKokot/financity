import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
  ViewChild,
} from '@angular/core';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import {
  BehaviorSubject,
  debounceTime,
  distinctUntilKeyChanged,
  exhaustMap,
  filter,
  map,
  merge,
  Observable,
  scan,
  share,
  shareReplay,
  startWith,
  Subject,
  switchMap,
  take,
  tap,
  withLatestFrom,
} from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import {
  TuiAlertService,
  TuiDialogService,
  TuiNotification,
} from '@taiga-ui/core';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { User } from '../../../auth/data-access/models/user';
import { ShareWalletComponent } from '../../ui/share-wallet/share-wallet.component';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';

@Component({
  selector: 'app-wallet-share-management',
  templateUrl: './wallet-share-management.component.html',
  styleUrls: ['./wallet-share-management.component.scss'],
  // encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletShareManagementComponent {
  form = this._fb.nonNullable.group({
    search: [''],
  });

  walletId$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id),
    shareReplay(1)
  );

  filters$: Observable<Record<string, string | string[]>> =
    this.form.valueChanges.pipe(
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
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService,
    @Inject(Injector) private _injector: Injector,
    private _dialog: TuiDialogService
  ) {}

  readonly columns = ['name', 'email', 'actions'];

  openShareDialog(): void {
    this.walletId$
      .pipe(
        take(1),
        switchMap(walletId => {
          return this._dialog.open<boolean>(
            new PolymorpheusComponent(ShareWalletComponent, this._injector),
            {
              label: 'Share wallet',
              data: {
                walletId,
              },
            }
          );
        }),
        switchMap(success => {
          if (success) {
            return this._alertService.open('Invitation sent successfully.', {
              status: TuiNotification.Success,
              label: 'Success',
            });
          }
          return this._alertService.open(
            'Something went wrong. Please try again later.',
            {
              status: TuiNotification.Error,
              label: 'Error',
            }
          );
        })
      )
      .subscribe();
  }

  revokeAccess(user: User): void {
    this.walletId$
      .pipe(
        take(1),
        switchMap(walletId => {
          return this._walletService.revoke({
            userEmail: user.email,
            walletId,
          });
        })
      )
      .subscribe(() => {
        this._revokedShare$.next({ id: user.id });
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

  users$ = this.page$.pipe(
    // distinctUntilChanged(),
    withLatestFrom(this.walletId$, this.filters$),
    exhaustMap(([page, walletId, filters]) =>
      this._walletService
        .getSharedToList(walletId, {
          page,
          pageSize: this._pageSize,
          filters,
        })
        .pipe(startWith(null))
    ),
    shareReplay(1)
  );

  gotAllResults = false;

  private _revokedShare$ = new Subject<Pick<User, 'id'>>();
  private _newShare$ = new Subject<User>();

  readonly request$ = merge(
    this.users$.pipe(
      filter((x): x is User[] => x !== null),
      map(items => (acc: User[]) => [...acc, ...items])
    ),
    this._revokedShare$.pipe(
      map(({ id }) => (acc: User[]) => {
        const index = acc.findIndex(x => x.id === id);

        if (index !== -1) {
          acc.splice(index, 1);
          return [...acc];
        }

        return null;
      }),
      filter((x): x is (acc: User[]) => User[] => x !== null)
    ),
    this._newShare$.pipe(
      map(
        item => (acc: User[]) =>
          [...acc, item].sort((a, b) => a.name.localeCompare(b.name))
      )
    ),
    this.filters$.pipe(
      map(() => () => []),
      tap(() => this.page$.next(1))
    )
  ).pipe(
    scan((acc: User[], fn) => fn(acc), [] as User[]),
    share()
  );

  readonly loading$ = this.users$.pipe(map(value => !value));
  readonly data$ = this.request$.pipe(startWith([] as User[]));
}
