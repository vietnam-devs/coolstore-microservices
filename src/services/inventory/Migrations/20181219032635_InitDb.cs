using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VND.CoolStore.Services.Inventory.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Inventories",
                columns: new[] { "Id", "Created", "Link", "Location", "Quantity", "Updated", "Version" },
                values: new object[] { new Guid("25e6ba6e-fddb-401d-99b2-33ddc9f29322"), new DateTime(2018, 12, 19, 3, 26, 34, 966, DateTimeKind.Utc).AddTicks(9388), "http://nashtechglobal.com", "London, UK", 100, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.InsertData(
                table: "Inventories",
                columns: new[] { "Id", "Created", "Link", "Location", "Quantity", "Updated", "Version" },
                values: new object[] { new Guid("cab3818f-e459-412f-972f-d4b2d36aa735"), new DateTime(2018, 12, 19, 3, 26, 34, 967, DateTimeKind.Utc).AddTicks(1464), "http://nashtechvietnam.com", "Ho Chi Minh City, Vietnam", 1000, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventories");
        }
    }
}
