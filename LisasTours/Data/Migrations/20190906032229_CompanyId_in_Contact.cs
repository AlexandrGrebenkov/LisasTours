using Microsoft.EntityFrameworkCore.Migrations;

namespace LisasTours.Data.Migrations
{
    public partial class CompanyId_in_Contact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Company_CompanyId",
                table: "Contacts");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Contacts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Company_CompanyId",
                table: "Contacts",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Company_CompanyId",
                table: "Contacts");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Contacts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Company_CompanyId",
                table: "Contacts",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
