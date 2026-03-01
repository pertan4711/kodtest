using LibraryService.Interfaces;
using Microsoft.EntityFrameworkCore;
using uppgift2.service.Data;
using uppgift2.service.GrpcServices;
using uppgift2.service.Services;

var builder = WebApplication.CreateBuilder(args);

// Lägg till gRPC
builder.Services.AddGrpc();

// Konfigurera databas
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;MultipleActiveResultSets=true"));

// Registrera befintliga services (EF Core-baserade)
builder.Services.AddScoped<ILibraryService, uppgift2.service.Services.LibraryService>();
builder.Services.AddScoped<IBookService, uppgift2.service.Services.BookService>();
builder.Services.AddScoped<IUserService, uppgift2.service.Services.UserService>();

var app = builder.Build();

// Migrera och seeda databasen vid start
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    context.Database.Migrate();
}

// Mappa gRPC-tjänster
app.MapGrpcService<LibraryGrpcService>();
app.MapGrpcService<BookGrpcService>();
app.MapGrpcService<UserGrpcService>();

app.MapGet("/", () => "gRPC Library Service is running on port 5002. Use a gRPC client to connect.");

app.Run();

