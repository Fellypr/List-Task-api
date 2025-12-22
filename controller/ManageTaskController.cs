using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lista_de_tarefa_api.data;
using Microsoft.AspNetCore.Mvc;
using lista_de_tarefa_api.model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace lista_de_tarefa_api.controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TakeOnTheTask : ControllerBase
    {
        private readonly AppDbContext _DbContext;

        public TakeOnTheTask(AppDbContext context)
        {
            _DbContext = context;
        }

        [HttpPost("registerTask")]
        public async Task<ActionResult> RegisterTask([FromBody] RegisterTaskDto task)
        {
            var idUserToken = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUserToken == null || !int.TryParse(idUserToken.Value, out int idUser))
            {
                return Unauthorized("O Id do Usuario nao foi encontrado");
            }
            if(string.IsNullOrWhiteSpace(task.NameTask) || string.IsNullOrWhiteSpace(task.DateTask.ToString()))
            {
                return BadRequest("O Nome da Tarefa ou a Data não pode estar Vazio.");
            }
            var dateInUtc = DateTime.SpecifyKind(task.DateTask, DateTimeKind.Utc);
            var checkTask = await _DbContext.ManageTasks.FirstOrDefaultAsync(x => x.NameTask == task.NameTask && x.IdUser == idUser && x.DateTask.Date == dateInUtc.Date);

            if (checkTask != null)
            {
                return Conflict("Já existe uma Tarefa Com Esse Nome Nessa Data.");
            }
            var NewTask = new ManageTask
            {
                NameTask = task.NameTask,
                DateTask = dateInUtc,
                Status = task.Status,
                IdUser = idUser

            };

            try
            {
                _DbContext.ManageTasks.Add(NewTask);
                await _DbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTask), new { id = NewTask.IdTask }, NewTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500,"Erro no servidor tente mais tarde.");
            }
        }
        [HttpGet("getAllTasks")]

        public async Task<ActionResult<IEnumerable<ManageTask>>> GetAllTasks([FromQuery] DateTime? targetDate)
        {
            try
            {
                var idUserToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (idUserToken == null || !int.TryParse(idUserToken, out int idUser))
                {
                    return Unauthorized("O Id do Usuario nao foi encontrado");
                }
                var dateInUtc = targetDate.HasValue ? DateTime.SpecifyKind(targetDate.Value, DateTimeKind.Utc) : (DateTime?)null;
                var searchTasks = await _DbContext.ManageTasks
                .Where(x => x.IdUser == idUser)

                .Where(t => dateInUtc == null || (t.DateTask.Date == dateInUtc.Value.Date))
                .ToListAsync();

                if (!searchTasks.Any())
                {
                    return Ok(new List<ManageTask>());
                }
                return Ok(searchTasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aqui estou: {ex.Message}");
                return StatusCode(500, ex.Message);
            }

        }


        [HttpGet("getTask/{id}")]

        public async Task<ActionResult<ManageTask>> GetTask(int id)
        {
            var idUserToken = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUserToken == null || !int.TryParse(idUserToken.Value, out int idUser))
            {
                return Unauthorized("O Id do Usuario nao foi encontrado");
            }

            var task = await _DbContext.ManageTasks.FirstOrDefaultAsync(x => x.IdTask == id && x.IdUser == idUser);

            if (task == null)
            {
                return NotFound();
            }
            return task;
        }




        [HttpPut("updateTask/{id}")]
        public async Task<ActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto updatedTask)
        {
            var idUserToken = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUserToken == null || !int.TryParse(idUserToken.Value, out int idUser))
            {
                return Unauthorized("O Id do Usuario nao foi encontrado");
            }
            var task = await _DbContext.ManageTasks.FirstOrDefaultAsync(x => x.IdTask == id && x.IdUser == idUser);
            if (task == null)
            {
                return NotFound();
            }
            task.Status = updatedTask.Status;

            try
            {
                _DbContext.ManageTasks.Update(task);
                await _DbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aqui estou: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("deleteTask/{id}")]

        public async Task<ActionResult> DeleteTask(int id)
        {
            var idUserToken = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUserToken == null || !int.TryParse(idUserToken.Value, out int idUser))
            {
                return Unauthorized("O Id do Usuario nao foi encontrado");
            }
            var task = await _DbContext.ManageTasks.FirstOrDefaultAsync(x => x.IdTask == id && x.IdUser == idUser);
            if (task == null)
            {
                return NotFound();
            }

            try
            {
                _DbContext.ManageTasks.Remove(task);
                await _DbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aqui estou: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}