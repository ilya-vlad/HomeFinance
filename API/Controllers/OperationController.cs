using Common.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{   
    public class OperationController : GenericController<OperationController>
    {
        public OperationController(IUnitOfWork unitOfWork, ILogger<OperationController> logger) 
            : base(unitOfWork, logger)
        {

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _unitOfWork.Operations.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {            
            var result = await _unitOfWork.Operations.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);                    
        }

        [HttpPost]
        public async Task<IActionResult> Add(Operation newOperation)
        {              
            var IsValidOperation = await IsValidOperationAsync(newOperation).ConfigureAwait(false);

            if (!IsValidOperation)
            {
                return BadRequest(ModelState);
            }

            var entity = new Operation
            {
                Date = newOperation.Date,
                Amount = newOperation.Amount,
                CategoryId = newOperation.CategoryId,
                IsIncome = newOperation.IsIncome,
                Description = newOperation.Description
            };

            await _unitOfWork.Operations.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {            
            var entity = await _unitOfWork.Operations.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            await _unitOfWork.Operations.DeleteAsync(entity.Id);
            await _unitOfWork.SaveAsync();

            return NoContent();  
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Operation operation)
        {            
            var IsValidOperation = await IsValidOperationAsync(operation).ConfigureAwait(false);

            if (id != operation.Id || !IsValidOperation)
            {
                return BadRequest();
            }

            var entity = await _unitOfWork.Operations.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Date = operation.Date;
            entity.IsIncome = operation.IsIncome;
            entity.Amount = operation.Amount;
            entity.CategoryId = operation.CategoryId;
            entity.Description = operation.Description;

            _unitOfWork.Operations.Update(entity);
            await _unitOfWork.SaveAsync();

            return NoContent();                     
        }

        private async Task<bool> IsValidOperationAsync(Operation operation)
        {
            if(operation == null)
            {
                return false;
            }

            OperationCategory category = null;

            if(operation.CategoryId != default)           
            {
                category = await _unitOfWork.Categories.GetByIdAsync(operation.CategoryId.Value);
            }            

            if (category == null)
            {
                ModelState.AddModelError(nameof(Operation.CategoryId), "Entered category is not exist");
            }

            if (operation.Date == default)
            {
                ModelState.AddModelError(nameof(Operation.Date), "Date is empty");
            }

            if (operation.Amount == default)
            {
                ModelState.AddModelError(nameof(Operation.Amount), "Amount is empty or equals zero");
            }

            return ModelState.IsValid;            
        }
    }
}
