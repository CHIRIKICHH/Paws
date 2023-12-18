using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paws.Migrations
{
    /// <inheritdoc />
    public partial class Timer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "OnlineTime",
                table: "WorkTimer",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnlineTime",
                table: "WorkTimer");
        }
    }
}
