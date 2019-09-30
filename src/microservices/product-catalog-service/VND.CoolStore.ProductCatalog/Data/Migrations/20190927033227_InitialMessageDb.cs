using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VND.CoolStore.ProductCatalog.Data.Migrations
{
    public partial class InitialMessageDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "message");

            migrationBuilder.CreateTable(
                name: "Outboxes",
                schema: "message",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    OccurredOn = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    ProcessedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outboxes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outboxes",
                schema: "message");
        }
    }
}
