using Microsoft.EntityFrameworkCore.Migrations;

namespace PartnerUser.Repositories.Migrations
{
    public partial class unique_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PartnerUsers_OfxUserGuid_PartnerAppId",
                table: "PartnerUsers",
                columns: new[] { "OfxUserGuid", "PartnerAppId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PartnerUsers_OfxUserGuid_PartnerAppId",
                table: "PartnerUsers");
        }
    }
}
