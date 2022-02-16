using API.Controllers;
using Common.Models;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Tests
{
    public class OperationControllerTests
    {
        public OperationController GetController(IUnitOfWork unitOfWork) => 
            new OperationController(unitOfWork);
        
        public UnitOfWorkFake GetUnitOfWork() => 
            new UnitOfWorkFake(TestData.Operations, TestData.Categories);

       

        [Test]
        public void GetAll_WhenCalled_ReturnsOkResult()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var actionResult = controller.GetAll();
            
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnsAllOperations()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var actionResult = controller.GetAll();
            var data = (actionResult as OkObjectResult).Value;
            var collection = data as IEnumerable<Operation>;
            
            var expected = TestData.Operations;

            Assert.AreEqual(expected.Count, collection.Count());
            Assert.AreEqual(expected.First().Amount, collection.First().Amount);
        }

        [Test]
        public void Get_PassedExistId_ReturnsOkResult()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var operation = TestData.Operations.Last();

            var actionResult = controller.Get(operation.Id).Result;

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public void Get_PassedWithNotExistId_ReturnsNotFoundResult()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var actionResult = controller.Get(int.MaxValue).Result;

            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public void Get_PassedWithExistId_ReturnsOperation()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var expected = TestData.Operations.Last();

            var actionResult = controller.Get(expected.Id).Result;
            var data = (actionResult as OkObjectResult).Value;
            var operation = data as Operation;
            
            Assert.IsInstanceOf<Operation>(operation);
            Assert.AreEqual(expected.Id, operation.Id);
            Assert.AreEqual(expected.Date, operation.Date);
            Assert.AreEqual(expected.Amount, operation.Amount);
            Assert.AreEqual(expected.Description, operation.Description);
            Assert.AreEqual(expected.CategoryId, operation.CategoryId);
        }

        [Test]
        public void Add_PassedValidObject_ReturnsCreatedResult()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            Operation op = new()
            {
                Id = 1000,
                Date = DateTime.UtcNow,
                Amount = 10m,
                Description = "123",
                CategoryId = TestData.Categories.First().Id,
                IsIncome = false
            };

            var actionResult = controller.Add(op).Result;

            Assert.IsInstanceOf<CreatedAtActionResult>(actionResult);
        }

        [Test]
        public void Add_PassedValidObject_AddedObjectToStorage()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            Operation expectedOp = new()
            {
                Id = 1000,
                Date = DateTime.UtcNow,
                Amount = 10m,
                Description = "123",
                CategoryId = TestData.Categories.First().Id,
                IsIncome = false
            };

            var storageBeforeAdd = unitOfWork.Operations.GetAll().ToList();

            var actionResult = controller.Add(expectedOp).Result;

            var storageAfterAdd = unitOfWork.Operations.GetAll().ToList();
            var addedOp = unitOfWork.Operations.GetAll().Last();

            Assert.AreEqual(storageBeforeAdd.Count + 1, storageAfterAdd.Count);
            Assert.AreEqual(expectedOp.Date, addedOp.Date);
            Assert.AreEqual(expectedOp.Amount, addedOp.Amount);
            Assert.AreEqual(expectedOp.Description, addedOp.Description);
            Assert.AreEqual(expectedOp.CategoryId, addedOp.CategoryId);
            Assert.AreEqual(expectedOp.IsIncome, addedOp.IsIncome);
        }

        [Test]
        public void Add_PassedInvalidObjectWithoutDate_ReturnsBadRequest()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            Operation expectedOp = new()
            {
                Id = 1000,                
                Amount = 10m,
                Description = "123",
                CategoryId = TestData.Categories.First().Id,
                IsIncome = false
            };

            var actionResult = controller.Add(expectedOp).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public void Add_PassedInvalidObjectWithoutAmout_ReturnsBadRequest()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            Operation expectedOp = new()
            {
                Id = 1000,
                Date = DateTime.UtcNow,                
                Description = "123",
                CategoryId = TestData.Categories.First().Id,
                IsIncome = false
            };

            var actionResult = controller.Add(expectedOp).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public void Add_PassedInvalidObjectWithoutCategoryId_ReturnsBadRequest()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            Operation expectedOp = new()
            {
                Id = 1000,
                Date = DateTime.UtcNow,
                Amount = 10m,
                Description = "123",                
                IsIncome = false
            };

            var actionResult = controller.Add(expectedOp).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public void Add_PassedValidObjectWithNotExistCategoryId_ReturnsBadRequest()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            Operation expectedOp = new()
            {
                Id = 1000,
                Date = DateTime.UtcNow,
                Amount = 10m,
                Description = "123",
                CategoryId = int.MaxValue,
                IsIncome = false
            };

            var actionResult = controller.Add(expectedOp).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public void Delete_PassedExistId_ReturnsNoContentResult()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var actionResult = controller.Delete(TestData.Operations.First().Id).Result;

            Assert.IsInstanceOf<NoContentResult>(actionResult);
        }

        [Test]
        public void Delete_PassedNotExistId_ReturnsNotFoundResult()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var actionResult = controller.Delete(int.MaxValue).Result;

            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public void Delete_PassedExistId_RemovedObjectFromStorage()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var storageBeforeRemove = unitOfWork.Operations.GetAll().ToList();

            var actionResult = controller.Delete(TestData.Operations.First().Id).Result;

            var storageAfterRemove = unitOfWork.Operations.GetAll().ToList();

            Assert.AreEqual(storageBeforeRemove.Count - 1, storageAfterRemove.Count);
        }

        [Test]
        public void Update_PassedValidRequest_ReturnsNoContentResult()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            Operation editedOp = TestData.Operations.First();

            var actionResult = controller.Update(editedOp.Id, editedOp).Result;

            Assert.IsInstanceOf<NoContentResult>(actionResult);
        }

        [Test]
        public void Update_PassedDifferentIdInRequestAndParam_ReturnsBadRequest()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            Operation editedOp = TestData.Operations.First();

            var actionResult = controller.Update(editedOp.Id + 1, editedOp).Result;

            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }

        [Test]
        public void Update_PassedInvalidObjectWithoutDate_ReturnsBadRequest()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var op = TestData.Operations.First();

            Operation editedOp = new()
            {
                Id = op.Id,
                Amount = op.Amount,
                Description = op.Description,
                CategoryId = op.CategoryId,
                IsIncome = op.IsIncome
            };

            var actionResult = controller.Update(editedOp.Id, editedOp).Result;

            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }

        [Test]
        public void Update_PassedInvalidObjectWithoutAmount_ReturnsBadRequest()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var op = TestData.Operations.First();

            Operation editedOp = new()
            {
                Id = op.Id,
                Date = op.Date,                
                Description = op.Description,
                CategoryId = op.CategoryId,
                IsIncome = op.IsIncome
            };

            var actionResult = controller.Update(editedOp.Id, editedOp).Result;

            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }

        [Test]
        public void Update_PassedInvalidObjectWithoutCategoryId_ReturnsBadRequest()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var op = TestData.Operations.First();

            Operation editedOp = new()
            {
                Id = op.Id,
                Date = op.Date,
                Amount = op.Amount,
                Description = op.Description,                
                IsIncome = op.IsIncome
            };

            var actionResult = controller.Update(editedOp.Id, editedOp).Result;

            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }

        [Test]
        public void Update_PassedInvalidObjectWithNotExistCategoryId_ReturnsBadRequest()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var op = TestData.Operations.First();

            Operation editedOp = new()
            {
                Id = op.Id,
                Date = op.Date,
                Amount = op.Amount,
                Description = op.Description,
                CategoryId = int.MaxValue,
                IsIncome = op.IsIncome
            };

            var actionResult = controller.Update(editedOp.Id, editedOp).Result;

            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }

        [Test]
        public void Update_PassedValidObject_EditedObjectInStorage()
        {
            var unitOfWork = GetUnitOfWork();
            var controller = GetController(unitOfWork);

            var storageBeforeEdited = unitOfWork.Operations.GetAll().ToList();

            var oldOp = storageBeforeEdited.Last();

            Operation expectedOp = new()
            {
                Id = oldOp.Id,
                Date = oldOp.Date + TimeSpan.FromDays(1),
                Amount = oldOp.Amount + 1,
                Description = oldOp.Description += "string",
                CategoryId = TestData.Categories.First( cat => cat.Id != oldOp.CategoryId).Id,
                IsIncome = !oldOp.IsIncome
            };            

            var actionResult = controller.Update(expectedOp.Id, expectedOp).Result;

            var storageAfterEdited = unitOfWork.Operations.GetAll().ToList();

            var editedOp = storageAfterEdited.First( op => op.Id == oldOp.Id );

            Assert.AreEqual(expectedOp.Id, editedOp.Id);
            Assert.AreEqual(expectedOp.Date, editedOp.Date);
            Assert.AreEqual(expectedOp.Amount, editedOp.Amount);
            Assert.AreEqual(expectedOp.Description, editedOp.Description);
            Assert.AreEqual(expectedOp.CategoryId, editedOp.CategoryId);
            Assert.AreEqual(expectedOp.IsIncome, editedOp.IsIncome);            
        }
    }
}