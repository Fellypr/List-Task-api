using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lista_de_tarefa_api.model;

namespace lista_de_tarefa_api.Services
{
        public interface IJwtService
        {
            string GenerateToken(RegisterUser user);
        }
}