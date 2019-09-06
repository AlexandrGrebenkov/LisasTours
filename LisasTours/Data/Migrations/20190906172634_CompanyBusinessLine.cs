using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LisasTours.Data.Migrations
{
    public partial class CompanyBusinessLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_BusinessLines_BusinessLineId",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_BusinessLineId",
                table: "Company");

            migrationBuilder.CreateTable(
                name: "CompanyBusinessLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(nullable: false),
                    BusenissLineId = table.Column<int>(nullable: false),
                    BusinessLineId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBusinessLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyBusinessLines_BusinessLines_BusinessLineId",
                        column: x => x.BusinessLineId,
                        principalTable: "BusinessLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyBusinessLines_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBusinessLines_BusinessLineId",
                table: "CompanyBusinessLines",
                column: "BusinessLineId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBusinessLines_CompanyId",
                table: "CompanyBusinessLines",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyBusinessLines");

            migrationBuilder.CreateIndex(
                name: "IX_Company_BusinessLineId",
                table: "Company",
                column: "BusinessLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_BusinessLines_BusinessLineId",
                table: "Company",
                column: "BusinessLineId",
                principalTable: "BusinessLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
