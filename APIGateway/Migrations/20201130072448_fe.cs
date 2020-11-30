using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGateway.Migrations
{
    public partial class fe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hrm_setup_academic_discipline",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Discipline = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Rank = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_academic_discipline", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_academic_grade",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Grade = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Rank = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_academic_grade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_academic_qualification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Qualification = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Rank = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_academic_qualification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_employmentlevel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Employment_level = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_employmentlevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_employmenttype",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Employment_type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_employmenttype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_gym_workouts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Gym = table.Column<string>(nullable: true),
                    Contact_phone_number = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Ratings = table.Column<string>(nullable: true),
                    Other_comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_gym_workouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_high_school_grades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Grade = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Rank = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_high_school_grades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_high_school_subjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_high_school_subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_hmo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Hmo_name = table.Column<string>(nullable: true),
                    Hmo_code = table.Column<string>(nullable: true),
                    Contact_phone_number = table.Column<string>(nullable: true),
                    Contact_email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Reg_date = table.Column<DateTime>(nullable: false),
                    Rating = table.Column<string>(nullable: true),
                    Order_comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_hmo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_jobdetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Job_title = table.Column<int>(nullable: false),
                    Job_description = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_jobdetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_jobgrade",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Job_grade = table.Column<string>(nullable: true),
                    Job_grade_reporting_to = table.Column<int>(nullable: false),
                    Rank = table.Column<string>(nullable: true),
                    Probation_period_in_months = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_jobgrade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_languages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_proffesional_membership",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Professional_membership = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_proffesional_membership", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_proffessional_certification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Certification = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Rank = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_proffessional_certification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hrm_setup_sub_skill",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Job_details_Id = table.Column<int>(nullable: false),
                    Skill = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Weight = table.Column<string>(nullable: true),
                    hrm_setup_jobdetailsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrm_setup_sub_skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_hrm_setup_sub_skill_hrm_setup_jobdetails_hrm_setup_jobdetailsId",
                        column: x => x.hrm_setup_jobdetailsId,
                        principalTable: "hrm_setup_jobdetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_hrm_setup_sub_skill_hrm_setup_jobdetailsId",
                table: "hrm_setup_sub_skill",
                column: "hrm_setup_jobdetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hrm_setup_academic_discipline");

            migrationBuilder.DropTable(
                name: "hrm_setup_academic_grade");

            migrationBuilder.DropTable(
                name: "hrm_setup_academic_qualification");

            migrationBuilder.DropTable(
                name: "hrm_setup_employmentlevel");

            migrationBuilder.DropTable(
                name: "hrm_setup_employmenttype");

            migrationBuilder.DropTable(
                name: "hrm_setup_gym_workouts");

            migrationBuilder.DropTable(
                name: "hrm_setup_high_school_grades");

            migrationBuilder.DropTable(
                name: "hrm_setup_high_school_subjects");

            migrationBuilder.DropTable(
                name: "hrm_setup_hmo");

            migrationBuilder.DropTable(
                name: "hrm_setup_jobgrade");

            migrationBuilder.DropTable(
                name: "hrm_setup_languages");

            migrationBuilder.DropTable(
                name: "hrm_setup_proffesional_membership");

            migrationBuilder.DropTable(
                name: "hrm_setup_proffessional_certification");

            migrationBuilder.DropTable(
                name: "hrm_setup_sub_skill");

            migrationBuilder.DropTable(name: "hrm_setup_jobdetails");
        }
    }
}
