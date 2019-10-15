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
                    UserId = table.Column<Guid>(nullable: false),
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
                    ImagePath = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InventoryId = table.Column<Guid>(nullable: false)
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
                columns: new[] { "Id", "Created", "Desc", "ImagePath", "InventoryId", "IsDeleted", "Name", "Price", "ProductId", "Updated", "Version" },
                values: new object[,]
                {
                    { new Guid("1c0fc113-0c71-4595-90a1-902acb26433b"), new DateTime(2019, 10, 15, 6, 49, 14, 598, DateTimeKind.Utc).AddTicks(4707), "quis nostrud exercitation ull", "https://picsum.photos/1200/900?image=1", new Guid("90c9479e-a11c-4d6d-aaaa-0405b6c0efcd"), false, "tempor incididunt ut labore et do", 638.0, new Guid("05233341-185a-468a-b074-00ebd08559aa"), null, 0 },
                    { new Guid("224f3f08-e581-4b7e-8a68-784183ecf0ef"), new DateTime(2019, 10, 15, 6, 49, 14, 598, DateTimeKind.Utc).AddTicks(8127), "sin", "https://picsum.photos/1200/900?image=1", new Guid("90c9479e-a11c-4d6d-aaaa-0405b6c0efcd"), false, "m", 671.0, new Guid("3cb275c5-aa53-40ff-bc6a-015327053af9"), null, 0 },
                    { new Guid("d4d05445-033f-4561-aacb-17646e04c04d"), new DateTime(2019, 10, 15, 6, 49, 14, 598, DateTimeKind.Utc).AddTicks(8190), "dolor sit amet, consectetur adipiscing e", "https://picsum.photos/1200/900?image=1", new Guid("90c9479e-a11c-4d6d-aaaa-0405b6c0efcd"), false, "ut labore et dolore magna aliqua. Ut enim ad minim ", 901.0, new Guid("a162b9ee-85b4-457a-93fc-015df74201dd"), null, 0 },
                    { new Guid("8e175f62-eee0-4e5c-a647-5c87b566f22e"), new DateTime(2019, 10, 15, 6, 49, 14, 598, DateTimeKind.Utc).AddTicks(8240), "est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididun", "https://picsum.photos/1200/900?image=1", new Guid("90c9479e-a11c-4d6d-aaaa-0405b6c0efcd"), false, "non proident, sunt in culpa qui officia deserunt mollit anim id", 661.0, new Guid("ff58a71d-76a2-41f8-af44-018969694a59"), null, 0 },
                    { new Guid("84829642-9c9b-4c84-a209-da742d9175ca"), new DateTime(2019, 10, 15, 6, 49, 14, 598, DateTimeKind.Utc).AddTicks(8251), "ipsum dolor sit amet, consectetur adipis", "https://picsum.photos/1200/900?image=1", new Guid("90c9479e-a11c-4d6d-aaaa-0405b6c0efcd"), false, "tempor incididunt ut labore ", 80.0, new Guid("9032b448-61f2-45f8-9e95-020961441613"), null, 0 },
                    { new Guid("52d2459b-bb2d-4736-82eb-02f138faace1"), new DateTime(2019, 10, 15, 6, 49, 14, 598, DateTimeKind.Utc).AddTicks(8260), "officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipi", "https://picsum.photos/1200/900?image=1", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "aliqua. Ut enim ad minim veniam, quis nostrud exercitation ul", 275.0, new Guid("d16e6353-0f88-43ba-9303-0241672d6ab6"), null, 0 },
                    { new Guid("ee4fae6f-a103-4bc9-943e-6e85fd7e3f29"), new DateTime(2019, 10, 15, 6, 49, 14, 598, DateTimeKind.Utc).AddTicks(8271), "mollit anim id est laborum.Lo", "https://picsum.photos/1200/900?image=1", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "velit esse cillum dolore eu fugiat ", 738.0, new Guid("80258882-2a90-4038-ac48-0283bb0ac9b7"), null, 0 },
                    { new Guid("7221e8d6-ae2c-4283-b685-096bb53a13fc"), new DateTime(2019, 10, 15, 6, 49, 14, 598, DateTimeKind.Utc).AddTicks(8280), "elit, sed do eiusmod tempor incididunt ut labore et dolore magna a", "https://picsum.photos/1200/900?image=1", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "aute irure dolor in re", 51.0, new Guid("a11128b0-dd82-4179-99d9-0288e22db70b"), null, 0 },
                    { new Guid("bbcee1b4-a9cf-450d-b954-96cc0dab89f0"), new DateTime(2019, 10, 15, 6, 49, 14, 598, DateTimeKind.Utc).AddTicks(8289), "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui ", "https://picsum.photos/1200/900?image=1", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsu", 847.0, new Guid("e96a0646-6508-4e40-a035-0294e3b6a017"), null, 0 },
                    { new Guid("12341364-fe69-4239-8020-c008488b17ce"), new DateTime(2019, 10, 15, 6, 49, 14, 598, DateTimeKind.Utc).AddTicks(8298), "voluptate velit esse cillum dolore eu fugiat nulla pariat", "https://picsum.photos/1200/900?image=1", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna a", 2.0, new Guid("d39650d3-7929-4702-bcb9-02978d2c2711"), null, 0 }
                });

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
