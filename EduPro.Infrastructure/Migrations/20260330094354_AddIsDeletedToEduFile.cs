using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToEduFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EduFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EduFiles");
        }
    }
}
