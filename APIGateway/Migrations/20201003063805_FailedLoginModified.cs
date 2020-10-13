using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGateway.Migrations
{
    public partial class FailedLoginModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "RetryTimeInMinutes",
                table: "ScrewIdentifierGrid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "InActiveSessionTimeout",
                table: "ScrewIdentifierGrid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfFailedAttemptsBeforeSecurityQuestions",
                table: "ScrewIdentifierGrid",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LogingFailedChecker",
                columns: table => new
                {
                    Userid = table.Column<string>(nullable: false),
                    Counter = table.Column<int>(nullable: false),
                    RetryTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogingFailedChecker", x => x.Userid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogingFailedChecker");

            migrationBuilder.DropColumn(
                name: "NumberOfFailedAttemptsBeforeSecurityQuestions",
                table: "ScrewIdentifierGrid");

            migrationBuilder.AlterColumn<int>(
                name: "RetryTimeInMinutes",
                table: "ScrewIdentifierGrid",
                type: "int",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<int>(
                name: "InActiveSessionTimeout",
                table: "ScrewIdentifierGrid",
                type: "int",
                nullable: false,
                oldClrType: typeof(TimeSpan));
        }
    }
}
