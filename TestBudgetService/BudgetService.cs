using System;
using System.Linq;

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
            var budgets = _repo.GetAll();
            budgets.Where(x=>x.GetYearMonth.Year>=start.Year&&x.GetYearMonth.Month>=start.Month)

            return 0;
        }
    }
}