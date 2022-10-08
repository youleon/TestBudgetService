using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace TestBudgetService
{
    public class BudgetTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202210",
                    Amount = 3100,
                },
                new Budget
                {
                    YearMonth = "202211",
                    Amount = 3100,
                },
                new Budget
                {
                    YearMonth = "202212",
                    Amount = 3100,
                },
            });
            var testBudgetService = new BudgetService(budgetRepo);
            Assert.AreEqual(testBudgetService.Query(new DateTime(2022, 10, 2), new DateTime(2022, 11, 5)), 2);
        }
    }
}