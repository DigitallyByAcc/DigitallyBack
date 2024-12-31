using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalyAPI.Migrations
{
    public partial class dbrecouv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1458fd70-a9cc-4171-a9ec-6580f12764b7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63db91a3-4a0d-4f28-aad2-b008af500321");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db43d278-8004-49b6-97ef-59a3d6c8a057");

            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "recouvreurs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Dateofbirth",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "08b6c07f-ad34-47bd-a215-c2f4e275b53a", "3", "RecouvreurContentieux", "RecouvreurContentieux" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4709bc3d-d155-4d84-8c31-9cef5ece2534", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7eb90e34-e1b0-4d65-9598-d024f4648288", "2", "RecouvreurAimable", "RecouvreurAimable" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "08b6c07f-ad34-47bd-a215-c2f4e275b53a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4709bc3d-d155-4d84-8c31-9cef5ece2534");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7eb90e34-e1b0-4d65-9598-d024f4648288");

            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "recouvreurs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Dateofbirth",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1458fd70-a9cc-4171-a9ec-6580f12764b7", "2", "RecouvreurAimable", "RecouvreurAimable" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "63db91a3-4a0d-4f28-aad2-b008af500321", "3", "RecouvreurContentieux", "RecouvreurContentieux" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "db43d278-8004-49b6-97ef-59a3d6c8a057", "1", "Admin", "Admin" });
        }
    }
}
