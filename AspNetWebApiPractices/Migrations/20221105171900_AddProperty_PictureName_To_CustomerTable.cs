﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetWebApiPractices.Migrations
{
    public partial class AddProperty_PictureName_To_CustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureName",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureName",
                table: "Customers");
        }
    }
}
