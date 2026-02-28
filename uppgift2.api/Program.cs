using Microsoft.EntityFrameworkCore;
using uppgift2.service.Data;
using uppgift2.service.Services;

var builder = WebApplication.CreateBuilder(args);

// Lägg till services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Biblioteks-API",
        Version = "v1",
        Description = @"<b>API för hantering av böcker och låntagare samt metoder för:</b><br /><br />
            • Vilka är de mest lånade böckerna?<br />
            • Hur många exemplar av en viss bok är för närvarande utlånade respektive tillgängliga?<br />
            • Vilka användare har lånat flest böcker under en viss tidsperiod?<br />
            • Vilka böcker har en enskild användare lånat under respektive tidsperiod?<br />
            • Vilka andra böcker har lånats av personer som lånat en viss bok?<br />
            • Ungefärlig läshastighet för en viss bok, uttryckt i sidor per dag<br />",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Biblioteks-API Support"
        }
    });

    // Aktivera XML-kommentarer om de finns
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Konfigurera databas
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;MultipleActiveResultSets=true"));

// Registrera services
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();

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
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteks-API v1");
        options.DocumentTitle = "Biblioteks-API Documentation";
        options.DefaultModelsExpandDepth(-1); // Dölj schemas som standard
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

