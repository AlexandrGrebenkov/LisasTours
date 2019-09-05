using Microsoft.EntityFrameworkCore.Migrations;

namespace LisasTours.Data.Migrations
{
    public partial class contacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Contacts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CompanyId",
                table: "Contacts",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Company_CompanyId",
                table: "Contacts",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Company_CompanyId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_CompanyId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Contacts");
        }
    }
}
