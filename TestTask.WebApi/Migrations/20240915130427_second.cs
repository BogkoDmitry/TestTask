using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.WebApi.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Exceptions",
                newName: "Text");

            migrationBuilder.AlterColumn<string>(
                name: "TreeName",
                table: "Nodes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Exceptions",
                newName: "Data");

            migrationBuilder.AlterColumn<string>(
                name: "TreeName",
                table: "Nodes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
