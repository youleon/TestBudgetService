using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace TestBudgetService
{
    public class BudgetService
    {
        private readonly IBudgetRepo _repo;

        public BudgetService(IBudgetRepo repo)
        {
            _repo = repo;
        }

        public decimal Query(DateTime startDay, DateTime endDay)
        {
            if (startDay > endDay) return 0;

            var inPeriodBudgets = GetInPeriodBudgets(startDay, endDay);

            decimal result = 0;

            foreach (var monthBudget in inPeriodBudgets)
            {
                var daysInMonth = GetBudgetDaysInMonth(monthBudget);
                var dayBudget = monthBudget.Amount / (decimal)daysInMonth;
                int days;

                if (IsSameMonth(startDay, endDay))
                {
                    // (Partial Month) 計算起、訖同月的天數
                    var timeSpan = endDay.AddDays(1) - startDay;
                    days = (int)timeSpan.TotalDays;
                }
                else if (!IsSameMonth(startDay, endDay) && IsStartMonthBudget(startDay, monthBudget))
                {
                    // (Cross Month) 計算預算起始月份的天數
                    days = daysInMonth - startDay.Day + 1;
                }
                else if (!IsSameMonth(startDay, endDay) && IsEndMonthBudget(endDay, monthBudget))
                {
                    // (Cross Month) 計算預算結束月份的天數
                    days = endDay.Day;
                }
                else
                {
                    // (Cross Month) 計算跨月（整個月）的天數
                    days = daysInMonth;
                }

                result += dayBudget * days;
            }

            return result;
        }

        private static int GetBudgetDaysInMonth(Budget monthBudget)
        {
            return DateTime.DaysInMonth(monthBudget.GetYearMonth().Year, monthBudget.GetYearMonth().Month);
        }

        private static bool IsEndMonthBudget(DateTime end, Budget monthBudget)
        {
            return monthBudget.YearMonth == end.ToString("yyyyMM");
        }

        private static bool IsStartMonthBudget(DateTime start, Budget monthBudget)
        {
            return monthBudget.YearMonth == start.ToString("yyyyMM");
        }

        private static bool IsSameMonth(DateTime start, DateTime end)
        {
            return start.Year == end.Year && start.Month == end.Month;
        }

        private IEnumerable<Budget> GetInPeriodBudgets(DateTime start, DateTime end)
        {
            var budgets = _repo.GetAll();
            var startDate = DateTime.ParseExact(start.ToString("yyyyMM") + "01", "yyyyMMdd", CultureInfo.CurrentCulture);
            var endDate = DateTime.ParseExact(end.ToString("yyyyMM") + "01", "yyyyMMdd", CultureInfo.CurrentCulture);
            return budgets.Where(w => w.GetYearMonth() >= startDate && w.GetYearMonth() <= endDate);
        }
    }
}