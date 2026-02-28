using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace uppgift2.service.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pages = table.Column<int>(type: "int", nullable: false),
                    PublishedYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberSince = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookCopies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    CopyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCopies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookCopies_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookCopyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_BookCopies_BookCopyId",
                        column: x => x.BookCopyId,
                        principalTable: "BookCopies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "ISBN", "Pages", "PublishedYear", "Title" },
                values: new object[,]
                {
                    { 1, "J.K. Rowling", "978-91-29-65843-7", 335, 1997, "Harry Potter och De vises sten" },
                    { 2, "J.R.R. Tolkien", "978-91-0-012345-6", 423, 1954, "Sagan om ringen: Härskarringen" },
                    { 3, "George Orwell", "978-91-7011-234-5", 328, 1949, "1984" },
                    { 4, "Jane Austen", "978-91-0-011122-3", 432, 1813, "Stolthet och fördom" },
                    { 5, "Suzanne Collins", "978-91-7429-123-4", 374, 2008, "Hungerspelen" },
                    { 6, "Astrid Lindgren", "978-91-29-65432-1", 224, 1973, "Bröderna Lejonhjärta" },
                    { 7, "Astrid Lindgren", "978-91-29-65111-2", 156, 1946, "Mästerdetektiven Blomkvist" },
                    { 8, "J.R.R. Tolkien", "978-91-0-012222-2", 310, 1937, "Hobbit" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "MemberSince", "Name" },
                values: new object[,]
                {
                    { 1, "anna@example.com", new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anna Andersson" },
                    { 2, "erik@example.com", new DateTime(2019, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Erik Eriksson" },
                    { 3, "maria@example.com", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maria Svensson" },
                    { 4, "johan@example.com", new DateTime(2020, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Johan Karlsson" },
                    { 5, "lisa@example.com", new DateTime(2022, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lisa Nilsson" }
                });

            migrationBuilder.InsertData(
                table: "BookCopies",
                columns: new[] { "Id", "BookId", "CopyNumber" },
                values: new object[,]
                {
                    { 1, 1, "HP-001" },
                    { 2, 1, "HP-002" },
                    { 3, 1, "HP-003" },
                    { 4, 2, "LOTR-001" },
                    { 5, 2, "LOTR-002" },
                    { 6, 3, "1984-001" },
                    { 7, 3, "1984-002" },
                    { 8, 4, "PP-001" },
                    { 9, 5, "HG-001" },
                    { 10, 5, "HG-002" },
                    { 11, 5, "HG-003" },
                    { 12, 6, "BL-001" },
                    { 13, 6, "BL-002" },
                    { 14, 7, "MB-001" },
                    { 15, 8, "HOB-001" },
                    { 16, 8, "HOB-002" }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "Id", "BookCopyId", "LoanDate", "ReturnDate", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 4, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 9, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 4, 6, new DateTime(2024, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 5, 2, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 6, 5, new DateTime(2024, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 7, 10, new DateTime(2024, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 8, 12, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 9, 15, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 10, 8, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2 },
                    { 11, 3, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 12, 11, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 13, 13, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3 },
                    { 14, 1, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 15, 9, new DateTime(2024, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 16, 16, new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4 },
                    { 17, 6, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 18, 14, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 19, 2, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_BookId",
                table: "BookCopies",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BookCopyId",
                table: "Loans",
                column: "BookCopyId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "BookCopies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
