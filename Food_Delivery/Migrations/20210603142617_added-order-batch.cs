using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Food_Delivery.Migrations
{
    public partial class addedorderbatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_EmployeeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_EmployeeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DatePlaced",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Delivered",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryLoc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNote",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryLoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatePlaced = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Delivered = table.Column<bool>(type: "bit", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderBatches_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderBatches_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BatchId",
                table: "Orders",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBatches_CustomerId",
                table: "OrderBatches",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBatches_EmployeeId",
                table: "OrderBatches",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderBatches_BatchId",
                table: "Orders",
                column: "BatchId",
                principalTable: "OrderBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderBatches_BatchId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderBatches");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BatchId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePlaced",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Delivered",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryLoc",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderNote",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeId",
                table: "Orders",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_EmployeeId",
                table: "Orders",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
