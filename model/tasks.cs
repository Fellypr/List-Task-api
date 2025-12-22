using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace lista_de_tarefa_api.model
{
    public class ManageTask
    {
        [Key]
        public int IdTask { get; set; }
        public string NameTask { get; set; }
        public string Status { get; set; }
        public DateTime DateTask { get; set; }
        public int IdUser { get; set; }
        public RegisterUser RegisterUser { get; set; }
    }
    public class RegisterTaskDto
    {
        [Required(ErrorMessage = "O campo Nome da Tarefa é Obrigatório.")]
        [MinLength(3, ErrorMessage = "O Nome da Tarefa deve ter no mínimo 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "O Nome da Tarefa deve ter no máximo 100 caracteres.")]
        public string NameTask { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "O campo Data da Tarefa é Obrigatório.")]
        public DateTime DateTask { get; set; }
    }
    public class UpdateTaskDto
    {
        public string Status { get; set; }
    }
}