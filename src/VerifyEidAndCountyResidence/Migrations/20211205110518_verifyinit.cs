using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VerifyEidAndCountyResidence.Migrations
{
    public partial class verifyinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DidId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DidTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DidData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dids", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EidAndCountyResidenceDataPresentationTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DidEid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DidCountyResidence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MattrPresentationTemplateReponse = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EidAndCountyResidenceDataPresentationTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EidAndCountyResidenceDataPresentationVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DidEid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DidCountyResidence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallbackUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvokePresentationResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Did = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignAndEncodePresentationRequestBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Challenge = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EidAndCountyResidenceDataPresentationVerifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerifiedEidAndCountyResidenceData",
                columns: table => new
                {
                    ChallengeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PresentationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimsId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    Holder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLocality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiedEidAndCountyResidenceData", x => x.ChallengeId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dids");

            migrationBuilder.DropTable(
                name: "EidAndCountyResidenceDataPresentationTemplates");

            migrationBuilder.DropTable(
                name: "EidAndCountyResidenceDataPresentationVerifications");

            migrationBuilder.DropTable(
                name: "VerifiedEidAndCountyResidenceData");
        }
    }
}
