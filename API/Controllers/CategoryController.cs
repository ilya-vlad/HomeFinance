using Common.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class CategoryController : GenericController<CategoryController>
    {
        public CategoryController(IUnitOfWork unitOfWork, ILogger<CategoryController> logger) 
            : base(unitOfWork, logger)
        {

        }

        [HttpGet]
        public IActionResult GetAll()
        {            
            var result = _unitOfWork.Categories.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _unitOfWork.Categories.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OperationCategory newCategory)
        {
            var entity = new OperationCategory
            {
                Name = newCategory.Name
            };

            await _unitOfWork.Categories.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.Categories.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            await _unitOfWork.Categories.DeleteAsync(entity.Id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OperationCategory category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            var entity = await _unitOfWork.Categories.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = category.Name;

            _unitOfWork.Categories.Update(entity);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
