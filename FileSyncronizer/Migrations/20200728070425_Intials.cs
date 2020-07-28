using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileSyncronizer.Migrations
{
    public partial class Intials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    Stream = table.Column<byte[]>(nullable: true),
                    Extention = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    EncryptionKey = table.Column<string>(nullable: true),
                    SharingUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");
        }
    }
}
