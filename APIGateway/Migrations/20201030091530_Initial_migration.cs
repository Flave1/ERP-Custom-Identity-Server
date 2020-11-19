using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGateway.Migrations
{
    public partial class Initial_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    StaffId = table.Column<int>(nullable: false),
                    IsFirstLoginAttempt = table.Column<bool>(nullable: false),
                    NextPasswordChangeDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    SecurityAnswer = table.Column<string>(maxLength: 256, nullable: true),
                    QuestionId = table.Column<int>(nullable: false),
                    IsQuestionTime = table.Column<bool>(nullable: false),
                    EnableAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cor_activityparent",
                columns: table => new
                {
                    ActivityParentId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    ActivityParentName = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_activityparent", x => x.ActivityParentId);
                });

            migrationBuilder.CreateTable(
                name: "cor_companystructure",
                columns: table => new
                {
                    CompanyStructureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    StructureTypeId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    HeadStaffId = table.Column<int>(nullable: true),
                    ParentCompanyID = table.Column<int>(nullable: true),
                    Parent = table.Column<string>(maxLength: 250, nullable: true),
                    Address1 = table.Column<string>(maxLength: 500, nullable: true),
                    Address2 = table.Column<string>(maxLength: 500, nullable: true),
                    Telephone = table.Column<string>(maxLength: 250, nullable: true),
                    Fax = table.Column<string>(maxLength: 250, nullable: true),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    RegistrationNumber = table.Column<string>(maxLength: 50, nullable: true),
                    TaxId = table.Column<string>(maxLength: 50, nullable: true),
                    NoOfEmployees = table.Column<int>(nullable: true),
                    WebSite = table.Column<string>(maxLength: 250, nullable: true),
                    Logo = table.Column<byte[]>(nullable: true),
                    LogoType = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<int>(nullable: true),
                    State = table.Column<int>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: true),
                    ReportCurrencyId = table.Column<int>(nullable: true),
                    ApplyRegistryTemplate = table.Column<string>(maxLength: 10, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 50, nullable: true),
                    IsMultiCompany = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Subsidairy_Level = table.Column<int>(nullable: true),
                    RegistryTemplate = table.Column<string>(maxLength: 50, nullable: true),
                    FSTemplateName = table.Column<string>(nullable: true),
                    FSTemplate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_companystructure", x => x.CompanyStructureId);
                });

            migrationBuilder.CreateTable(
                name: "cor_companystructuredefinition",
                columns: table => new
                {
                    StructureDefinitionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Definition = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 150, nullable: true),
                    StructureLevel = table.Column<int>(nullable: false),
                    IsMultiCompany = table.Column<bool>(nullable: false),
                    OperatingLevel = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_companystructuredefinition", x => x.StructureDefinitionId);
                });

            migrationBuilder.CreateTable(
                name: "cor_country",
                columns: table => new
                {
                    CountryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    CountryCode = table.Column<string>(maxLength: 10, nullable: false),
                    CountryName = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "cor_currency",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    CurrencyCode = table.Column<string>(maxLength: 10, nullable: false),
                    CurrencyName = table.Column<string>(maxLength: 250, nullable: false),
                    BaseCurrency = table.Column<bool>(nullable: true),
                    INUSE = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_currency", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "cor_emailconfig",
                columns: table => new
                {
                    EmailConfigId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    SmtpClient = table.Column<string>(nullable: true),
                    SenderEmail = table.Column<string>(nullable: true),
                    SenderUserName = table.Column<string>(nullable: true),
                    BaseFrontend = table.Column<string>(nullable: true),
                    EnableSSL = table.Column<bool>(nullable: false),
                    SMTPPort = table.Column<int>(nullable: false),
                    MailCaption = table.Column<string>(nullable: true),
                    SenderPassword = table.Column<string>(nullable: true),
                    SendNotification = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_emailconfig", x => x.EmailConfigId);
                });

            migrationBuilder.CreateTable(
                name: "cor_employertype",
                columns: table => new
                {
                    EmployerTypeId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_employertype", x => x.EmployerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "cor_gender",
                columns: table => new
                {
                    GenderId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Gender = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_gender", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "cor_identification",
                columns: table => new
                {
                    IdentificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    IdentificationName = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_identification", x => x.IdentificationId);
                });

            migrationBuilder.CreateTable(
                name: "cor_jobtitles",
                columns: table => new
                {
                    JobTitleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    JobDescription = table.Column<string>(maxLength: 2000, nullable: true),
                    Skills = table.Column<string>(maxLength: 1000, nullable: true),
                    SkillDescription = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_jobtitles", x => x.JobTitleId);
                });

            migrationBuilder.CreateTable(
                name: "cor_maritalstatus",
                columns: table => new
                {
                    MaritalStatusId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_maritalstatus", x => x.MaritalStatusId);
                });

            migrationBuilder.CreateTable(
                name: "cor_operationtype",
                columns: table => new
                {
                    OperationTypeId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    ModuleId = table.Column<int>(nullable: false),
                    OperationTypeName = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_operationtype", x => x.OperationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "cor_title",
                columns: table => new
                {
                    TitleId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_title", x => x.TitleId);
                });

            migrationBuilder.CreateTable(
                name: "cor_useraccess",
                columns: table => new
                {
                    UserAccessLevelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    AccessLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_useraccess", x => x.UserAccessLevelId);
                });

            migrationBuilder.CreateTable(
                name: "Cor_Useremailconfirmations",
                columns: table => new
                {
                    cor_useremailconfirmationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    ConfirnamationTokenCode = table.Column<string>(nullable: true),
                    IssuedDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cor_Useremailconfirmations", x => x.cor_useremailconfirmationId);
                });

            migrationBuilder.CreateTable(
                name: "cor_workflowgroup",
                columns: table => new
                {
                    WorkflowGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    WorkflowGroupName = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_workflowgroup", x => x.WorkflowGroupId);
                });

            migrationBuilder.CreateTable(
                name: "cor_workflowtask",
                columns: table => new
                {
                    WorkflowTaskId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    StaffId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    OperationId = table.Column<int>(nullable: false),
                    DefferedExecution = table.Column<bool>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: false),
                    StaffEmail = table.Column<string>(nullable: true),
                    StaffAccessId = table.Column<int>(nullable: false),
                    StaffRoles = table.Column<string>(nullable: true),
                    WorkflowTaskStatus = table.Column<int>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    IsMailSent = table.Column<bool>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    WorkflowToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_workflowtask", x => x.WorkflowTaskId);
                });

            migrationBuilder.CreateTable(
                name: "credit_documenttype",
                columns: table => new
                {
                    DocumentTypeId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_documenttype", x => x.DocumentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EmailAddress",
                columns: table => new
                {
                    EmailAddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    EmailMessageId = table.Column<int>(nullable: false),
                    Action = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ReceiverUserId = table.Column<string>(nullable: true),
                    SingleUserStatus = table.Column<int>(nullable: false),
                    Attachment = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAddress", x => x.EmailAddressId);
                });

            migrationBuilder.CreateTable(
                name: "EmailMessage",
                columns: table => new
                {
                    EmailMessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    EmailStatus = table.Column<int>(nullable: false),
                    SentBy = table.Column<string>(nullable: true),
                    ReceivedBy = table.Column<string>(nullable: true),
                    ReceiverUserId = table.Column<string>(nullable: true),
                    Module = table.Column<int>(nullable: false),
                    DateSent = table.Column<DateTime>(nullable: false),
                    SendIt = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessage", x => x.EmailMessageId);
                });

            migrationBuilder.CreateTable(
                name: "LogingFailedChecker",
                columns: table => new
                {
                    Userid = table.Column<string>(nullable: false),
                    Counter = table.Column<int>(nullable: false),
                    QuestionTimeCount = table.Column<int>(nullable: false),
                    RetryTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogingFailedChecker", x => x.Userid);
                });

            migrationBuilder.CreateTable(
                name: "OTPTracker",
                columns: table => new
                {
                    OTPId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    OTP = table.Column<string>(nullable: true),
                    DateIssued = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPTracker", x => x.OTPId);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Token = table.Column<string>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    JwtId = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    Used = table.Column<bool>(nullable: false),
                    Invalidated = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "ScrewIdentifierGrid",
                columns: table => new
                {
                    ScrewIdentifierGridId = table.Column<int>(nullable: false),
                    ShouldAthenticate = table.Column<bool>(nullable: false),
                    Media = table.Column<int>(nullable: false),
                    Module = table.Column<int>(nullable: false),
                    ActiveOnMobileApp = table.Column<bool>(nullable: false),
                    ActiveOnWebApp = table.Column<bool>(nullable: false),
                    UseActiveDirectory = table.Column<bool>(nullable: false),
                    ActiveDirectory = table.Column<string>(nullable: true),
                    EnableLoginFailedLockout = table.Column<bool>(nullable: false),
                    NumberOfFailedLoginBeforeLockout = table.Column<int>(nullable: false),
                    NumberOfFailedAttemptsBeforeSecurityQuestions = table.Column<int>(nullable: false),
                    RetryTimeInMinutes = table.Column<TimeSpan>(nullable: false),
                    EnableRetryOnMobileApp = table.Column<bool>(nullable: false),
                    EnableRetryOnWebApp = table.Column<bool>(nullable: false),
                    ShouldRetryAfterLockoutEnabled = table.Column<bool>(nullable: false),
                    InActiveSessionTimeout = table.Column<TimeSpan>(nullable: false),
                    PasswordUpdateCycle = table.Column<int>(nullable: false),
                    SecuritySettingActiveOnMobileApp = table.Column<bool>(nullable: false),
                    SecuritySettingsActiveOnWebApp = table.Column<bool>(nullable: false),
                    EnableLoadBalance = table.Column<bool>(nullable: false),
                    LoadBalanceInHours = table.Column<int>(nullable: false),
                    WhenNextToUpdatePassword = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrewIdentifierGrid", x => x.ScrewIdentifierGridId);
                });

            migrationBuilder.CreateTable(
                name: "SessionChecker",
                columns: table => new
                {
                    Userid = table.Column<string>(nullable: false),
                    Module = table.Column<int>(nullable: false),
                    LastRefreshed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionChecker", x => x.Userid);
                });

            migrationBuilder.CreateTable(
                name: "SolutionModule",
                columns: table => new
                {
                    SolutionModuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    SolutionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolutionModule", x => x.SolutionModuleId);
                });

            migrationBuilder.CreateTable(
                name: "Tracker",
                columns: table => new
                {
                    MeasureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Browser = table.Column<string>(nullable: true),
                    Os = table.Column<string>(nullable: true),
                    Device = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    Os_Device = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracker", x => x.MeasureId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cor_activity",
                columns: table => new
                {
                    ActivityId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    ActivityParentId = table.Column<int>(nullable: false),
                    ActivityName = table.Column<string>(maxLength: 256, nullable: false),
                    cor_activityparentActivityParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_activity", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_cor_activity_cor_activityparent_cor_activityparentActivityParentId",
                        column: x => x.cor_activityparentActivityParentId,
                        principalTable: "cor_activityparent",
                        principalColumn: "ActivityParentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_publicholiday",
                columns: table => new
                {
                    PublicHolidayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    cor_countryCountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_publicholiday", x => x.PublicHolidayId);
                    table.ForeignKey(
                        name: "FK_cor_publicholiday_cor_country_cor_countryCountryId",
                        column: x => x.cor_countryCountryId,
                        principalTable: "cor_country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_state",
                columns: table => new
                {
                    StateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    StateCode = table.Column<string>(maxLength: 10, nullable: false),
                    StateName = table.Column<string>(maxLength: 250, nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    cor_countryCountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_state", x => x.StateId);
                    table.ForeignKey(
                        name: "FK_cor_state_cor_country_cor_countryCountryId",
                        column: x => x.cor_countryCountryId,
                        principalTable: "cor_country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Address1 = table.Column<string>(maxLength: 500, nullable: true),
                    Address2 = table.Column<string>(maxLength: 500, nullable: true),
                    Telephone = table.Column<string>(maxLength: 250, nullable: true),
                    Fax = table.Column<string>(maxLength: 250, nullable: true),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    RegistrationNumber = table.Column<string>(maxLength: 50, nullable: true),
                    TaxId = table.Column<string>(maxLength: 50, nullable: true),
                    NoOfEmployees = table.Column<int>(nullable: true),
                    WebSite = table.Column<string>(maxLength: 250, nullable: true),
                    Logo = table.Column<byte[]>(type: "image", nullable: true),
                    LogoType = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    State = table.Column<string>(maxLength: 50, nullable: true),
                    CountryId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    ApplyRegistryTemplate = table.Column<string>(maxLength: 10, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 50, nullable: true),
                    IsMultiCompany = table.Column<bool>(nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Subsidairy_Level = table.Column<int>(nullable: true),
                    RegistryTemplate = table.Column<string>(maxLength: 50, nullable: true),
                    cor_countryCountryId = table.Column<int>(nullable: true),
                    cor_currencyCurrencyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_company", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_cor_company_cor_country_cor_countryCountryId",
                        column: x => x.cor_countryCountryId,
                        principalTable: "cor_country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cor_company_cor_currency_cor_currencyCurrencyId",
                        column: x => x.cor_currencyCurrencyId,
                        principalTable: "cor_currency",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_currencyrate",
                columns: table => new
                {
                    CurrencyRateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    BuyingRate = table.Column<double>(nullable: true),
                    SellingRate = table.Column<double>(nullable: true),
                    cor_currencyCurrencyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_currencyrate", x => x.CurrencyRateId);
                    table.ForeignKey(
                        name: "FK_cor_currencyrate_cor_currency_cor_currencyCurrencyId",
                        column: x => x.cor_currencyCurrencyId,
                        principalTable: "cor_currency",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_operation",
                columns: table => new
                {
                    OperationId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    OperationName = table.Column<string>(maxLength: 250, nullable: false),
                    OperationTypeId = table.Column<int>(nullable: false),
                    EnableWorkflow = table.Column<bool>(nullable: true),
                    cor_operationtypeOperationTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_operation", x => x.OperationId);
                    table.ForeignKey(
                        name: "FK_cor_operation_cor_operationtype_cor_operationtypeOperationTypeId",
                        column: x => x.cor_operationtypeOperationTypeId,
                        principalTable: "cor_operationtype",
                        principalColumn: "OperationTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_workflowlevel",
                columns: table => new
                {
                    WorkflowLevelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    WorkflowLevelName = table.Column<string>(maxLength: 250, nullable: false),
                    WorkflowGroupId = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    RequiredLimit = table.Column<bool>(nullable: false),
                    LimitAmount = table.Column<decimal>(nullable: true),
                    CanModify = table.Column<bool>(nullable: true),
                    cor_workflowgroupWorkflowGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_workflowlevel", x => x.WorkflowLevelId);
                    table.ForeignKey(
                        name: "FK_cor_workflowlevel_cor_workflowgroup_cor_workflowgroupWorkflowGroupId",
                        column: x => x.cor_workflowgroupWorkflowGroupId,
                        principalTable: "cor_workflowgroup",
                        principalColumn: "WorkflowGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_userroleactivity",
                columns: table => new
                {
                    UserRoleActivityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    ActivityId = table.Column<int>(nullable: false),
                    CanEdit = table.Column<bool>(nullable: true),
                    CanAdd = table.Column<bool>(nullable: true),
                    CanView = table.Column<bool>(nullable: true),
                    CanDelete = table.Column<bool>(nullable: true),
                    CanApprove = table.Column<bool>(nullable: true),
                    cor_activityActivityId = table.Column<int>(nullable: true),
                    cor_userroleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_userroleactivity", x => x.UserRoleActivityId);
                    table.ForeignKey(
                        name: "FK_cor_userroleactivity_cor_activity_cor_activityActivityId",
                        column: x => x.cor_activityActivityId,
                        principalTable: "cor_activity",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cor_userroleactivity_AspNetRoles_cor_userroleId",
                        column: x => x.cor_userroleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_userroleadditionalactivity",
                columns: table => new
                {
                    UserRoleAdditionalActivityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    ActivityId = table.Column<int>(nullable: false),
                    CanEdit = table.Column<bool>(nullable: true),
                    CanAdd = table.Column<bool>(nullable: true),
                    CanView = table.Column<bool>(nullable: true),
                    CanDelete = table.Column<bool>(nullable: true),
                    CanApprove = table.Column<bool>(nullable: true),
                    cor_activityActivityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_userroleadditionalactivity", x => x.UserRoleAdditionalActivityId);
                    table.ForeignKey(
                        name: "FK_cor_userroleadditionalactivity_cor_activity_cor_activityActivityId",
                        column: x => x.cor_activityActivityId,
                        principalTable: "cor_activity",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_city",
                columns: table => new
                {
                    CityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    CityCode = table.Column<string>(maxLength: 10, nullable: false),
                    CityName = table.Column<string>(maxLength: 250, nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    cor_stateStateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_city", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_cor_city_cor_state_cor_stateStateId",
                        column: x => x.cor_stateStateId,
                        principalTable: "cor_state",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_staff",
                columns: table => new
                {
                    StaffId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    StaffCode = table.Column<string>(maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    JobTitle = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Address = table.Column<string>(maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<string>(maxLength: 10, nullable: true),
                    StateId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    Photo = table.Column<byte[]>(nullable: true),
                    StaffLimit = table.Column<decimal>(nullable: true),
                    AccessLevel = table.Column<int>(nullable: true),
                    StaffOfficeId = table.Column<int>(nullable: true),
                    cor_stateStateId = table.Column<int>(nullable: true),
                    cor_countryCountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_staff", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_cor_staff_cor_country_cor_countryCountryId",
                        column: x => x.cor_countryCountryId,
                        principalTable: "cor_country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cor_staff_cor_state_cor_stateStateId",
                        column: x => x.cor_stateStateId,
                        principalTable: "cor_state",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_branch",
                columns: table => new
                {
                    BranchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    BranchCode = table.Column<string>(maxLength: 10, nullable: false),
                    BranchName = table.Column<string>(maxLength: 250, nullable: false),
                    Address = table.Column<string>(maxLength: 250, nullable: false),
                    cor_companyCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_branch", x => x.BranchId);
                    table.ForeignKey(
                        name: "FK_cor_branch_cor_company_cor_companyCompanyId",
                        column: x => x.cor_companyCompanyId,
                        principalTable: "cor_company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_workflow",
                columns: table => new
                {
                    WorkflowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    WorkflowName = table.Column<string>(maxLength: 250, nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    WorkflowAccessId = table.Column<int>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    cor_operationOperationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_workflow", x => x.WorkflowId);
                    table.ForeignKey(
                        name: "FK_cor_workflow_cor_operation_cor_operationOperationId",
                        column: x => x.cor_operationOperationId,
                        principalTable: "cor_operation",
                        principalColumn: "OperationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_workflowtrail",
                columns: table => new
                {
                    WorkflowTrailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    TargetId = table.Column<int>(nullable: false),
                    cor_staffId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: false),
                    FromWorkflowLevelId = table.Column<int>(nullable: true),
                    ToWorkflowLevelId = table.Column<int>(nullable: true),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    RequestStaffId = table.Column<int>(nullable: false),
                    ResponseStaffId = table.Column<int>(nullable: true),
                    ArrivalDate = table.Column<DateTime>(nullable: false),
                    ActualArrivalDate = table.Column<DateTime>(nullable: true),
                    ResponseDate = table.Column<DateTime>(nullable: true),
                    cor_workflowlevelWorkflowLevelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_workflowtrail", x => x.WorkflowTrailId);
                    table.ForeignKey(
                        name: "FK_cor_workflowtrail_cor_workflowlevel_cor_workflowlevelWorkflowLevelId",
                        column: x => x.cor_workflowlevelWorkflowLevelId,
                        principalTable: "cor_workflowlevel",
                        principalColumn: "WorkflowLevelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_workflowlevelstaff",
                columns: table => new
                {
                    WorkflowLevelStaffId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    StaffId = table.Column<int>(nullable: false),
                    WorkflowGroupId = table.Column<int>(nullable: false),
                    WorkflowLevelId = table.Column<int>(nullable: false),
                    cor_staffStaffId = table.Column<int>(nullable: true),
                    cor_workflowgroupWorkflowGroupId = table.Column<int>(nullable: true),
                    cor_workflowlevelWorkflowLevelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_workflowlevelstaff", x => x.WorkflowLevelStaffId);
                    table.ForeignKey(
                        name: "FK_cor_workflowlevelstaff_cor_staff_cor_staffStaffId",
                        column: x => x.cor_staffStaffId,
                        principalTable: "cor_staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cor_workflowlevelstaff_cor_workflowgroup_cor_workflowgroupWorkflowGroupId",
                        column: x => x.cor_workflowgroupWorkflowGroupId,
                        principalTable: "cor_workflowgroup",
                        principalColumn: "WorkflowGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cor_workflowlevelstaff_cor_workflowlevel_cor_workflowlevelWorkflowLevelId",
                        column: x => x.cor_workflowlevelWorkflowLevelId,
                        principalTable: "cor_workflowlevel",
                        principalColumn: "WorkflowLevelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    DepartmentCode = table.Column<string>(maxLength: 10, nullable: false),
                    DepartmentName = table.Column<string>(maxLength: 250, nullable: false),
                    BranchId = table.Column<int>(nullable: false),
                    cor_branchBranchId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_department", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_cor_department_cor_branch_cor_branchBranchId",
                        column: x => x.cor_branchBranchId,
                        principalTable: "cor_branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_workflowaccess",
                columns: table => new
                {
                    WorkflowCompanyAccessId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    WorkflowId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    OfficeAccessId = table.Column<int>(nullable: false),
                    cor_operationOperationId = table.Column<int>(nullable: true),
                    cor_workflowWorkflowId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_workflowaccess", x => x.WorkflowCompanyAccessId);
                    table.ForeignKey(
                        name: "FK_cor_workflowaccess_cor_operation_cor_operationOperationId",
                        column: x => x.cor_operationOperationId,
                        principalTable: "cor_operation",
                        principalColumn: "OperationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cor_workflowaccess_cor_workflow_cor_workflowWorkflowId",
                        column: x => x.cor_workflowWorkflowId,
                        principalTable: "cor_workflow",
                        principalColumn: "WorkflowId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_workflowdetails",
                columns: table => new
                {
                    WorkflowDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    WorkflowId = table.Column<int>(nullable: false),
                    WorkflowGroupId = table.Column<int>(nullable: false),
                    WorkflowLevelId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    AccessId = table.Column<int>(nullable: false),
                    OfficeAccessIds = table.Column<string>(maxLength: 50, nullable: true),
                    Position = table.Column<int>(nullable: false),
                    cor_workflowWorkflowId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_workflowdetails", x => x.WorkflowDetailId);
                    table.ForeignKey(
                        name: "FK_cor_workflowdetails_cor_workflow_cor_workflowWorkflowId",
                        column: x => x.cor_workflowWorkflowId,
                        principalTable: "cor_workflow",
                        principalColumn: "WorkflowId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_workflowdetailsaccess",
                columns: table => new
                {
                    WorkflowDetailsAccessId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    WorkflowDetailId = table.Column<int>(nullable: false),
                    OfficeAccessId = table.Column<int>(nullable: false),
                    cor_workflowdetailsWorkflowDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_workflowdetailsaccess", x => x.WorkflowDetailsAccessId);
                    table.ForeignKey(
                        name: "FK_cor_workflowdetailsaccess_cor_workflowdetails_cor_workflowdetailsWorkflowDetailId",
                        column: x => x.cor_workflowdetailsWorkflowDetailId,
                        principalTable: "cor_workflowdetails",
                        principalColumn: "WorkflowDetailId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_cor_activity_cor_activityparentActivityParentId",
                table: "cor_activity",
                column: "cor_activityparentActivityParentId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_branch_cor_companyCompanyId",
                table: "cor_branch",
                column: "cor_companyCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_city_cor_stateStateId",
                table: "cor_city",
                column: "cor_stateStateId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_company_cor_countryCountryId",
                table: "cor_company",
                column: "cor_countryCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_company_cor_currencyCurrencyId",
                table: "cor_company",
                column: "cor_currencyCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_currencyrate_cor_currencyCurrencyId",
                table: "cor_currencyrate",
                column: "cor_currencyCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_department_cor_branchBranchId",
                table: "cor_department",
                column: "cor_branchBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_operation_cor_operationtypeOperationTypeId",
                table: "cor_operation",
                column: "cor_operationtypeOperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_publicholiday_cor_countryCountryId",
                table: "cor_publicholiday",
                column: "cor_countryCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_staff_cor_countryCountryId",
                table: "cor_staff",
                column: "cor_countryCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_staff_cor_stateStateId",
                table: "cor_staff",
                column: "cor_stateStateId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_state_cor_countryCountryId",
                table: "cor_state",
                column: "cor_countryCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_userroleactivity_cor_activityActivityId",
                table: "cor_userroleactivity",
                column: "cor_activityActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_userroleactivity_cor_userroleId",
                table: "cor_userroleactivity",
                column: "cor_userroleId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_userroleadditionalactivity_cor_activityActivityId",
                table: "cor_userroleadditionalactivity",
                column: "cor_activityActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_workflow_cor_operationOperationId",
                table: "cor_workflow",
                column: "cor_operationOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_workflowaccess_cor_operationOperationId",
                table: "cor_workflowaccess",
                column: "cor_operationOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_workflowaccess_cor_workflowWorkflowId",
                table: "cor_workflowaccess",
                column: "cor_workflowWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_workflowdetails_cor_workflowWorkflowId",
                table: "cor_workflowdetails",
                column: "cor_workflowWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_workflowdetailsaccess_cor_workflowdetailsWorkflowDetailId",
                table: "cor_workflowdetailsaccess",
                column: "cor_workflowdetailsWorkflowDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_workflowlevel_cor_workflowgroupWorkflowGroupId",
                table: "cor_workflowlevel",
                column: "cor_workflowgroupWorkflowGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_workflowlevelstaff_cor_staffStaffId",
                table: "cor_workflowlevelstaff",
                column: "cor_staffStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_workflowlevelstaff_cor_workflowgroupWorkflowGroupId",
                table: "cor_workflowlevelstaff",
                column: "cor_workflowgroupWorkflowGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_workflowlevelstaff_cor_workflowlevelWorkflowLevelId",
                table: "cor_workflowlevelstaff",
                column: "cor_workflowlevelWorkflowLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_workflowtrail_cor_workflowlevelWorkflowLevelId",
                table: "cor_workflowtrail",
                column: "cor_workflowlevelWorkflowLevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "cor_city");

            migrationBuilder.DropTable(
                name: "cor_companystructure");

            migrationBuilder.DropTable(
                name: "cor_companystructuredefinition");

            migrationBuilder.DropTable(
                name: "cor_currencyrate");

            migrationBuilder.DropTable(
                name: "cor_department");

            migrationBuilder.DropTable(
                name: "cor_emailconfig");

            migrationBuilder.DropTable(
                name: "cor_employertype");

            migrationBuilder.DropTable(
                name: "cor_gender");

            migrationBuilder.DropTable(
                name: "cor_identification");

            migrationBuilder.DropTable(
                name: "cor_jobtitles");

            migrationBuilder.DropTable(
                name: "cor_maritalstatus");

            migrationBuilder.DropTable(
                name: "cor_publicholiday");

            migrationBuilder.DropTable(
                name: "cor_title");

            migrationBuilder.DropTable(
                name: "cor_useraccess");

            migrationBuilder.DropTable(
                name: "Cor_Useremailconfirmations");

            migrationBuilder.DropTable(
                name: "cor_userroleactivity");

            migrationBuilder.DropTable(
                name: "cor_userroleadditionalactivity");

            migrationBuilder.DropTable(
                name: "cor_workflowaccess");

            migrationBuilder.DropTable(
                name: "cor_workflowdetailsaccess");

            migrationBuilder.DropTable(
                name: "cor_workflowlevelstaff");

            migrationBuilder.DropTable(
                name: "cor_workflowtask");

            migrationBuilder.DropTable(
                name: "cor_workflowtrail");

            migrationBuilder.DropTable(
                name: "credit_documenttype");

            migrationBuilder.DropTable(
                name: "EmailAddress");

            migrationBuilder.DropTable(
                name: "EmailMessage");

            migrationBuilder.DropTable(
                name: "LogingFailedChecker");

            migrationBuilder.DropTable(
                name: "OTPTracker");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "ScrewIdentifierGrid");

            migrationBuilder.DropTable(
                name: "SessionChecker");

            migrationBuilder.DropTable(
                name: "SolutionModule");

            migrationBuilder.DropTable(
                name: "Tracker");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "cor_branch");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "cor_activity");

            migrationBuilder.DropTable(
                name: "cor_workflowdetails");

            migrationBuilder.DropTable(
                name: "cor_staff");

            migrationBuilder.DropTable(
                name: "cor_workflowlevel");

            migrationBuilder.DropTable(
                name: "cor_company");

            migrationBuilder.DropTable(
                name: "cor_activityparent");

            migrationBuilder.DropTable(
                name: "cor_workflow");

            migrationBuilder.DropTable(
                name: "cor_state");

            migrationBuilder.DropTable(
                name: "cor_workflowgroup");

            migrationBuilder.DropTable(
                name: "cor_currency");

            migrationBuilder.DropTable(
                name: "cor_operation");

            migrationBuilder.DropTable(
                name: "cor_country");

            migrationBuilder.DropTable(
                name: "cor_operationtype");
        }
    }
}
