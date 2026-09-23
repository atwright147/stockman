using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockMan.Migrations
{
    /// <inheritdoc />
    public partial class AddStockTransactionAndUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockLevels_Locations_LocationId",
                table: "StockLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_StockLevels_Products_ProductId",
                table: "StockLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockLevels",
                table: "StockLevels");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StockLevels");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StockLevels");

            migrationBuilder.RenameTable(
                name: "StockLevels",
                newName: "StockLevel");

            migrationBuilder.RenameIndex(
                name: "IX_StockLevels_ProductId_LocationId",
                table: "StockLevel",
                newName: "IX_StockLevel_ProductId_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_StockLevels_LocationId",
                table: "StockLevel",
                newName: "IX_StockLevel_LocationId");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "StockLevel",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockLevel",
                table: "StockLevel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StockTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    FromLocationId = table.Column<int>(type: "INTEGER", nullable: true),
                    ToLocationId = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransaction_Locations_FromLocationId",
                        column: x => x.FromLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransaction_Locations_ToLocationId",
                        column: x => x.ToLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransaction_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Barcode",
                table: "Products",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Sku",
                table: "Products",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Code",
                table: "Locations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockTransaction_FromLocationId",
                table: "StockTransaction",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransaction_ProductId",
                table: "StockTransaction",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransaction_ToLocationId",
                table: "StockTransaction",
                column: "ToLocationId");

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

            migrationBuilder.DropTable(
                name: "StockTransaction");

            migrationBuilder.DropIndex(
                name: "IX_Products_Barcode",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Sku",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Code",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockLevel",
                table: "StockLevel");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "StockLevel");

            migrationBuilder.RenameTable(
                name: "StockLevel",
                newName: "StockLevels");

            migrationBuilder.RenameIndex(
                name: "IX_StockLevel_ProductId_LocationId",
                table: "StockLevels",
                newName: "IX_StockLevels_ProductId_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_StockLevel_LocationId",
                table: "StockLevels",
                newName: "IX_StockLevels_LocationId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StockLevels",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StockLevels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockLevels",
                table: "StockLevels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockLevels_Locations_LocationId",
                table: "StockLevels",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockLevels_Products_ProductId",
                table: "StockLevels",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
