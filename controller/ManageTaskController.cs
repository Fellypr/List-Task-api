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
            if(idUserToken == null || !int.TryParse(idUserToken.Value, out int idUser))
            {
                return Unauthorized("O Id do Usuario nao foi encontrado");
            }
            var dateInUtc = DateTime.SpecifyKind(task.DateTask, DateTimeKind.Utc);
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
            }catch(Exception ex)
            {
                Console.WriteLine($"Aqui estou: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("getTask/{id}")]

        public async Task<ActionResult <ManageTask>>GetTask(int id)
        {
            var idUserToken = User.FindFirst(ClaimTypes.NameIdentifier);
            if(idUserToken == null || !int.TryParse(idUserToken.Value, out int idUser))
            {
                return Unauthorized("O Id do Usuario nao foi encontrado");
            }

            var task = await _DbContext.ManageTasks.FirstOrDefaultAsync(x => x.IdTask == id && x.IdUser == idUser);

            if(task == null)
            {
                return NotFound();
            }
            return task;
        }
        

    }

    public interface IActionResult<T>
    {
    }
}