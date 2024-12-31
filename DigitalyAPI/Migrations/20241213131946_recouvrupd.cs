using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalyAPI.Migrations
{
    public partial class recouvrupd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20cfbc7a-d86b-4221-8862-2c04b6892af5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "617e6f20-e4d0-479a-b19b-21e7e8861124");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cfacd2bb-63cd-42af-9992-08312a556965");

            migrationBuilder.AddColumn<string>(
                name: "refrecouvreur",
                table: "recouvreurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "15fdb4ca-0acf-4a85-ac53-d6d70ecddf6e", "3", "RecouvreurContentieux", "RecouvreurContentieux" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "47a6bd65-7804-4f37-9265-d4e61fc1a334", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "73b5b544-7345-4b6d-ae02-8619f0469412", "2", "RecouvreurAimable", "RecouvreurAimable" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15fdb4ca-0acf-4a85-ac53-d6d70ecddf6e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47a6bd65-7804-4f37-9265-d4e61fc1a334");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73b5b544-7345-4b6d-ae02-8619f0469412");

            migrationBuilder.DropColumn(
                name: "refrecouvreur",
                table: "recouvreurs");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "20cfbc7a-d86b-4221-8862-2c04b6892af5", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "617e6f20-e4d0-479a-b19b-21e7e8861124", "3", "RecouvreurContentieux", "RecouvreurContentieux" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cfacd2bb-63cd-42af-9992-08312a556965", "2", "RecouvreurAimable", "RecouvreurAimable" });
        }
    }
}
