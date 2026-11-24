using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicCollectionAndFavoriteCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Collections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Collections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CollectionId",
                table: "Favorites",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Collections_CollectionId",
                table: "Favorites",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Collections_CollectionId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_CollectionId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Collections");
        }
    }
}
