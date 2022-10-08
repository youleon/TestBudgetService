using System;
using System.Globalization;

namespace TestBudgetService
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
        public DateTime GetYearMonth() => DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", CultureInfo.CurrentCulture);
    }
}