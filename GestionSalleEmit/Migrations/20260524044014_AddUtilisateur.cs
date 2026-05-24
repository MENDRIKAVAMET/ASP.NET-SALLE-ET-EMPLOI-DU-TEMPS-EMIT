using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionSalleEmit.Migrations
{
    /// <inheritdoc />
    public partial class AddUtilisateur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    IdUtilisateur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomUtilisateur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailUtilisateur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotDePasseUtilisateur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleUtilisateur = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.IdUtilisateur);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Utilisateurs");
        }
    }
}
