using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentAssist.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Odontologos",
                columns: table => new
                {
                    IdOdontologo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contrasenia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odontologos", x => x.IdOdontologo);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    IdPaciente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RUT = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.IdPaciente);
                });

            migrationBuilder.CreateTable(
                name: "Recepcionistas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasenia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recepcionistas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tratamientos",
                columns: table => new
                {
                    IdTratamiento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PrecioEstimado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tratamientos", x => x.IdTratamiento);
                });

            migrationBuilder.CreateTable(
                name: "PlanTratamientos",
                columns: table => new
                {
                    IdPlan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdPaciente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdOdontologo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTratamientos", x => x.IdPlan);
                    table.ForeignKey(
                        name: "FK_PlanTratamientos_Odontologos_IdOdontologo",
                        column: x => x.IdOdontologo,
                        principalTable: "Odontologos",
                        principalColumn: "IdOdontologo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanTratamientos_Pacientes_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "Pacientes",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    IdTurno = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DuracionMinutos = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdPaciente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdOdontologo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecepcionistaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.IdTurno);
                    table.ForeignKey(
                        name: "FK_Turnos_Odontologos_IdOdontologo",
                        column: x => x.IdOdontologo,
                        principalTable: "Odontologos",
                        principalColumn: "IdOdontologo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turnos_Pacientes_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "Pacientes",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turnos_Recepcionistas_RecepcionistaId",
                        column: x => x.RecepcionistaId,
                        principalTable: "Recepcionistas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PasoTratamientos",
                columns: table => new
                {
                    IdPaso = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdPlan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdTratamiento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaEstimada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasoTratamientos", x => x.IdPaso);
                    table.ForeignKey(
                        name: "FK_PasoTratamientos_PlanTratamientos_IdPlan",
                        column: x => x.IdPlan,
                        principalTable: "PlanTratamientos",
                        principalColumn: "IdPlan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PasoTratamientos_Tratamientos_IdTratamiento",
                        column: x => x.IdTratamiento,
                        principalTable: "Tratamientos",
                        principalColumn: "IdTratamiento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialTratamientos",
                columns: table => new
                {
                    IdHistorial = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdPaciente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdTratamiento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdOdontologo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaRealizada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdPasoTratamiento = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialTratamientos", x => x.IdHistorial);
                    table.ForeignKey(
                        name: "FK_HistorialTratamientos_Odontologos_IdOdontologo",
                        column: x => x.IdOdontologo,
                        principalTable: "Odontologos",
                        principalColumn: "IdOdontologo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialTratamientos_Pacientes_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "Pacientes",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialTratamientos_PasoTratamientos_IdPasoTratamiento",
                        column: x => x.IdPasoTratamiento,
                        principalTable: "PasoTratamientos",
                        principalColumn: "IdPaso");
                    table.ForeignKey(
                        name: "FK_HistorialTratamientos_Tratamientos_IdTratamiento",
                        column: x => x.IdTratamiento,
                        principalTable: "Tratamientos",
                        principalColumn: "IdTratamiento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialTratamientos_IdOdontologo",
                table: "HistorialTratamientos",
                column: "IdOdontologo");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialTratamientos_IdPaciente",
                table: "HistorialTratamientos",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialTratamientos_IdPasoTratamiento",
                table: "HistorialTratamientos",
                column: "IdPasoTratamiento");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialTratamientos_IdTratamiento",
                table: "HistorialTratamientos",
                column: "IdTratamiento");

            migrationBuilder.CreateIndex(
                name: "IX_PasoTratamientos_IdPlan",
                table: "PasoTratamientos",
                column: "IdPlan");

            migrationBuilder.CreateIndex(
                name: "IX_PasoTratamientos_IdTratamiento",
                table: "PasoTratamientos",
                column: "IdTratamiento");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTratamientos_IdOdontologo",
                table: "PlanTratamientos",
                column: "IdOdontologo");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTratamientos_IdPaciente",
                table: "PlanTratamientos",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdOdontologo",
                table: "Turnos",
                column: "IdOdontologo");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdPaciente",
                table: "Turnos",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_RecepcionistaId",
                table: "Turnos",
                column: "RecepcionistaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialTratamientos");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "PasoTratamientos");

            migrationBuilder.DropTable(
                name: "Recepcionistas");

            migrationBuilder.DropTable(
                name: "PlanTratamientos");

            migrationBuilder.DropTable(
                name: "Tratamientos");

            migrationBuilder.DropTable(
                name: "Odontologos");

            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
