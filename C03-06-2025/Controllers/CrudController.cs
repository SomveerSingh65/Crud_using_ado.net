using C03_06_2025.model;
using C03_06_2025.Repo.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace C03_06_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudController : ControllerBase
    {
        private readonly IRepository _repository;

        public CrudController(IRepository repository)
        {
            _repository = repository;
        }


        [HttpPost]
        public async Task<CommonModel> create([FromBody] modelRequest model)
        {
            try
            {
                var store = await _repository.create(model);
                return store;
            }
            catch (Exception ex)
            {
                return null;
            }



        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] modelRequest model)
        {
            try
            {
                var result = await _repository.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonModel
                {
                    status = 0,
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDetails()
        {
            try
            {
                var result = await _repository.GetAllDetails();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonModel
                {
                    status = 0,
                    message = ex.Message
                });
            }
        }

        [HttpGet("Get/{Id}")]
        public async Task<IActionResult> GetSingleDetails(int Id)
        {
            try
            {
                var result = await _repository.GetSingleDetails(Id);
                if (result == null)
                {
                    return NotFound(new CommonModel
                    {
                        status = 0,
                        message = "Model not found"
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonModel
                {
                    status = 0,
                    message = ex.Message
                });
            }
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var result = await _repository.Delete(Id);
                if (result.status == 0)
                {
                    return NotFound(new CommonModel
                    {
                        status = 0,
                        message = "Model not found"
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonModel
                {
                    status = 0,
                    message = ex.Message
                });
            }
        }


    }
}
