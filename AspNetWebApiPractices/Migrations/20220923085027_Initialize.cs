using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetWebApiPractices.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "FullName" },
                values: new object[] { 1, "Roy Fielding" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "FullName" },
                values: new object[] { 2, "John Doe" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "FullName" },
                values: new object[] { 3, "Ali Gholipour" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
