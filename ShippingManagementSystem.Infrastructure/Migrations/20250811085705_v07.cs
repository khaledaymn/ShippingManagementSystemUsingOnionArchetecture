using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v07 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippigRepresentatives_ShippigRepresentativeId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippigRepresentativeId",
                table: "Orders",
                newName: "ShippingRepresentativeId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippigRepresentativeId",
                table: "Orders",
                newName: "IX_Orders_ShippingRepresentativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippigRepresentatives_ShippingRepresentativeId",
                table: "Orders",
                column: "ShippingRepresentativeId",
                principalTable: "ShippigRepresentatives",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippigRepresentatives_ShippingRepresentativeId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingRepresentativeId",
                table: "Orders",
                newName: "ShippigRepresentativeId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippingRepresentativeId",
                table: "Orders",
                newName: "IX_Orders_ShippigRepresentativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippigRepresentatives_ShippigRepresentativeId",
                table: "Orders",
                column: "ShippigRepresentativeId",
                principalTable: "ShippigRepresentatives",
                principalColumn: "UserID");
        }
    }
}
