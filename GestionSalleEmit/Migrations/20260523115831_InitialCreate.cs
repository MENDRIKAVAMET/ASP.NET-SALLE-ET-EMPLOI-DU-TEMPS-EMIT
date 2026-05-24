using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionSalleEmit.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enseignants",
                columns: table => new
                {
                    IdEnseignant = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomEnseignant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrenomEnseignant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailEnseignant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneEnseignant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeEnseignant = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enseignants", x => x.IdEnseignant);
                });

            migrationBuilder.CreateTable(
                name: "Filieres",
                columns: table => new
                {
                    IdFiliere = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomFiliere = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filieres", x => x.IdFiliere);
                });

            migrationBuilder.CreateTable(
                name: "Matieres",
                columns: table => new
                {
                    IdMatiere = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomMatiere = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VolumeHoraire = table.Column<int>(type: "int", nullable: false),
                    Semestre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coefficient = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matieres", x => x.IdMatiere);
                });

            migrationBuilder.CreateTable(
                name: "Salles",
                columns: table => new
                {
                    IdSalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomSalle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacite = table.Column<int>(type: "int", nullable: false),
                    TypeSalle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salles", x => x.IdSalle);
                });

            migrationBuilder.CreateTable(
                name: "Parcours",
                columns: table => new
                {
                    IdParcours = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomParcours = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdFiliere = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcours", x => x.IdParcours);
                    table.ForeignKey(
                        name: "FK_Parcours_Filieres_IdFiliere",
                        column: x => x.IdFiliere,
                        principalTable: "Filieres",
                        principalColumn: "IdFiliere",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enseigners",
                columns: table => new
                {
                    IdEnseignant = table.Column<int>(type: "int", nullable: false),
                    IdMatiere = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enseigners", x => new { x.IdEnseignant, x.IdMatiere });
                    table.ForeignKey(
                        name: "FK_Enseigners_Enseignants_IdEnseignant",
                        column: x => x.IdEnseignant,
                        principalTable: "Enseignants",
                        principalColumn: "IdEnseignant",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enseigners_Matieres_IdMatiere",
                        column: x => x.IdMatiere,
                        principalTable: "Matieres",
                        principalColumn: "IdMatiere",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Niveaux",
                columns: table => new
                {
                    IdNiveau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomNiveau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdParcours = table.Column<int>(type: "int", nullable: false),
                    FiliereIdFiliere = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Niveaux", x => x.IdNiveau);
                    table.ForeignKey(
                        name: "FK_Niveaux_Filieres_FiliereIdFiliere",
                        column: x => x.FiliereIdFiliere,
                        principalTable: "Filieres",
                        principalColumn: "IdFiliere");
                    table.ForeignKey(
                        name: "FK_Niveaux_Parcours_IdParcours",
                        column: x => x.IdParcours,
                        principalTable: "Parcours",
                        principalColumn: "IdParcours",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmploisDuTemps",
                columns: table => new
                {
                    IdEDT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jour = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HeureDebut = table.Column<TimeSpan>(type: "time", nullable: false),
                    HeureFin = table.Column<TimeSpan>(type: "time", nullable: false),
                    Semestre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdSalle = table.Column<int>(type: "int", nullable: false),
                    IdEnseignant = table.Column<int>(type: "int", nullable: false),
                    IdMatiere = table.Column<int>(type: "int", nullable: false),
                    IdNiveau = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploisDuTemps", x => x.IdEDT);
                    table.ForeignKey(
                        name: "FK_EmploisDuTemps_Enseignants_IdEnseignant",
                        column: x => x.IdEnseignant,
                        principalTable: "Enseignants",
                        principalColumn: "IdEnseignant",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmploisDuTemps_Matieres_IdMatiere",
                        column: x => x.IdMatiere,
                        principalTable: "Matieres",
                        principalColumn: "IdMatiere",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmploisDuTemps_Niveaux_IdNiveau",
                        column: x => x.IdNiveau,
                        principalTable: "Niveaux",
                        principalColumn: "IdNiveau",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmploisDuTemps_Salles_IdSalle",
                        column: x => x.IdSalle,
                        principalTable: "Salles",
                        principalColumn: "IdSalle",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmploisDuTemps_IdEnseignant",
                table: "EmploisDuTemps",
                column: "IdEnseignant");

            migrationBuilder.CreateIndex(
                name: "IX_EmploisDuTemps_IdMatiere",
                table: "EmploisDuTemps",
                column: "IdMatiere");

            migrationBuilder.CreateIndex(
                name: "IX_EmploisDuTemps_IdNiveau",
                table: "EmploisDuTemps",
                column: "IdNiveau");

            migrationBuilder.CreateIndex(
                name: "IX_EmploisDuTemps_IdSalle",
                table: "EmploisDuTemps",
                column: "IdSalle");

            migrationBuilder.CreateIndex(
                name: "IX_Enseigners_IdMatiere",
                table: "Enseigners",
                column: "IdMatiere");

            migrationBuilder.CreateIndex(
                name: "IX_Niveaux_FiliereIdFiliere",
                table: "Niveaux",
                column: "FiliereIdFiliere");

            migrationBuilder.CreateIndex(
                name: "IX_Niveaux_IdParcours",
                table: "Niveaux",
                column: "IdParcours");

            migrationBuilder.CreateIndex(
                name: "IX_Parcours_IdFiliere",
                table: "Parcours",
                column: "IdFiliere");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmploisDuTemps");

            migrationBuilder.DropTable(
                name: "Enseigners");

            migrationBuilder.DropTable(
                name: "Niveaux");

            migrationBuilder.DropTable(
                name: "Salles");

            migrationBuilder.DropTable(
                name: "Enseignants");

            migrationBuilder.DropTable(
                name: "Matieres");

            migrationBuilder.DropTable(
                name: "Parcours");

            migrationBuilder.DropTable(
                name: "Filieres");
        }
    }
}
