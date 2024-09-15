using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.WebApi.Migrations
{
    public partial class fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nodes_Nodes_TreeNodeId",
                table: "Nodes");

            migrationBuilder.DropIndex(
                name: "IX_Nodes_TreeNodeId",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "TreeNodeId",
                table: "Nodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TreeNodeId",
                table: "Nodes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_TreeNodeId",
                table: "Nodes",
                column: "TreeNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nodes_Nodes_TreeNodeId",
                table: "Nodes",
                column: "TreeNodeId",
                principalTable: "Nodes",
                principalColumn: "Id");
        }
    }
}
