using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VND.CoolStore.Services.Review.Migrations
{
    public partial class InitReviewDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewAuthors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewAuthors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    ReviewAuthorId = table.Column<Guid>(nullable: false),
                    ReviewProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_ReviewAuthors_ReviewAuthorId",
                        column: x => x.ReviewAuthorId,
                        principalTable: "ReviewAuthors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_ReviewProducts_ReviewProductId",
                        column: x => x.ReviewProductId,
                        principalTable: "ReviewProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewAuthorId",
                table: "Reviews",
                column: "ReviewAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewProductId",
                table: "Reviews",
                column: "ReviewProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "ReviewAuthors");

            migrationBuilder.DropTable(
                name: "ReviewProducts");
        }
    }
}
