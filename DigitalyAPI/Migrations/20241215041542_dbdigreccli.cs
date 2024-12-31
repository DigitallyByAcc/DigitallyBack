using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalyAPI.Migrations
{
    public partial class dbdigreccli : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "RecouvreurId",
                table: "clients",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "43e0dd05-e819-4a2c-8504-fd8bbd7e8fe4", "3", "RecouvreurContentieux", "RecouvreurContentieux" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bd6950b8-6dbf-4d37-8d68-ab5296c733e9", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e1d1786c-d15d-4874-bd82-95ef106e7e15", "2", "RecouvreurAimable", "RecouvreurAimable" });

            migrationBuilder.CreateIndex(
                name: "IX_clients_RecouvreurId",
                table: "clients",
                column: "RecouvreurId");

            migrationBuilder.AddForeignKey(
                name: "FK_clients_recouvreurs_RecouvreurId",
                table: "clients",
                column: "RecouvreurId",
                principalTable: "recouvreurs",
                principalColumn: "IdRecouvreur",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clients_recouvreurs_RecouvreurId",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "IX_clients_RecouvreurId",
                table: "clients");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43e0dd05-e819-4a2c-8504-fd8bbd7e8fe4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd6950b8-6dbf-4d37-8d68-ab5296c733e9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1d1786c-d15d-4874-bd82-95ef106e7e15");

            migrationBuilder.DropColumn(
                name: "RecouvreurId",
                table: "clients");

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
    }
}
