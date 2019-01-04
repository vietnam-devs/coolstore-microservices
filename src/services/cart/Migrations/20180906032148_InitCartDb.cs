using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VND.CoolStore.Services.Cart.Migrations
{
    public partial class InitCartDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Carts",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CartItemTotal = table.Column<double>(nullable: false),
                    CartItemPromoSavings = table.Column<double>(nullable: false),
                    ShippingTotal = table.Column<double>(nullable: false),
                    ShippingPromoSavings = table.Column<double>(nullable: false),
                    CartTotal = table.Column<double>(nullable: false),
                    IsCheckout = table.Column<bool>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Carts", x => x.Id); });

            migrationBuilder.CreateTable(
                "CartItems",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    PromoSavings = table.Column<double>(nullable: false),
                    CartId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        "FK_CartItems_Carts_CartId",
                        x => x.CartId,
                        "Carts",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Products",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        "FK_Products_CartItems_Id",
                        x => x.Id,
                        "CartItems",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_CartItems_CartId",
                "CartItems",
                "CartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Products");

            migrationBuilder.DropTable(
                "CartItems");

            migrationBuilder.DropTable(
                "Carts");
        }
    }
}
