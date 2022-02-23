using API.Controllers;
using Common.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Tests
{
    public class ReportControllerTests
    {
        [Test]
        public void GetDailyOperations_PassedEmptyDateAndStorageIsNOTContainsTodayOperations_ReturnsNotFoundResult()
        {
            var unitOfWork = new UnitOfWorkFake(new TestData());
            var loggerMock = new Mock<ILogger<ReportController>>();
            var logger = loggerMock.Object;
            var controller = new ReportController(unitOfWork, logger);

            var actionResult = controller.GetDailyOperations(default);

            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public void GetDailyOperations_PassedEmptyDateAndStorageIsContainsTodayOperations_ReturnsOkResult()
        {
            var unitOfWork = new UnitOfWorkFake(new TestData());
            var loggerMock = new Mock<ILogger<ReportController>>();
            var logger = loggerMock.Object;
            var controller = new ReportController(unitOfWork, logger);

            var op = unitOfWork.Operations.GetAll().First();
            op.Date = DateTime.Now;

            var actionResult = controller.GetDailyOperations(default);

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public void GetDailyOperations_PassedEmptyDateAndStorageIsContainsTodayOperations_ReturnsReportOperation()
        {
            var unitOfWork = new UnitOfWorkFake(new TestData());
            var loggerMock = new Mock<ILogger<ReportController>>();
            var logger = loggerMock.Object;
            var controller = new ReportController(unitOfWork, logger);

            var op1 = new Operation { Amount = 10, Date = DateTime.Now, IsIncome = false };
            var op2 = new Operation { Amount = 15, Date = DateTime.Now, IsIncome = false };
            var op3 = new Operation { Amount = 25, Date = DateTime.Now, IsIncome = true };

            unitOfWork.Operations.AddAsync(op1);
            unitOfWork.Operations.AddAsync(op2);
            unitOfWork.Operations.AddAsync(op3);

            var actionResult = controller.GetDailyOperations(default);
            var data = (actionResult as OkObjectResult).Value;
            var report = data as OperationReport;

            Assert.IsInstanceOf<OperationReport>(report);
            Assert.AreEqual(op1.Amount + op2.Amount, report.Expenses);
            Assert.AreEqual(op3.Amount, report.Income);
            Assert.AreEqual(3, report.Operations.Count());
        }

        [Test]
        public void GetDailyOperations_PassedValidDateAndStorageIsContainsTodayOperations_ReturnsReportOperation()
        {
            var unitOfWork = new UnitOfWorkFake(new TestData());
            var loggerMock = new Mock<ILogger<ReportController>>();
            var logger = loggerMock.Object;
            var controller = new ReportController(unitOfWork, logger);

            var op1 = new Operation { Amount = 10, Date = new DateTime(2000, 1, 1), IsIncome = false };
            var op2 = new Operation { Amount = 15, Date = new DateTime(2000, 1, 1), IsIncome = false };
            var op3 = new Operation { Amount = 25, Date = new DateTime(2000, 1, 1), IsIncome = true };

            unitOfWork.Operations.AddAsync(op1);
            unitOfWork.Operations.AddAsync(op2);
            unitOfWork.Operations.AddAsync(op3);

            var actionResult = controller.GetDailyOperations(new DateTime(2000, 1, 1));
            var data = (actionResult as OkObjectResult).Value;
            var report = data as OperationReport;

            Assert.IsInstanceOf<OperationReport>(report);
            Assert.AreEqual(op1.Amount + op2.Amount, report.Expenses);
            Assert.AreEqual(op3.Amount, report.Income);
            Assert.AreEqual(3, report.Operations.Count());
        }

        [Test]
        public void GetDailyOperations_PassedValidDateAndStorageIsNOTContainsTodayOperations_ReturnsNotFoundResult()
        {
            var unitOfWork = new UnitOfWorkFake(new TestData());
            var loggerMock = new Mock<ILogger<ReportController>>();
            var logger = loggerMock.Object;
            var controller = new ReportController(unitOfWork, logger);

            var actionResult = controller.GetDailyOperations(new DateTime(2000, 1, 1));            

            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public void GetRangeOperations_PassedEmptyParams_ReturnsBadRequest()
        {
            var unitOfWork = new UnitOfWorkFake(new TestData());
            var loggerMock = new Mock<ILogger<ReportController>>();
            var logger = loggerMock.Object;
            var controller = new ReportController(unitOfWork, logger);

            var actionResult = controller.GetRangeOperations(default, default);

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public void GetRangeOperations_PassedEmptyFirstParam_ReturnsBadRequest()
        {
            var unitOfWork = new UnitOfWorkFake(new TestData());
            var loggerMock = new Mock<ILogger<ReportController>>();
            var logger = loggerMock.Object;
            var controller = new ReportController(unitOfWork, logger);

            var actionResult = controller.GetRangeOperations(default, DateTime.UtcNow);

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public void GetRangeOperations_PassedEmptySecondParam_ReturnsBadRequest()
        {
            var unitOfWork = new UnitOfWorkFake(new TestData());
            var loggerMock = new Mock<ILogger<ReportController>>();
            var logger = loggerMock.Object;
            var controller = new ReportController(unitOfWork, logger);

            var actionResult = controller.GetRangeOperations(DateTime.UtcNow, default);

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public void GetRangeOperations_PassedFirstParamBiggerThanSecondParam_ReturnsBadRequest()
        {
            var unitOfWork = new UnitOfWorkFake(new TestData());
            var loggerMock = new Mock<ILogger<ReportController>>();
            var logger = loggerMock.Object;
            var controller = new ReportController(unitOfWork, logger);

            var actionResult = controller
                .GetRangeOperations(DateTime.UtcNow + TimeSpan.FromDays(1), DateTime.UtcNow);

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public void GetRangeOperations_PassedValidParamsAndStorageIsContainsOperations_ReturnsReportOperation()
        {
            var unitOfWork = new UnitOfWorkFake(new TestData());
            var loggerMock = new Mock<ILogger<ReportController>>();
            var logger = loggerMock.Object;
            var controller = new ReportController(unitOfWork, logger);

            var op1 = new Operation { Amount = 10, Date = new DateTime(2000, 1, 1), IsIncome = false };
            var op2 = new Operation { Amount = 15, Date = new DateTime(2000, 1, 2), IsIncome = false };
            var op3 = new Operation { Amount = 25, Date = new DateTime(2000, 1, 3), IsIncome = true };

            unitOfWork.Operations.AddAsync(op1);
            unitOfWork.Operations.AddAsync(op2);
            unitOfWork.Operations.AddAsync(op3);

            var actionResult = controller.GetRangeOperations(new DateTime(2000, 1, 1), new DateTime(2000, 1, 3));

            var data = (actionResult as OkObjectResult).Value;
            var report = data as OperationReport;

            Assert.IsInstanceOf<OperationReport>(report);
            Assert.AreEqual(op1.Amount + op2.Amount, report.Expenses);
            Assert.AreEqual(op3.Amount, report.Income);
            Assert.AreEqual(3, report.Operations.Count());
        }
    }
}
