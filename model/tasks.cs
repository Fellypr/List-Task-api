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
        public string NameTask { get; set; }
        public string Status { get; set; }
        public DateTime DateTask { get; set; }
    }
}