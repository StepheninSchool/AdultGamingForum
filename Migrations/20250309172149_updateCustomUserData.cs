using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdultGamingForum.Migrations
{
    /// <inheritdoc />
    public partial class updateCustomUserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForHire",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "AspNetUsers",
                newName: "Location");

            migrationBuilder.AddColumn<string>(
                name: "FavoriteGame",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageFilename",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoriteGame",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageFilename",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "AspNetUsers",
                newName: "location");

            migrationBuilder.AddColumn<bool>(
                name: "IsForHire",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
