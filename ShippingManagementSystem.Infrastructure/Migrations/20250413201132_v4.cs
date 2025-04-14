using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingRepGovernorate_ShippigRepresentatives_ShippingRepId",
                table: "ShippingRepGovernorate");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingRepGovernorate_governorates_GovernorateId",
                table: "ShippingRepGovernorate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingRepGovernorate",
                table: "ShippingRepGovernorate");

            migrationBuilder.RenameTable(
                name: "ShippingRepGovernorate",
                newName: "ShippingRepGovernorates");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingRepGovernorate_GovernorateId",
                table: "ShippingRepGovernorates",
                newName: "IX_ShippingRepGovernorates_GovernorateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingRepGovernorates",
                table: "ShippingRepGovernorates",
                columns: new[] { "ShippingRepId", "GovernorateId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingRepGovernorates_ShippigRepresentatives_ShippingRepId",
                table: "ShippingRepGovernorates",
                column: "ShippingRepId",
                principalTable: "ShippigRepresentatives",
                principalColumn: "UserID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingRepGovernorates_governorates_GovernorateId",
                table: "ShippingRepGovernorates",
                column: "GovernorateId",
                principalTable: "governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingRepGovernorates_ShippigRepresentatives_ShippingRepId",
                table: "ShippingRepGovernorates");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingRepGovernorates_governorates_GovernorateId",
                table: "ShippingRepGovernorates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingRepGovernorates",
                table: "ShippingRepGovernorates");

            migrationBuilder.RenameTable(
                name: "ShippingRepGovernorates",
                newName: "ShippingRepGovernorate");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingRepGovernorates_GovernorateId",
                table: "ShippingRepGovernorate",
                newName: "IX_ShippingRepGovernorate_GovernorateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingRepGovernorate",
                table: "ShippingRepGovernorate",
                columns: new[] { "ShippingRepId", "GovernorateId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingRepGovernorate_ShippigRepresentatives_ShippingRepId",
                table: "ShippingRepGovernorate",
                column: "ShippingRepId",
                principalTable: "ShippigRepresentatives",
                principalColumn: "UserID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingRepGovernorate_governorates_GovernorateId",
                table: "ShippingRepGovernorate",
                column: "GovernorateId",
                principalTable: "governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
