using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aspProject.Migrations
{
    /// <inheritdoc />
    public partial class AddFraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fraction",
                table: "Characters",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fraction",
                table: "Characters");
        }
    }
}
