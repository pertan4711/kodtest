# Snabbstart - Biblioteks-API

## Starta applikationen

1. **Kör API:t**:
   ```bash
   dotnet run --project uppgift2.api
   ```

2. **Öppna Swagger UI**:
   - Öppna webbläsaren och gå till: `https://localhost:5001/swagger`
   - Här kan du testa alla API-endpoints direkt

## Testa API:t med exempel

### CRUD Operationer

#### Skapa en ny bok
```bash
POST https://localhost:5001/api/books
Content-Type: application/json

{
  "title": "Den lilla prinsen",
  "author": "Antoine de Saint-Exupéry",
  "isbn": "978-91-29-65000-0",
  "pages": 96,
  "publishedYear": 1943
}
```

#### Hämta alla böcker
```bash
GET https://localhost:5001/api/books
```

#### Uppdatera en bok
```bash
PUT https://localhost:5001/api/books/1
Content-Type: application/json

{
  "title": "Harry Potter och De vises sten (uppdaterad)",
  "author": "J.K. Rowling",
  "isbn": "978-91-29-65843-7",
  "pages": 335,
  "publishedYear": 1997
}
```

#### Ta bort en bok
```bash
DELETE https://localhost:5001/api/books/1
```

#### Skapa en ny användare
```bash
POST https://localhost:5001/api/users
Content-Type: application/json

{
  "name": "Kalle Karlsson",
  "email": "kalle@example.com",
  "memberSince": "2024-12-01T00:00:00"
}
```

### Biblioteksstatistik

### 1. Hämta mest lånade böcker
```bash
GET https://localhost:5001/api/library/most-borrowed?top=5
```

**Förväntat resultat**: Lista över de 5 mest lånade böckerna med antal lån.

### 2. Kolla tillgänglighet för Harry Potter (BookId = 1)
```bash
GET https://localhost:5001/api/library/books/1/availability
```

**Förväntat resultat**:
```json
{
  "bookId": 1,
  "title": "Harry Potter och De vises sten",
  "totalCopies": 3,
  "availableCopies": 2,
  "borrowedCopies": 1
}
```

### 3. Hitta mest aktiva låntagare Q1 2024
```bash
GET https://localhost:5001/api/library/top-borrowers?startDate=2024-01-01&endDate=2024-03-31&top=5
```

### 4. Se Anna Anderssons lånehistorik (UserId = 1)
```bash
GET https://localhost:5001/api/library/users/1/loan-history
```

### 5. Hitta böcker relaterade till Harry Potter (BookId = 1)
```bash
GET https://localhost:5001/api/library/books/1/related?top=5
```

**Detta visar vilka andra böcker som personer som läst Harry Potter också har lånat.**

### 6. Beräkna läshastighet för Harry Potter
```bash
GET https://localhost:5001/api/library/books/1/reading-speed
```

**Förväntat resultat**:
```json
{
  "bookId": 1,
  "title": "Harry Potter och De vises sten",
  "pages": 335,
  "averagePagesPerDay": 24.62,
  "completedLoans": 2
}
```

## Seeddata - Översikt

### Böcker i databasen
1. Harry Potter och De vises sten (3 exemplar)
2. Sagan om ringen: Härskarringen (2 exemplar)
3. 1984 (2 exemplar)
4. Stolthet och fördom (1 exemplar)
5. Hungerspelen (3 exemplar)
6. Bröderna Lejonhjärta (2 exemplar)
7. Mästerdetektiven Blomkvist (1 exemplar)
8. Hobbit (2 exemplar)

### Användare
1. Anna Andersson
2. Erik Eriksson (mest aktiv låntagare)
3. Maria Svensson
4. Johan Karlsson
5. Lisa Nilsson

### Aktiva lån (december 2024)
- Anna: 1984
- Erik: Stolthet och fördom
- Maria: Bröderna Lejonhjärta
- Johan: Hobbit
- Lisa: Harry Potter

## Vanliga frågor

**Q: Hur ändrar jag databasanslutningen?**
A: Redigera `appsettings.json` i uppgift2.api-projektet och ändra `ConnectionStrings:DefaultConnection`.

**Q: Databasen skapas inte?**
A: Kontrollera att LocalDB är installerat eller ändra connection string till din SQL Server-instans.

**Q: Hur lägger jag till mer testdata?**
A: Redigera `LibraryContext.cs` i metoden `OnModelCreating` och lägg till fler objekt i `HasData()`-anropen.

**Q: Kan jag använda en annan databas (t.ex. PostgreSQL)?**
A: Ja! Byt ut `UseSqlServer` mot `UseNpgsql` i `Program.cs` och uppdatera NuGet-paketen.
