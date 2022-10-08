using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace TestBudgetService
{
    public class BudgetTests
    {
        private IBudgetRepo _budgetRepo;

        [SetUp]
        public void Setup()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
        }
        [Test]
        public void QueryOneMonth()
        {
            _budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            _budgetRepo.GetAll().Returns(new List<Budget>
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
            var testBudgetService = new BudgetService(_budgetRepo);
            Assert.AreEqual(testBudgetService.Query(new DateTime(2022, 10, 1),
                new DateTime(2022, 10, 31)), 3100);
        }
        [Test]
        public void QueryPartialMonth()
        {
            _budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            _budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202210",
                    Amount = 3100,
                }
            });
            var testBudgetService = new BudgetService(_budgetRepo);
            Assert.AreEqual(testBudgetService.Query(new DateTime(2022, 10, 1),
                new DateTime(2022, 10, 5)), 500);
        }
        [Test]
        public void QueryCrossTwoMonth()
        {
            _budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            _budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202210",
                    Amount = 310,
                },
                new Budget
                {
                    YearMonth = "202211",
                    Amount = 3000,
                }
            });
            var testBudgetService = new BudgetService(_budgetRepo);
            Assert.AreEqual(testBudgetService.Query(new DateTime(2022, 10, 30),
                new DateTime(2022, 11, 5)), 520);
        }
        [Test]
        public void QueryCrossThreeMonth()
        {
            _budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            _budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202210",
                    Amount = 310,
                },
                new Budget
                {
                    YearMonth = "202211",
                    Amount = 3000,
                },
                new Budget
                {
                    YearMonth = "202212",
                    Amount = 31,
                }
            });
            var testBudgetService = new BudgetService(_budgetRepo);
            Assert.AreEqual(testBudgetService.Query(new DateTime(2022, 10, 30),
                new DateTime(2022, 12, 3)), 3023);
        }
        [Test]
        public void QueryInvalidDate()
        {
            _budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            _budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202210",
                    Amount = 310,
                },
                new Budget
                {
                    YearMonth = "202211",
                    Amount = 3000,
                },
                new Budget
                {
                    YearMonth = "202212",
                    Amount = 31,
                }
            });
            var testBudgetService = new BudgetService(_budgetRepo);
            Assert.AreEqual(testBudgetService.Query(new DateTime(2022, 12, 30),
                new DateTime(2022, 12, 3)), 0);
        }
    }
}