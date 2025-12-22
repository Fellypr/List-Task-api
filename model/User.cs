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
        public ICollection<ManageTask> Tasks { get; set; } = new List<ManageTask>();
    }
    public class RegisterUserDto
    {        
        public string Name { get; set; }    
        [EmailAddress(ErrorMessage = "Formato de Email Inválido.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Use o Email do Gmail.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Senha é Obrigatório.")]
        [MinLength(6, ErrorMessage = "A Senha deve ter no mínimo 6 caracteres.")]
        [MaxLength(20, ErrorMessage = "A Senha deve ter no máximo 20 caracteres.")]
        public string Password { get; set; }
    }
    public class LoginUserDto
    {
        [EmailAddress(ErrorMessage = "Formato de Email Inválido.")]
        [Required(ErrorMessage = "O campo Email é Obrigatório.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Senha é Obrigatório.")]
        [MinLength(6, ErrorMessage = "A Senha deve ter no mínimo 6 caracteres.")]   
        public string Password { get; set; }
    }
}