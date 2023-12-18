using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Paws.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkTimer1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkTimer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkDayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartWorkDayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndWorkDayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BreakTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EmployeesId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTimer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTimer_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimer_EmployeesId",
                table: "WorkTimer",
                column: "EmployeesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkTimer");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Employees");
        }
    }
}
