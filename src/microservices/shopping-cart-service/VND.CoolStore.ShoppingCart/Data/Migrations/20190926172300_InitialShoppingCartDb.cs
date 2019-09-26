using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VND.CoolStore.ShoppingCart.Data.Migrations
{
    public partial class InitialShoppingCartDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cart");

            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "Carts",
                schema: "cart",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    CartItemTotal = table.Column<double>(nullable: false),
                    CartItemPromoSavings = table.Column<double>(nullable: false),
                    ShippingTotal = table.Column<double>(nullable: false),
                    ShippingPromoSavings = table.Column<double>(nullable: false),
                    CartTotal = table.Column<double>(nullable: false),
                    IsCheckout = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCatalogs",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCatalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                schema: "cart",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    PromoSavings = table.Column<double>(nullable: false),
                    CurrentCartId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CurrentCartId",
                        column: x => x.CurrentCartId,
                        principalSchema: "cart",
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCatalogIds",
                schema: "cart",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    CurrentCartItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCatalogIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCatalogIds_CartItems_CurrentCartItemId",
                        column: x => x.CurrentCartItemId,
                        principalSchema: "cart",
                        principalTable: "CartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "ProductCatalogs",
                columns: new[] { "Id", "Created", "Desc", "ImagePath", "Name", "Price", "ProductId", "Updated", "Version" },
                values: new object[] { new Guid("df79f461-e985-4ebe-bf65-922bc85a6f8d"), new DateTime(2019, 9, 26, 17, 22, 59, 746, DateTimeKind.Utc).AddTicks(6695), "quis nostrud exercitation ull", "https://picsum.photos/1200/900?image=1", "tempor incididunt ut labore et do", 638.0, new Guid("05233341-185a-468a-b074-00ebd08559aa"), null, 0 });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "ProductCatalogs",
                columns: new[] { "Id", "Created", "Desc", "ImagePath", "Name", "Price", "ProductId", "Updated", "Version" },
                values: new object[] { new Guid("be559748-37b4-488f-bb31-deae8109895b"), new DateTime(2019, 9, 26, 17, 22, 59, 746, DateTimeKind.Utc).AddTicks(9546), "sin", "https://picsum.photos/1200/900?image=1", "m", 671.0, new Guid("3cb275c5-aa53-40ff-bc6a-015327053af9"), null, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CurrentCartId",
                schema: "cart",
                table: "CartItems",
                column: "CurrentCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCatalogIds_CurrentCartItemId",
                schema: "cart",
                table: "ProductCatalogIds",
                column: "CurrentCartItemId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCatalogIds",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "ProductCatalogs",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "CartItems",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "Carts",
                schema: "cart");
        }
    }
}
