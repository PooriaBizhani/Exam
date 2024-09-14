using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace First_Sample.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class chengsometb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsesrId",
                table: "Answers",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Questions",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Choice",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Answers",
                newName: "UsesrId");

            migrationBuilder.AlterColumn<int>(
                name: "Questions",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Choice",
                table: "Answers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
