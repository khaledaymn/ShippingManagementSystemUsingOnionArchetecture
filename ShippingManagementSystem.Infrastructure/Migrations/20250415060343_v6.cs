using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                  name: "PK_GroupMedules",
                  table: "GroupMedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMedules",
                table: "GroupMedules",
                columns: new[] { "MeduleId", "GroupId", "Permission" });
          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMedules",
                table: "GroupMedules");

        }
    }
}
