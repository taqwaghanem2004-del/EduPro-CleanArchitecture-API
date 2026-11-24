using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEduFileCourseDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EduFiles_Courses_CourseId",
                table: "EduFiles");

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
                name: "IsDeleted",
                table: "Collections");

            migrationBuilder.AddForeignKey(
                name: "FK_EduFiles_Courses_CourseId",
                table: "EduFiles",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EduFiles_Courses_CourseId",
                table: "EduFiles");

            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Collections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CollectionId",
                table: "Favorites",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_EduFiles_Courses_CourseId",
                table: "EduFiles",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Collections_CollectionId",
                table: "Favorites",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id");
        }
    }
}
