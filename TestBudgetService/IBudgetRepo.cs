using System.Collections.Generic;

namespace TestBudgetService
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}