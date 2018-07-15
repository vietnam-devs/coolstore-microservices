using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VND.CoolStore.Services.Inventory.Migrations
{
    public partial class init_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inventory_Inventorys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_Inventorys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventory_Inventorys");
        }
    }
}
