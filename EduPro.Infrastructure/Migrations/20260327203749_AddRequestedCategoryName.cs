using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestedCategoryName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EduFiles_PendingCourseRequests_PendingCourseRequestId",
                table: "EduFiles");

            migrationBuilder.DropTable(
                name: "PendingCourseRequests");

            migrationBuilder.DropIndex(
                name: "IX_EduFiles_PendingCourseRequestId",
                table: "EduFiles");

            migrationBuilder.DropColumn(
                name: "PendingCourseRequestId",
                table: "EduFiles");

            migrationBuilder.AddColumn<string>(
                name: "RequestedCategoryName",
                table: "EduFiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestedCategoryName",
                table: "EduFiles");

            migrationBuilder.AddColumn<int>(
                name: "PendingCourseRequestId",
                table: "EduFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PendingCourseRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResolvedCourseId = table.Column<int>(type: "int", nullable: true),
                    CourseName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingCourseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingCourseRequests_Courses_ResolvedCourseId",
                        column: x => x.ResolvedCourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EduFiles_PendingCourseRequestId",
                table: "EduFiles",
                column: "PendingCourseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingCourseRequests_ResolvedCourseId",
                table: "PendingCourseRequests",
                column: "ResolvedCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EduFiles_PendingCourseRequests_PendingCourseRequestId",
                table: "EduFiles",
                column: "PendingCourseRequestId",
                principalTable: "PendingCourseRequests",
                principalColumn: "Id");
        }
    }
}
