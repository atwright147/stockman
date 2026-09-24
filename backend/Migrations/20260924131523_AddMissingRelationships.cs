using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockMan.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockLevel_Locations_LocationId",
                table: "StockLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_StockLevel_Products_ProductId",
                table: "StockLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransaction_Products_ProductId",
                table: "StockTransaction");

            migrationBuilder.AddForeignKey(
                name: "FK_StockLevel_Locations_LocationId",
                table: "StockLevel",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockLevel_Products_ProductId",
                table: "StockLevel",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransaction_Products_ProductId",
                table: "StockTransaction",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockLevel_Locations_LocationId",
                table: "StockLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_StockLevel_Products_ProductId",
                table: "StockLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransaction_Products_ProductId",
                table: "StockTransaction");

            migrationBuilder.AddForeignKey(
                name: "FK_StockLevel_Locations_LocationId",
                table: "StockLevel",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockLevel_Products_ProductId",
                table: "StockLevel",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransaction_Products_ProductId",
                table: "StockTransaction",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
