using Common.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace API.Controllers
{      
    public class ReportController : GenericController
    {
        public ReportController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet("Daily")]
        public IActionResult GetDailyOperations(DateTime date)
        {
            if(date == default)
            {
                date = DateTime.UtcNow;
            }

            var operations = _unitOfWork.Operations.GetAll()
                .Where( op => op.Date.Date == date.Date);

            if (!operations.Any())
            {
                return NotFound();
            }
            
            return Ok(GetOperationReport(operations));
        }

        [HttpGet("Range")]
        public IActionResult GetRangeOperations(DateTime from, DateTime to)
        {
            if (from == default || to == default || from > to)
            {
                return BadRequest("Invalid params 'from' or 'to'.");
            }

            var operations = _unitOfWork.Operations.GetAll()
                .Where(op => from.Date <= op.Date.Date && op.Date.Date <= to.Date);

            if (!operations.Any())
            {
                return NotFound();
            }

            return Ok(GetOperationReport(operations));
        }

        private OperationReport GetOperationReport(IQueryable<Operation> operations)
        {
            decimal income = operations.Where(op => op.IsIncome).Sum(op => op.Amount);
            decimal expenses = operations.Where(op => !op.IsIncome).Sum(op => op.Amount);

            var report = new OperationReport()
            {
                Income = income,
                Expenses = expenses,
                Operations = operations
            };

            return report;
        }
    }
}
