using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGateway.Migrations
{
    public partial class staffdeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCreditAdmin",
                table: "cor_staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDepositAdmin",
                table: "cor_staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExpenseManagementAdmin",
                table: "cor_staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinanceAdmin",
                table: "cor_staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHRAdmin",
                table: "cor_staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInvestorFundAdmin",
                table: "cor_staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPandPAdmin",
                table: "cor_staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTreasuryAdmin",
                table: "cor_staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PPEAdmin",
                table: "cor_staff",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCreditAdmin",
                table: "cor_staff");

            migrationBuilder.DropColumn(
                name: "IsDepositAdmin",
                table: "cor_staff");

            migrationBuilder.DropColumn(
                name: "IsExpenseManagementAdmin",
                table: "cor_staff");

            migrationBuilder.DropColumn(
                name: "IsFinanceAdmin",
                table: "cor_staff");

            migrationBuilder.DropColumn(
                name: "IsHRAdmin",
                table: "cor_staff");

            migrationBuilder.DropColumn(
                name: "IsInvestorFundAdmin",
                table: "cor_staff");

            migrationBuilder.DropColumn(
                name: "IsPandPAdmin",
                table: "cor_staff");

            migrationBuilder.DropColumn(
                name: "IsTreasuryAdmin",
                table: "cor_staff");

            migrationBuilder.DropColumn(
                name: "PPEAdmin",
                table: "cor_staff");
        }
    }
}
