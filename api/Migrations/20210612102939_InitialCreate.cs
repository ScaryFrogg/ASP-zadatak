using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autori",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ime = table.Column<string>(type: "TEXT", nullable: true),
                    prezime = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autori", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", nullable: true),
                    password = table.Column<string>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacije",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    knjigaId = table.Column<string>(type: "TEXT", nullable: true),
                    korisnikId = table.Column<string>(type: "TEXT", nullable: true),
                    odobrena = table.Column<bool>(type: "INTEGER", nullable: false),
                    istek = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacije", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Knjige",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    naziv = table.Column<string>(type: "TEXT", nullable: true),
                    autorid = table.Column<int>(type: "INTEGER", nullable: true),
                    stanje = table.Column<int>(type: "INTEGER", nullable: false),
                    Posetilacid = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knjige", x => x.id);
                    table.ForeignKey(
                        name: "FK_Knjige_Autori_autorid",
                        column: x => x.autorid,
                        principalTable: "Autori",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Knjige_Korisnici_Posetilacid",
                        column: x => x.Posetilacid,
                        principalTable: "Korisnici",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Knjige_autorid",
                table: "Knjige",
                column: "autorid");

            migrationBuilder.CreateIndex(
                name: "IX_Knjige_Posetilacid",
                table: "Knjige",
                column: "Posetilacid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Knjige");

            migrationBuilder.DropTable(
                name: "Rezervacije");

            migrationBuilder.DropTable(
                name: "Autori");

            migrationBuilder.DropTable(
                name: "Korisnici");
        }
    }
}
