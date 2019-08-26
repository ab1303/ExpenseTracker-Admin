﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Admin.Repositories.Migrations
{
    public partial class unique_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PartnerUsers_OfxUserGuid_PartnerAppId",
                table: "Users",
                columns: new[] { "OfxUserGuid", "PartnerAppId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PartnerUsers_OfxUserGuid_PartnerAppId",
                table: "Users");
        }
    }
}
