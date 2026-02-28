# Biblioteks-API

En ASP.NET Core Web API-applikation för att hantera ett biblioteks böcker och låntagare.

## Projektstruktur

- **uppgift2.api** - API-lager med HTTP endpoints
- **uppgift2.service** - Service-lager med affärslogik, datamodeller och DbContext

## Kom igång

### Förutsättningar
- .NET 10 SDK
- SQL Server eller LocalDB

### Installation och körning

1. **Bygg projekten**:
   ```bash
   dotnet build
   ```

2. **Skapa databasen** (migrations körs automatiskt vid start):
   ```bash
   cd uppgift2.api
   dotnet ef migrations add InitialCreate --project ../uppgift2.service
   ```

3. **Starta API:t**:
   ```bash
   dotnet run --project uppgift2.api
   ```

4. **Öppna Swagger UI**:
   Navigera till `https://localhost:5001/swagger` (eller den port som visas i terminalen)

## API Endpoints

### 1. Mest lånade böcker
```
GET /api/library/most-borrowed?top=10
```
Returnerar de mest lånade böckerna sorterade efter antal lån.

**Exempelanrop**:
```
GET https://localhost:5001/api/library/most-borrowed?top=5
```

### 2. Boktillgänglighet
```
GET /api/library/books/{bookId}/availability
```
Visar hur många exemplar av en bok som är tillgängliga respektive utlånade.

**Exempelanrop**:
```
GET https://localhost:5001/api/library/books/1/availability
```

### 3. Mest aktiva låntagare
```
GET /api/library/top-borrowers?startDate=2024-01-01&endDate=2024-12-31&top=10
```
Visar användare som lånat flest böcker under en viss tidsperiod.

**Exempelanrop**:
```
GET https://localhost:5001/api/library/top-borrowers?startDate=2024-01-01&endDate=2024-03-31&top=5
```

### 4. Användares lånehistorik
```
GET /api/library/users/{userId}/loan-history?startDate=2024-01-01&endDate=2024-12-31
```
Visar alla böcker som en användare har lånat, med valfri tidsfiltrering.

**Exempelanrop**:
```
GET https://localhost:5001/api/library/users/1/loan-history
GET https://localhost:5001/api/library/users/1/loan-history?startDate=2024-01-01&endDate=2024-03-31
```

### 5. Relaterade böcker
```
GET /api/library/books/{bookId}/related?top=10
```
Visar andra böcker som har lånats av personer som också lånat den angivna boken.

**Exempelanrop**:
```
GET https://localhost:5001/api/library/books/1/related?top=5
```

### 6. Läshastighet
```
GET /api/library/books/{bookId}/reading-speed
```
Beräknar genomsnittlig läshastighet i sidor per dag baserat på låneperioder.

**Exempelanrop**:
```
GET https://localhost:5001/api/library/books/1/reading-speed
```

## Datamodell

### Book (Bok)
- Id, Title, Author, ISBN, Pages, PublishedYear

### BookCopy (Bokexemplar)
- Id, BookId, CopyNumber

### User (Användare)
- Id, Name, Email, MemberSince

### Loan (Lån)
- Id, BookCopyId, UserId, LoanDate, ReturnDate

## Seeddata

Databasen innehåller förpopulerad data med:
- 8 böcker (Harry Potter, Sagan om ringen, 1984, etc.)
- 16 bokexemplar
- 5 användare
- 19 lån (både historiska och aktiva)

## Teknologier

- ASP.NET Core Web API (.NET 10)
- Entity Framework Core 9.0
- SQL Server
- Swagger/OpenAPI
