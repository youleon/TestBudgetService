using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace TestBudgetService
{
    public class BudgetTests
    {
        private IBudgetRepo _budgetRepo;
        private static BudgetService _testBudgetService;

        [SetUp]
        public void Setup()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
        }
        [Test]
        public void QueryOneMonth()
        {
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

            _testBudgetService = new BudgetService(_budgetRepo);
            BudgetShouldBe(3100,
                new DateTime(2022, 10, 1),
                new DateTime(2022, 10, 31));
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
            _testBudgetService = new BudgetService(_budgetRepo);
            BudgetShouldBe(500,
                new DateTime(2022, 10, 1),
                    new DateTime(2022, 10, 5));
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

            _testBudgetService = new BudgetService(_budgetRepo);
            BudgetShouldBe(520,
                new DateTime(2022, 10, 30),
                new DateTime(2022, 11, 5));
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
            _testBudgetService = new BudgetService(_budgetRepo);
            BudgetShouldBe(3023, 
                new DateTime(2022, 10, 30), new DateTime(2022, 12, 3));
        }

        private static void BudgetShouldBe(int expected, DateTime startDate, DateTime endDate)
        {
            Assert.AreEqual(_testBudgetService.Query(startDate,
                endDate), expected);
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
            _testBudgetService = new BudgetService(_budgetRepo);
            BudgetShouldBe(0,
                new DateTime(2022, 12, 30),
                new DateTime(2022, 12, 3));
        }
        [Test]
        public void QueryNoBudgetData()
        {
            _budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            _budgetRepo.GetAll().Returns(new List<Budget>
            {
                
            });

            _testBudgetService = new BudgetService(_budgetRepo);
            BudgetShouldBe(0,
                new DateTime(2022, 1, 1),
                new DateTime(2022, 1, 30));
        }
    }
}