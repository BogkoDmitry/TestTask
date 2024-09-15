using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.WebApi.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Exceptions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EventId",
                table: "Exceptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
