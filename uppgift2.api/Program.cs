using Microsoft.OpenApi.Models;

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
    options.SwaggerDoc("v1", new OpenApiInfo
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
        Contact = new OpenApiContact
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

// Registrera gRPC-klienter
var grpcServerUrl = builder.Configuration.GetValue<string>("GrpcServer:Url") ?? "https://localhost:5002";

builder.Services.AddGrpcClient<LibraryService.Grpc.Library.LibraryService.LibraryServiceClient>(o =>
{
    o.Address = new Uri(grpcServerUrl);
});

builder.Services.AddGrpcClient<LibraryService.Grpc.Books.BookService.BookServiceClient>(o =>
{
    o.Address = new Uri(grpcServerUrl);
});

builder.Services.AddGrpcClient<LibraryService.Grpc.Users.UserService.UserServiceClient>(o =>
{
    o.Address = new Uri(grpcServerUrl);
});

var app = builder.Build();

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

