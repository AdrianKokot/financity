import { TuiDay, TuiDayRange } from '@taiga-ui/cdk';
import { TuiDayRangePeriod } from '@taiga-ui/kit';

const createTransactionDateFilterGroups = () => {
  //https://github.com/Tinkoff/taiga-ui/blob/fdde12d70356bf5a018eed3c3e6747fff5adc8b0/projects/kit/utils/miscellaneous/create-default-day-range-periods.ts

  const today = TuiDay.currentLocal();
  const startOfWeek = today.append({ day: -today.dayOfWeek() });

  const endOfWeek = startOfWeek.append({ day: 6 });
  const startOfMonth = today.append({ day: 1 - today.day });
  const endOfMonth = startOfMonth.append({ month: 1, day: -1 });
  const startOfLastMonth = startOfMonth.append({ month: -1 });

  const startOfYear = startOfMonth.append({ month: -startOfMonth.month });

  return {
    today: new TuiDayRangePeriod(new TuiDayRange(today, today), 'Today'),

    'this week': new TuiDayRangePeriod(
      new TuiDayRange(startOfWeek, endOfWeek),
      'This week'
    ),

    'last week': new TuiDayRangePeriod(
      new TuiDayRange(
        startOfWeek.append({ day: -7 }),
        endOfWeek.append({ day: -7 })
      ),
      'Last week'
    ),

    'this month': new TuiDayRangePeriod(
      new TuiDayRange(startOfMonth, endOfMonth),
      'This month'
    ),

    'last month': new TuiDayRangePeriod(
      new TuiDayRange(startOfLastMonth, startOfMonth.append({ day: -1 })),
      'Last month'
    ),

    'last 30 days': new TuiDayRangePeriod(
      new TuiDayRange(today.append({ day: -30 }), today),
      'Last 30 days'
    ),

    'this year': new TuiDayRangePeriod(
      new TuiDayRange(startOfYear, startOfYear.append({ month: 12, day: -1 })),
      'This year'
    ),

    'last year': new TuiDayRangePeriod(
      new TuiDayRange(
        startOfYear.append({ year: -1 }),
        startOfYear.append({ day: -1 })
      ),
      'Last year'
    ),
  };
};

export const DATE_RANGE_FILTER_GROUPS_MAP = createTransactionDateFilterGroups();

export const DATE_RANGE_FILTER_GROUPS = Object.values(
  DATE_RANGE_FILTER_GROUPS_MAP
);
