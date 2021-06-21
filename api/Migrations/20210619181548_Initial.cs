using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class Initial : Migration
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
                    knjigaId = table.Column<int>(type: "INTEGER", nullable: false),
                    korisnikId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    stanje = table.Column<int>(type: "INTEGER", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "KnjigaPosetilac",
                columns: table => new
                {
                    knjigeid = table.Column<int>(type: "INTEGER", nullable: false),
                    korisniciid = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnjigaPosetilac", x => new { x.knjigeid, x.korisniciid });
                    table.ForeignKey(
                        name: "FK_KnjigaPosetilac_Knjige_knjigeid",
                        column: x => x.knjigeid,
                        principalTable: "Knjige",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KnjigaPosetilac_Korisnici_korisniciid",
                        column: x => x.korisniciid,
                        principalTable: "Korisnici",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KnjigaPosetilac_korisniciid",
                table: "KnjigaPosetilac",
                column: "korisniciid");

            migrationBuilder.CreateIndex(
                name: "IX_Knjige_autorid",
                table: "Knjige",
                column: "autorid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnjigaPosetilac");

            migrationBuilder.DropTable(
                name: "Rezervacije");

            migrationBuilder.DropTable(
                name: "Knjige");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Autori");
        }
    }
}
