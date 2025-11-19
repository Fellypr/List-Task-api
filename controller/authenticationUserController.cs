using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using lista_de_tarefa_api.data;
using lista_de_tarefa_api.model;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
namespace lista_de_tarefa_api.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationUser : ControllerBase
    {
        private readonly AppDbContext _DbContext;
        public AuthenticationUser(AppDbContext context)
        {
            _DbContext = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult> AuthenticateUser([FromBody] RegisterUser user)
        {
            if (String.IsNullOrWhiteSpace(user.Email) || String.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Invalid client request");
            }
            try
            {
                var userFound = await _DbContext.RegisterUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
                var userNameCheck = await _DbContext.RegisterUsers.FirstOrDefaultAsync(x => x.Name == user.Name);

                if (userFound != null)
                {
                    return Conflict("Usuario já cadastrado Com Esse Email");
                }else if(userNameCheck != null)
                {
                    return Conflict("Usuario já cadastrado Com Esse Nome");
                }
                else
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    _DbContext.RegisterUsers.Add(user);
                    await _DbContext.SaveChangesAsync();
                    return Ok(user);          
                }
                
                
            }catch(Exception ex)
            {
                Console.WriteLine("Aqui o error" + ex.Message); 
                return StatusCode(500, ex.Message);
                
            }
            
        }
        [HttpPost("Login")]

        public async Task<ActionResult> LoginUser([FromBody] RegisterUser user)
        {
            if (String.IsNullOrWhiteSpace(user.Email) || String.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Prencha todos os campos");
            }

            try
            {
                var checkEmail = await _DbContext.RegisterUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

                if(checkEmail == null)
                {
                    return NotFound("Usuario não encontrado");
                }
                else if(BCrypt.Net.BCrypt.Verify(user.Password, checkEmail.Password))
                {
                    return Ok("Login efetuado com sucesso");
                }else
                {
                    return BadRequest("Email ou Senha incorreta");
                }
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
    }
}