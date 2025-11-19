
using lista_de_tarefa_api.data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("defaultConnection"))
);


builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseCors("PermitirFrontend");

app.UseAuthorization();
app.MapControllers();

app.Run();