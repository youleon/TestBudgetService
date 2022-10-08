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

        public decimal Query(DateTime start, DateTime end)
        {
            if (start > end) return 0;

            var inPeriodBudgets = GetBudgets(start, end);

            decimal result =0;
            
            foreach (var budget in inPeriodBudgets)
            {
                var daysInMonth = DateTime.DaysInMonth(budget.GetYearMonth().Year, budget.GetYearMonth().Month);
                var dayBudget = budget.Amount / (decimal)daysInMonth;

                if (start.Year == end.Year && start.Month == end.Month)
                {
                    var current = start;
                    
                    while (end >= current)
                    {
                        result += dayBudget;
                        current = current.AddDays(1);
                    }

                    return result;
                }

                if (budget.YearMonth == start.ToString("yyyyMM"))
                {
                    result += dayBudget * (daysInMonth - start.Day+1);
                }
                else if (budget.YearMonth == end.ToString("yyyyMM"))
                {
                    result += dayBudget *  end.Day ;
                }
                else
                {
                    result += budget.Amount;
                }

            }

            return result;
        }

        private List<Budget> GetBudgets(DateTime start, DateTime end)
        {
            var budgets = _repo.GetAll();
            var startDate = DateTime.ParseExact(start.ToString("yyyyMM") + "01", "yyyyMMdd", CultureInfo.CurrentCulture);
            var endDate = DateTime.ParseExact(end.ToString("yyyyMM") + "01", "yyyyMMdd", CultureInfo.CurrentCulture);
            var inPeriodBudgets = budgets.Where(w => w.GetYearMonth() >= startDate && w.GetYearMonth() <= endDate);
            return inPeriodBudgets.ToList();
        }
    }
}