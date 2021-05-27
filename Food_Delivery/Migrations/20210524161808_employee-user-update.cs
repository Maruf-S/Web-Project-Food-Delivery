using Microsoft.EntityFrameworkCore.Migrations;

namespace Food_Delivery.Migrations
{
    public partial class employeeuserupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CVdocumentPath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVdocumentPath",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "AspNetUsers");
        }
    }
}
