using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalyAPI.Migrations
{
    public partial class dbrecuserr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "recouvreurs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
             table: "recouvreurs",
            type: "nvarchar(450)",
            nullable: true,
    defaultValue: null,
    oldClrType: typeof(string),
    oldType: "nvarchar(450)");

            migrationBuilder.Sql(@"
    UPDATE recouvreurs
    SET UserId = NULL
    WHERE UserId NOT IN (SELECT Id FROM AspNetUsers);
");
       


            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a1b2ab9f-bfde-4196-9350-e5ca4a3d143f", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d29c5d25-3d5e-4a7f-b590-6081a0350630", "3", "RecouvreurContentieux", "RecouvreurContentieux" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "de25d4cf-ee7b-4db7-8a4d-630c93f347bf", "2", "RecouvreurAimable", "RecouvreurAimable" });

            migrationBuilder.CreateIndex(
                name: "IX_recouvreurs_UserId",
                table: "recouvreurs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_recouvreurs_AspNetUsers_UserId",
                table: "recouvreurs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recouvreurs_AspNetUsers_UserId",
                table: "recouvreurs");

            migrationBuilder.DropIndex(
                name: "IX_recouvreurs_UserId",
                table: "recouvreurs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2ab9f-bfde-4196-9350-e5ca4a3d143f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d29c5d25-3d5e-4a7f-b590-6081a0350630");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de25d4cf-ee7b-4db7-8a4d-630c93f347bf");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "recouvreurs");

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
    }
}
