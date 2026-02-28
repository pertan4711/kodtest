# CRUD Operations Guide

## Books API

### GET /api/books
Hämtar alla böcker med deras exemplar.

**Response:**
```json
[
  {
    "id": 1,
    "title": "Harry Potter och De vises sten",
    "author": "J.K. Rowling",
    "isbn": "978-91-29-65843-7",
    "pages": 335,
    "publishedYear": 1997,
    "copies": [...]
  }
]
```

### GET /api/books/{id}
Hämtar en specifik bok med alla exemplar.

**Example:** `GET /api/books/1`

### POST /api/books
Skapar en ny bok.

**Request Body:**
```json
{
  "title": "Den lilla prinsen",
  "author": "Antoine de Saint-Exupéry",
  "isbn": "978-91-29-65000-0",
  "pages": 96,
  "publishedYear": 1943
}
```

**Response:** 201 Created
```json
{
  "id": 9,
  "title": "Den lilla prinsen",
  "author": "Antoine de Saint-Exupéry",
  "isbn": "978-91-29-65000-0",
  "pages": 96,
  "publishedYear": 1943,
  "copies": []
}
```

### PUT /api/books/{id}
Uppdaterar en befintlig bok.

**Request Body:**
```json
{
  "title": "Harry Potter och De vises sten (Specialutgåva)",
  "author": "J.K. Rowling",
  "isbn": "978-91-29-65843-7",
  "pages": 350,
  "publishedYear": 1997
}
```

**Response:** 200 OK med uppdaterad bok

### DELETE /api/books/{id}
Tar bort en bok.

**Response:** 204 No Content

---

## Users API

### GET /api/users
Hämtar alla användare med deras lånehistorik.

**Response:**
```json
[
  {
    "id": 1,
    "name": "Anna Andersson",
    "email": "anna@example.com",
    "memberSince": "2020-01-15T00:00:00",
    "loans": [...]
  }
]
```

### GET /api/users/{id}
Hämtar en specifik användare med fullständig lånehistorik.

**Example:** `GET /api/users/1`

### POST /api/users
Skapar en ny användare.

**Request Body:**
```json
{
  "name": "Kalle Karlsson",
  "email": "kalle@example.com",
  "memberSince": "2024-12-01T00:00:00"
}
```

**Response:** 201 Created
```json
{
  "id": 6,
  "name": "Kalle Karlsson",
  "email": "kalle@example.com",
  "memberSince": "2024-12-01T00:00:00",
  "loans": []
}
```

### PUT /api/users/{id}
Uppdaterar en befintlig användare.

**Request Body:**
```json
{
  "name": "Kalle Karlsson (Senior)",
  "email": "kalle.senior@example.com",
  "memberSince": "2024-12-01T00:00:00"
}
```

**Response:** 200 OK med uppdaterad användare

### DELETE /api/users/{id}
Tar bort en användare.

**Response:** 204 No Content

---

## Testning med cURL

### Skapa en bok
```bash
curl -X POST https://localhost:5001/api/books \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Den lilla prinsen",
    "author": "Antoine de Saint-Exupéry",
    "isbn": "978-91-29-65000-0",
    "pages": 96,
    "publishedYear": 1943
  }'
```

### Hämta alla böcker
```bash
curl https://localhost:5001/api/books
```

### Uppdatera en bok
```bash
curl -X PUT https://localhost:5001/api/books/1 \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Harry Potter - Uppdaterad",
    "author": "J.K. Rowling",
    "isbn": "978-91-29-65843-7",
    "pages": 335,
    "publishedYear": 1997
  }'
```

### Ta bort en bok
```bash
curl -X DELETE https://localhost:5001/api/books/9
```

---

## Testning med Swagger UI

1. Starta applikationen: `dotnet run --project uppgift2.api`
2. Öppna: `https://localhost:5001/swagger`
3. Expandera BooksController eller UsersController
4. Klicka på "Try it out" för önskad operation
5. Fyll i request body (för POST/PUT)
6. Klicka "Execute"

---

## Valideringsregler

### Book
- **Title**: Obligatorisk, får inte vara tom
- **Author**: Obligatorisk, får inte vara tom
- **ISBN**: Obligatorisk, får inte vara tom
- **Pages**: Måste vara > 0
- **PublishedYear**: Måste vara ett giltigt år

### User
- **Name**: Obligatorisk, får inte vara tom
- **Email**: Obligatorisk, måste vara giltig e-postadress
- **MemberSince**: Obligatorisk, datum

---

## HTTP Status Codes

- **200 OK** - Lyckad GET/PUT request
- **201 Created** - Lyckad POST request (skapad resurs)
- **204 No Content** - Lyckad DELETE request
- **400 Bad Request** - Ogiltig request data
- **404 Not Found** - Resursen hittades inte
- **500 Internal Server Error** - Serverfel
