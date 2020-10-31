using Microsoft.EntityFrameworkCore.Migrations;
using N8T.Infrastructure.EfCore;

namespace ProductCatalogService.Infrastructure.Data.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.MigrateDataFromScript();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
