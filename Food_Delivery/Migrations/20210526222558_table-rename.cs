using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Food_Delivery.Migrations
{
    public partial class tablerename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Resturants_ResturantId",
                table: "Foods");

            migrationBuilder.DropTable(
                name: "Resturants");

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Restaurants_ResturantId",
                table: "Foods",
                column: "ResturantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Restaurants_ResturantId",
                table: "Foods");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.CreateTable(
                name: "Resturants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resturants", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Resturants_ResturantId",
                table: "Foods",
                column: "ResturantId",
                principalTable: "Resturants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
