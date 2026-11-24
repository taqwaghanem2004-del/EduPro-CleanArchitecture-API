using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class collectioncourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Collections",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Collections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Collections",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Collections_CourseId",
                table: "Collections",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_Courses_CourseId",
                table: "Collections",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collections_Courses_CourseId",
                table: "Collections");

            migrationBuilder.DropIndex(
                name: "IX_Collections_CourseId",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Collections");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
