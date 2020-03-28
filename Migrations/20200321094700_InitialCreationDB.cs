using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UploadFiles.Migrations
{
    public partial class InitialCreationDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "varchar(150)", nullable: true),
                    Guid = table.Column<string>(type: "varchar(40)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCustomer = table.Column<int>(nullable: false),
                    COD = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    JobName = table.Column<string>(type: "varchar(150)", nullable: true),
                    DateExpiration = table.Column<DateTime>(type: "Date", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    IsUnique = table.Column<bool>(nullable: false),
                    GUID = table.Column<string>(type: "varchar(40)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Customers_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobsFileUpload",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdJobs = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobsFileUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobsFileUpload_Jobs_IdJobs",
                        column: x => x.IdJobs,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IdCustomer",
                table: "Jobs",
                column: "IdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_JobsFileUpload_IdJobs",
                table: "JobsFileUpload",
                column: "IdJobs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobsFileUpload");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
