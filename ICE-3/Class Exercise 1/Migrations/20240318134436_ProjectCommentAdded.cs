using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Exercise_1.Migrations
{
    /// <inheritdoc />
    public partial class ProjectCommentAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "ProjectComments",
                newName: "DatePosted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "ProjectComments",
                newName: "CreatedDate");
        }
    }
}
