using Microsoft.EntityFrameworkCore;
using uppgift2.service.Data;
using uppgift2.service.Services;

var builder = WebApplication.CreateBuilder(args);

// Lägg till services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Konfigurera databas
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;MultipleActiveResultSets=true"));

// Registrera services
builder.Services.AddScoped<ILibraryService, LibraryService>();

var app = builder.Build();

// Migrera och seeda databasen vid start
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    context.Database.Migrate();
}

// Konfigurera HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

