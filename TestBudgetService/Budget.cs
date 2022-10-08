using System;

namespace TestBudgetService
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
        //"202210"
        public DateTime GetYearMonth => Convert.ToDateTime(YearMonth);
    }
}