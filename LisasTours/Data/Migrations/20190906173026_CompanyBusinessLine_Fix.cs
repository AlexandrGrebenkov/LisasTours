using Microsoft.EntityFrameworkCore.Migrations;

namespace LisasTours.Data.Migrations
{
    public partial class CompanyBusinessLine_Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyBusinessLines_BusinessLines_BusinessLineId",
                table: "CompanyBusinessLines");

            migrationBuilder.DropColumn(
                name: "BusenissLineId",
                table: "CompanyBusinessLines");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessLineId",
                table: "CompanyBusinessLines",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyBusinessLines_BusinessLines_BusinessLineId",
                table: "CompanyBusinessLines",
                column: "BusinessLineId",
                principalTable: "BusinessLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyBusinessLines_BusinessLines_BusinessLineId",
                table: "CompanyBusinessLines");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessLineId",
                table: "CompanyBusinessLines",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "BusenissLineId",
                table: "CompanyBusinessLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyBusinessLines_BusinessLines_BusinessLineId",
                table: "CompanyBusinessLines",
                column: "BusinessLineId",
                principalTable: "BusinessLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
