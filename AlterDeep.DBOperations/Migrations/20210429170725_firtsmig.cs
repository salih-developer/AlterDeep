using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace AlterDeep.DBOperations.Migrations
{
    public partial class firtsmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ContentText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    FriendlyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionPageContents",
                columns: table => new
                {
                    TransactionPageId = table.Column<int>(nullable: false),
                    ContentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPageContents", x => new { x.TransactionPageId, x.ContentId });
                    table.ForeignKey(
                        name: "FK_TransactionPageContents_Content_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionPageContents_TransactionPage_TransactionPageId",
                        column: x => x.TransactionPageId,
                        principalTable: "TransactionPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionPageContents_ContentId",
                table: "TransactionPageContents",
                column: "ContentId");

            migrationBuilder.Sql(
                "CREATE TABLE `__EFMigrationsHistory` (`MigrationId` varchar(150) NOT NULL, `ProductVersion` varchar(32) NOT NULL,  PRIMARY KEY (`MigrationId`));");
         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flow");

            migrationBuilder.DropTable(
                name: "TransactionPageContents");

            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "TransactionPage");
        }
    }
}
