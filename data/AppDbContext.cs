using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lista_de_tarefa_api.model;

namespace lista_de_tarefa_api.data
{
    public class AppDbContext : DbContext
    {
        public DbSet<RegisterUser> RegisterUsers { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
    }
}