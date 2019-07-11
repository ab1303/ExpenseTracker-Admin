using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Admin.Repositories.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartnerUsers",
                columns: table => new
                {
                    PartnerUserId = table.Column<Guid>(maxLength: 36, nullable: false),
                    PartnerAppId = table.Column<Guid>(maxLength: 36, nullable: false),
                    OfxUserGuid = table.Column<Guid>(maxLength: 36, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BeneficiaryId = table.Column<Guid>(maxLength: 36, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerUsers", x => x.PartnerUserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartnerUsers");
        }
    }
}
