# Swagger API Documentation

## Översikt

Din Biblioteks-API har nu fullständig Swagger/OpenAPI-dokumentation installerad.

## Hur man använder Swagger UI

### 1. Starta applikationen
```bash
dotnet run --project uppgift2.api
```

### 2. Öppna Swagger UI
Navigera till: `https://localhost:5001/swagger`

### 3. Utforska API:t

Swagger UI visar nu:

#### **Huvudbeskrivning**
```
API för hantering av böcker och låntagare samt metoder för:
• Vilka är de mest lånade böckerna?
• Hur många exemplar av en viss bok är för närvarande utlånade respektive tillgängliga?
• Vilka användare har lånat flest böcker under en viss tidsperiod?
• Vilka böcker har en enskild användare lånat under respektive tidsperiod?
• Vilka andra böcker har lånats av personer som lånat en viss bok?
• Ungefärlig läshastighet för en viss bok, uttryckt i sidor per dag
```

#### **Tre Controllers:**

1. **BooksController** - CRUD-operationer för böcker
   - Fullständiga beskrivningar
   - HTTP-statuskoder dokumenterade
   - Request/Response-exempel

2. **UsersController** - CRUD-operationer för användare
   - Fullständiga beskrivningar
   - HTTP-statuskoder dokumenterade
   - Request/Response-exempel

3. **LibraryController** - Biblioteksstatistik och analyser
   - Varje endpoint mappat till en specifik affärsfråga
   - Detaljerade beskrivningar
   - HTTP-statuskoder dokumenterade

## Funktioner i Swagger UI

### Try it out
1. Klicka på en endpoint
2. Klicka på "Try it out"
3. Fyll i parametrar
4. Klicka "Execute"
5. Se svaret direkt i gränssnittet

### HTTP Status Codes
Alla endpoints dokumenterar:
- ? **200 OK** - Lyckad förfrågan
- ? **201 Created** - Resurs skapad
- ? **204 No Content** - Lyckad borttagning
- ? **400 Bad Request** - Ogiltig data
- ? **404 Not Found** - Resursen finns inte

### Request/Response Models
- Expandera "Schemas" längst ner för att se alla datamodeller
- Varje endpoint visar vilka modeller som används
- Exempel på request bodies genereras automatiskt

## XML-kommentarer

Projektet är konfigurerat att generera XML-dokumentation från dina kod-kommentarer.

### Trippel-slash kommentarer används:
```csharp
/// <summary>
/// Beskrivning av metoden
/// </summary>
/// <param name="parameter">Parameterbeskrivning</param>
/// <returns>Vad metoden returnerar</returns>
/// <response code="200">Beskrivning av 200-svar</response>
/// <remarks>
/// Ytterligare information och exempel
/// </remarks>
```

## Swagger JSON

API-definitionen finns även som JSON på:
- `https://localhost:5001/swagger/v1/swagger.json`

Denna kan användas för:
- API-klienter (t.ex. Postman)
- Kodgenerering
- API-testning
- Integration med andra verktyg

## Anpassningar

### Ändra API-titel eller beskrivning
Redigera `Program.cs`:
```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Din titel",
        Version = "v1",
        Description = "Din beskrivning"
    });
});
```

### Lägg till fler kommentarer
Lägg till XML-kommentarer i dina controllers:
```csharp
/// <summary>
/// Din beskrivning här
/// </summary>
[HttpGet]
public async Task<IActionResult> MyMethod()
{
    // ...
}
```

## Producerad dokumentation

### LibraryController endpoints
Varje endpoint har nu en tydlig frågeformulering:
- "Vilka är de mest lånade böckerna?"
- "Hur många exemplar av en viss bok är för närvarande utlånade respektive tillgängliga?"
- osv.

Detta gör det enkelt för användare att förstå vad varje endpoint gör utan att behöva läsa kod.

## Tips

1. **Schemas-sektionen** - Använd detta för att se strukturen på alla DTOs
2. **Response examples** - Swagger genererar exempel automatiskt
3. **Authorization** - Kan läggas till senare om du implementerar autentisering
4. **Versioning** - Swagger stödjer API-versioner om du behöver det

## Nästa steg

- ? Swagger UI är konfigurerat
- ? XML-dokumentation aktiverad
- ? Alla controllers dokumenterade
- ? HTTP-statuskoder specificerade

Starta om din app och testa Swagger UI nu! ??
