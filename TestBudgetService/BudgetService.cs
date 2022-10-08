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
            var list = GetList(start, end);
            //foreach (var VARIABLE in COLLECTION)
            //{
                
            //}

            return list.Count;
        }

        private List<Budget> GetList(DateTime start, DateTime end)
        {
            var budgets = _repo.GetAll();
            var startDate = DateTime.ParseExact(start.ToString("yyyyMM") + "01", "yyyyMMdd", CultureInfo.CurrentCulture);
            var endDate = DateTime.ParseExact(end.ToString("yyyyMM") + "01", "yyyyMMdd", CultureInfo.CurrentCulture);
            var lists = budgets.Where(w => w.GetYearMonth() >= startDate && w.GetYearMonth() <= endDate);
            return lists.ToList();
        }
    }
}