using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace lista_de_tarefa_api.model
{
    public class RegisterUser
    {
        [Key]
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}