using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using lista_de_tarefa_api.data;
using lista_de_tarefa_api.model;
using lista_de_tarefa_api.Services;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace lista_de_tarefa_api.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationUser : ControllerBase
    {
        private readonly AppDbContext _DbContext;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthenticationUser> _logger;

        public AuthenticationUser(AppDbContext context, IJwtService jwtService, ILogger<AuthenticationUser> logger)
        {
            _DbContext = context;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> AuthenticateUser([FromBody] RegisterUserDto user)
        {
            if (String.IsNullOrWhiteSpace(user.Email) || String.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Invalid client request");
            }
            var newUser = new RegisterUser
            {
                Name = user.Name,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            };
            try
            {
                var userFound = await _DbContext.RegisterUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
                var userNameCheck = await _DbContext.RegisterUsers.FirstOrDefaultAsync(x => x.Name == user.Name);


                if (userFound != null)
                {
                    return Conflict("Usuario já cadastrado Com Esse Email");
                }
                else if (userNameCheck != null)
                {
                    return Conflict("Usuario já cadastrado Com Esse Nome");
                }
                else
                {
                    _DbContext.RegisterUsers.Add(newUser);
                    await _DbContext.SaveChangesAsync();
                    var token = _jwtService.GenerateToken(newUser);
                    return StatusCode(201, new
                    {
                        token = token,
                        user = new
                        {
                            id = newUser.IdUser,
                            name = newUser.Name
                        },
                        message = $"Seja Bem Vindo {newUser.Name}"

                    });
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Aqui o error" + ex.Message);
                return StatusCode(500, ex.Message);

            }

        }
        [HttpPost("login")]

        public async Task<ActionResult> LoginUser([FromBody] LoginUserDto user)
        {
            if (String.IsNullOrWhiteSpace(user.Email) || String.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Prencha todos os campos");
            }

            try
            {
                var checkEmail = await _DbContext.RegisterUsers.FirstOrDefaultAsync(x => x.Email == user.Email);


                if (checkEmail == null)
                {
                    return NotFound("Usuario não encontrado");
                }
                else if (BCrypt.Net.BCrypt.Verify(user.Password, checkEmail.Password))
                {
                    var token = _jwtService.GenerateToken(checkEmail);
                    return Ok(new
                    {
                        token = token,
                        user = new
                        {
                            id = checkEmail.IdUser,
                            name = checkEmail.Name
                        }
                    });

                }
                else
                {
                    return BadRequest("Email ou Senha incorreta");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> InfoUser()
        {
            var idUserToken = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUserToken == null || !int.TryParse(idUserToken.Value, out int idUser))
            {
                return Unauthorized("O Id do Usuario nao foi encontrado");
            }

            try
            {
                var user = await _DbContext.RegisterUsers.Where(u => u.IdUser == idUser).FirstOrDefaultAsync();
                if (user == null)
                {
                    return NotFound("Usuario nao encontrado");
                }

                return Ok(new
                {
                    id = user.IdUser,
                    name = user.Name,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}