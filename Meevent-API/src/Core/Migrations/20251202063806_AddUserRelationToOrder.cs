using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meevent_API.src.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRelationToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_orders_user_id",
                table: "orders",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_Users_user_id",
                table: "orders",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_Users_user_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_user_id",
                table: "orders");
        }
    }
}
