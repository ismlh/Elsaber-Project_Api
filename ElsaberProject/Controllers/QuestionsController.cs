using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElsaberProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public QuestionsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        { 
            var query = await unitOfWork.Questions.GetAllAsync();
            if (query == null)
            {
                return NoContent();
            }
            return Ok(query);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var question = await unitOfWork.Questions.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPost]
           [Authorize(Roles = "superadmin")]

        public async Task<IActionResult> Post([FromBody] QuestionDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            var question = new Question
            {
                question = dto.question,
                Answer = dto.Answer
            };
            try
            {
                await unitOfWork.Questions.AddAsync(question);
                unitOfWork.Complete();
                return Ok(question);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id:int}")]
           [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Put(int id, [FromBody] QuestionDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            var question = await unitOfWork.Questions.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            question.question = dto.question;
            question.Answer = dto.Answer;
            try
            {
                await unitOfWork.Questions.UpdateAsync(question);
                unitOfWork.Complete();
                return Ok(question);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
           [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await unitOfWork.Questions.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            try
            {
                unitOfWork.Questions.Delete(question);
                unitOfWork.Complete();
                return Ok(question);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
