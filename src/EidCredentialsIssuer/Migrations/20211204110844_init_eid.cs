using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EidCredentialsIssuer.Migrations
{
    public partial class init_eid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EidDataCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OidcIssuerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OidcIssuer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Did = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EidDataCredentials", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EidDataCredentials");
        }
    }
}
