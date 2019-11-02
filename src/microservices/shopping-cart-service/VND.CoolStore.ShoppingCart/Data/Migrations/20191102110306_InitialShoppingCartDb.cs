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
                    { new Guid("2bff915e-5ae3-4399-bcd9-d9b41d969482"), new DateTime(2019, 11, 2, 11, 3, 6, 294, DateTimeKind.Utc).AddTicks(1501), "IPhone 8", "https://picsum.photos/1200/900?image=1", new Guid("90c9479e-a11c-4d6d-aaaa-0405b6c0efcd"), false, "IPhone 8", 900.0, new Guid("ba16da71-c7dd-4eac-9ead-5c2c2244e69f"), null, 0 },
                    { new Guid("ab7b752a-ac88-4053-be5e-ed0e4c4a896c"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(363), "Implt/repl carddefib tot", "https://picsum.photos/1200/900?image=25", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Soup - Campbells Chili", 3294.0, new Guid("b243a35d-120a-4db3-ad12-7b3fa80e6391"), null, 0 },
                    { new Guid("04404f65-0648-4967-8896-ffae967e721c"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(325), "Oth chest cage rep/plast", "https://picsum.photos/1200/900?image=24", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Lotus Leaves", 1504.0, new Guid("e88e07f8-358d-48f7-b17c-8a16458f9c0a"), null, 0 },
                    { new Guid("8124fd52-5565-47b6-b16b-1a9ef6e67b2a"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(286), "Excision of wrist NEC", "https://picsum.photos/1200/900?image=23", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Mushroom - Lg - Cello", 3318.0, new Guid("89b46ea8-b9a6-40e5-8df3-dba1095695f7"), null, 0 },
                    { new Guid("c8f4e2e5-2fbc-4bdc-be11-1a20b1aa3ae3"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(249), "Appendiceal ops NEC", "https://picsum.photos/1200/900?image=22", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Mix - Cocktail Ice Cream", 232.0, new Guid("3b69e116-9dd6-400f-96ce-9911f4f6ac8b"), null, 0 },
                    { new Guid("9ad64cce-30b0-47aa-808d-ba74aa4c22ee"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(207), "Proximal gastrectomy", "https://picsum.photos/1200/900?image=21", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Ice Cream Bar - Oreo Cone", 2236.0, new Guid("6b8d0110-e3e8-4727-a51e-06f38864e464"), null, 0 },
                    { new Guid("91e4bca2-4b63-429a-afb1-71ec29d9f6d3"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(143), "Hepatic lobectomy", "https://picsum.photos/1200/900?image=20", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Beef - Shank", 3196.0, new Guid("c3770b10-dd0f-4b1c-83aa-d424c175c087"), null, 0 },
                    { new Guid("1460975d-b66c-4ef2-b513-6dc1f0697fc9"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(107), "Interat ven retrn transp", "https://picsum.photos/1200/900?image=20", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Milk - Skim", 3310.0, new Guid("1adbc55a-4354-4205-b96d-c95e2dc806f4"), null, 0 },
                    { new Guid("7a211d61-c5da-4891-af3b-decef054b1c7"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(71), "Plastic rep ext ear NEC", "https://picsum.photos/1200/900?image=19", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Pasta - Cappellini, Dry", 3305.0, new Guid("fac2ccc6-2c3f-4c1e-acbd-5354ba0873fb"), null, 0 },
                    { new Guid("4fb812db-8b7e-4d74-bbc3-ffd6c662406f"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(35), "Chng hnd mus/ten lng NEC", "https://picsum.photos/1200/900?image=18", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Crab - Dungeness, Whole, live", 1665.0, new Guid("cfc5cff8-ab2a-4c70-994d-5ab8ccb7cb0d"), null, 0 },
                    { new Guid("c9e864dc-a949-4b6c-b5e1-8fc941ba8efb"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9996), "Skull plate removal", "https://picsum.photos/1200/900?image=17", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Oil - Olive", 1124.0, new Guid("97ad5bf4-d153-41c5-a6e0-6d0bfbbb4f67"), null, 0 },
                    { new Guid("cfc3bbe3-7576-4f99-89e7-6b1ff16e064e"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9954), "Vessel operation NEC", "https://picsum.photos/1200/900?image=16", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Beef - Tenderloin Tails", 967.0, new Guid("22112bb2-c324-4860-8eb9-9c78853a52a5"), null, 0 },
                    { new Guid("6aad4645-5b3d-4b42-bb81-eb53e916f7d2"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9853), "Tendon excision for grft", "https://picsum.photos/1200/900?image=15", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Tarragon - Primerba, Paste", 642.0, new Guid("85b9767c-1a09-4c33-8e77-faa25f1d69e1"), null, 0 },
                    { new Guid("cb2cab44-3c47-4f4b-b2e5-36344c75b4e3"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9816), "Opn/oth part gastrectomy", "https://picsum.photos/1200/900?image=14", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Ecolab - Balanced Fusion", 1769.0, new Guid("cbe43158-2010-4cb1-a8de-f00da16a7362"), null, 0 },
                    { new Guid("fe44ccd1-7a97-4e68-809f-d76cc0a58849"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9780), "Toxicology-endocrine", "https://picsum.photos/1200/900?image=13", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Godiva White Chocolate", 2067.0, new Guid("f92bfa6a-2522-4234-a7f1-9004596a4a85"), null, 0 },
                    { new Guid("af7398b6-5b33-49d0-b00b-666c5f86c44d"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9743), "Oth thorac op thymus NOS", "https://picsum.photos/1200/900?image=12", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Lettuce - Boston Bib", 3453.0, new Guid("71c46659-9560-4d6a-ac18-893477ed6662"), null, 0 },
                    { new Guid("b118e886-3bdf-4e00-8e53-62416aea1438"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9707), "Other bone dx proc NEC", "https://picsum.photos/1200/900?image=11", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Cheese - Swiss", 975.0, new Guid("3a0a0a89-9b3a-4046-bf2b-deee64a764d2"), null, 0 },
                    { new Guid("1b9a1364-0b3b-40b9-a22e-946629985bda"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9670), "Remove bladder stimulat", "https://picsum.photos/1200/900?image=10", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Oranges - Navel, 72", 1731.0, new Guid("297c5959-4808-4f40-8d6a-4a899505e1f7"), null, 0 },
                    { new Guid("2368006a-66f1-4537-a8ce-054dc828abaf"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9626), "Removal of FB NOS", "https://picsum.photos/1200/900?image=9", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Hersey Shakes", 2441.0, new Guid("386b04c6-303a-4840-8a51-d92b1ea2d339"), null, 0 },
                    { new Guid("8fb095ab-c964-47a9-8199-da74813e478c"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9551), "Tibia/fibula inj op NOS", "https://picsum.photos/1200/900?image=8", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Wonton Wrappers", 2200.0, new Guid("2d2245e4-213a-49de-93d3-79e9439400f5"), null, 0 },
                    { new Guid("9a268fe5-4588-4e93-ab51-e327f43ce480"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9514), "Open periph nerve biopsy", "https://picsum.photos/1200/900?image=7", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Bag - Regular Kraft 20 Lb", 2147.0, new Guid("fee1fc67-7469-4490-b418-47f4732de53f"), null, 0 },
                    { new Guid("9cb36c66-2172-46ad-836a-14de6ee1c2fb"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9477), "Fiber-optic bronchoscopy", "https://picsum.photos/1200/900?image=6", new Guid("ec186ddf-f430-44ec-84e5-205c93d84f14"), false, "Bread - White, Unsliced", 2809.0, new Guid("6a0e6d20-8bcc-450f-bc5c-b8f727083dcd"), null, 0 },
                    { new Guid("67e7acb3-05b2-4b3f-8df7-0940fc95338e"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9429), "Mastoidectomy revision", "https://picsum.photos/1200/900?image=5", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Cheese - Camembert", 253.0, new Guid("a4811778-5070-4d70-8a9c-e6cb70dfcca4"), null, 0 },
                    { new Guid("ef59b997-c867-44bd-819e-960bfec71ef1"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9391), "Asus UX370U i7 8550U (C4217TS)", "https://picsum.photos/1200/900?image=4", new Guid("90c9479e-a11c-4d6d-aaaa-0405b6c0efcd"), false, "Asus UX370U i7 8550U (C4217TS)", 500.0, new Guid("ffd60654-1802-48bd-b4c3-d49831a8ab2c"), null, 0 },
                    { new Guid("e757dd63-3cfd-42a3-bfd4-92241afe4f9a"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9341), "MacBook Pro 2019", "https://picsum.photos/1200/900?image=3", new Guid("90c9479e-a11c-4d6d-aaaa-0405b6c0efcd"), false, "MacBook Pro 2019", 4000.0, new Guid("b8f0a771-339f-4602-a862-f7a51afd5b79"), null, 0 },
                    { new Guid("1a2b2109-33db-4a3b-96f0-88394cb68328"), new DateTime(2019, 11, 2, 11, 3, 6, 295, DateTimeKind.Utc).AddTicks(9080), "IPhone X", "https://picsum.photos/1200/900?image=2", new Guid("90c9479e-a11c-4d6d-aaaa-0405b6c0efcd"), false, "IPhone X", 1000.0, new Guid("13d02035-2286-4055-ad2d-6855a60efbbb"), null, 0 },
                    { new Guid("041e4f4c-0a3a-4016-849f-5124d04775d5"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(399), "Wound catheter irrigat", "https://picsum.photos/1200/900?image=26", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Longos - Penne With Pesto", 3639.0, new Guid("6e3ac253-517d-48e5-96ad-800451f8591c"), null, 0 },
                    { new Guid("fd85394c-420e-486e-a937-08d529ade7e1"), new DateTime(2019, 11, 2, 11, 3, 6, 296, DateTimeKind.Utc).AddTicks(594), "Abdomen wall repair NEC", "https://picsum.photos/1200/900?image=27", new Guid("b8b62196-6369-409d-b709-11c112dd023f"), false, "Prunes - Pitted", 1191.0, new Guid("4693520a-2b14-4d90-8b64-541575511382"), null, 0 }
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
