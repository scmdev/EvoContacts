using Microsoft.EntityFrameworkCore.Migrations;

namespace EvoContacts.Infrastructure.Migrations
{
    public partial class SMAddeduniqueindexonContactentityEmailproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Email",
                table: "Contacts",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL AND IsDeleted = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contacts_Email",
                table: "Contacts");
        }
    }
}
