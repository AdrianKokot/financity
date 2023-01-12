import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { DATE_RANGE_FILTER_GROUPS } from '../../utils/date-range-filter-groups.constants';
import { TuiDay, TuiDayLike } from '@taiga-ui/cdk';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { ActivatedRoute, Router } from '@angular/router';
import { WalletApiService } from '@shared/data-access/api/wallet-api.service';
import { tuiFormatNumber } from '@taiga-ui/core';
import { getFiltersFromParamMap } from '../../utils/filters-from-param-map';
import { filter, map, startWith, switchMap } from 'rxjs';
import { tuiFormatCurrency } from '@taiga-ui/addon-commerce';
import { distinctUntilChangedObject } from '@shared/utils/rxjs/distinct-until-changed-object';

@Component({
  selector: 'app-wallet-stats',
  templateUrl: './wallet-stats.component.html',
  styleUrls: ['./wallet-stats.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletStatsComponent {
  private readonly _walletId: Wallet['id'] =
    this._activatedRoute.snapshot.params['id'];

  readonly ui = {
    transactionDate: {
      items: DATE_RANGE_FILTER_GROUPS,
      maxLength: { month: 12 } as TuiDayLike,
      min: new TuiDay(1, 0, 1),
      max: new TuiDay(9999, 11, 31),
    } as const,
    dataApis: this._walletApiService.getConcreteWalletApi(this._walletId),
  };

  readonly filters = this._fb.filters({
    transactionDate: [DATE_RANGE_FILTER_GROUPS[5].range],
  });

  private readonly _showOnlyRows = 20;

  readonly stats$ = this.filters.filters$.pipe(
    filter(
      x => 'transactionDate_lte' in x && x['transactionDate_lte'] !== null
    ),
    distinctUntilChangedObject(),
    switchMap(payload =>
      this._walletApiService.getWalletStats(this._walletId, payload).pipe(
        map(({ expensesByCategory, currencyId }) => {
          const total = expensesByCategory.reduce((a, b) => a + b.expenses, 0);

          const list = expensesByCategory
            .map(x => ({
              id: x.id,
              label: x.id !== null ? x.name : 'Transactions without category',
              value: x.expenses,
              percentage: (x.expenses / total) * 100,
            }))
            .sort((a, b) => b.value - a.value)
            .reduce(
              (obj, x, i) => {
                if (i < this._showOnlyRows) {
                  obj.values.push(x.value);
                  obj.labels.push(x.label);
                  obj.ids.push(x.id ?? '00000000-0000-0000-0000-00000000000');
                  obj.percentages.push(x.percentage);
                } else {
                  obj.values[obj.values.length - 1] += x.value;
                  obj.labels[obj.labels.length - 1] = 'Other categories';
                  obj.percentages[obj.percentages.length - 1] =
                    (obj.values[obj.values.length - 1] / total) * 100;
                }

                return obj;
              },
              {
                ids: new Array<string>(),
                labels: new Array<string>(),
                values: new Array<number>(),
                percentages: new Array<number>(),
              }
            );

          let max = Math.max(...list.values);
          max =
            Math.ceil(max / 10 ** (max.toFixed(0).length - 1)) *
            10 ** (max.toFixed(0).length - 1);

          return {
            total,
            max,
            yLabels: list.labels,
            percentages: list.percentages,
            values: [list.values],
            ids: list.ids,
            xLabels: new Array(5)
              .fill(0)
              .map((_, i) => ((i + 1) * max) / 5)
              .map(
                x => `${tuiFormatNumber(x)} ${tuiFormatCurrency(currencyId)}`
              ),
            currencyId,
          };
        }),
        startWith(null)
      )
    )
  );

  click(event: MouseEvent, ids: string[]) {
    if (!(event.target instanceof HTMLElement)) {
      return;
    }

    const wrapper = event
      .composedPath()
      .find(
        x => x instanceof HTMLElement && x.className.includes('t-wrapper')
      ) as HTMLElement | undefined;

    if (wrapper === undefined || wrapper.parentElement === null) {
      return;
    }

    const idx = Array.prototype.indexOf.call(
      wrapper.parentElement.children,
      wrapper
    );

    this._router.navigate(['../transactions'], {
      relativeTo: this._activatedRoute,
      queryParams: {
        categories: [ids[idx]],
        transactionDate: [
          this.filters.form.controls.transactionDate.value.from.toJSON(),
          this.filters.form.controls.transactionDate.value.to.toJSON(),
        ],
      },
    });
  }

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _walletApiService: WalletApiService,
    private readonly _router: Router
  ) {
    this.filters.form.patchValue(
      getFiltersFromParamMap(
        this.filters.form,
        this._activatedRoute.snapshot.queryParamMap
      )
    );
  }
}
