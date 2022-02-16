using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    public static class TestData
    {
        public static List<OperationCategory> Categories = new()
        {
            new OperationCategory { Id = 1, Name = "Foodstuffs" },
            new OperationCategory { Id = 2, Name = "Rent" },
            new OperationCategory { Id = 3, Name = "FastFood" },
            new OperationCategory { Id = 4, Name = "Hobby" },
            new OperationCategory { Id = 5, Name = "Other" },
            new OperationCategory { Id = 6, Name = "Salary" },
            new OperationCategory { Id = 7, Name = "Present" },
        };

        public static List<Operation> Operations = new()
        {
            new Operation { Id = 1, Amount = 10m, Date = new DateTime(2022, 2, 10), CategoryId = 1, IsIncome = false },
            new Operation { Id = 2, Amount = 2m, Date = new DateTime(2022, 2, 10), CategoryId = 3, IsIncome = false },
            new Operation { Id = 3, Amount = 220m, Date = new DateTime(2022, 2, 10), CategoryId = 4, IsIncome = false },
            new Operation { Id = 4, Amount = 40m, Date = new DateTime(2022, 2, 9), CategoryId = 1, IsIncome = false },
            new Operation { Id = 5, Amount = 140m, Date = new DateTime(2022, 2, 9), CategoryId = 5, IsIncome = false, Description = "bought kitchen table" },
            new Operation { Id = 6, Amount = 5m, Date = new DateTime(2022, 2, 9), CategoryId = 3, IsIncome = false },
            new Operation { Id = 7, Amount = 2m, Date = new DateTime(2022, 2, 8), CategoryId = 3, IsIncome = false },
            new Operation { Id = 8, Amount = 53m, Date = new DateTime(2022, 2, 7), CategoryId = 5, IsIncome = false },
            new Operation { Id = 9, Amount = 10m, Date = new DateTime(2022, 2, 7), CategoryId = 1, IsIncome = false },
            new Operation { Id = 10, Amount = 300m, Date = new DateTime(2022, 2, 6), CategoryId = 2, IsIncome = false },

            new Operation { Id = 11, Amount = 1000m, Date = new DateTime(2022, 2, 5), CategoryId = 6, IsIncome = true, Description = "salary!" },
            new Operation { Id = 12, Amount = 100m, Date = new DateTime(2022, 2, 5), CategoryId = 7, IsIncome = true, Description = "my friend returned the debt" }
        };
    }
}
