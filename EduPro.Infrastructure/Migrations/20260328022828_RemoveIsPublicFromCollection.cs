using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsPublicFromCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Collections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Collections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
