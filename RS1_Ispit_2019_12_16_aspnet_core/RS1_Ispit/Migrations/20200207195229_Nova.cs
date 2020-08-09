using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class Nova : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TakmicenjeUcesnik",
                columns: table => new
                {
                    TakmicenjeUcesnikId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TakmicenjeId = table.Column<int>(nullable: false),
                    DodjeljenPredmetId = table.Column<int>(nullable: false),
                    Bodovi = table.Column<float>(nullable: false),
                    Pristupio = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakmicenjeUcesnik", x => x.TakmicenjeUcesnikId);
                    table.ForeignKey(
                        name: "FK_TakmicenjeUcesnik_DodjeljenPredmet_DodjeljenPredmetId",
                        column: x => x.DodjeljenPredmetId,
                        principalTable: "DodjeljenPredmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TakmicenjeUcesnik_Takmicenje_TakmicenjeId",
                        column: x => x.TakmicenjeId,
                        principalTable: "Takmicenje",
                        principalColumn: "TakmicenjeId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TakmicenjeUcesnik_DodjeljenPredmetId",
                table: "TakmicenjeUcesnik",
                column: "DodjeljenPredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_TakmicenjeUcesnik_TakmicenjeId",
                table: "TakmicenjeUcesnik",
                column: "TakmicenjeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TakmicenjeUcesnik");
        }
    }
}
