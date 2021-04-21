using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class CreatePassion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Passions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    YearOfStudy = table.Column<int>(type: "integer", nullable: false),
                    Bio = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    AccessLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PassionUser",
                columns: table => new
                {
                    PassionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassionUser", x => new { x.PassionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_PassionUser_Passions_PassionsId",
                        column: x => x.PassionsId,
                        principalTable: "Passions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassionUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PassionUser_UsersId",
                table: "PassionUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassionUser");

            migrationBuilder.DropTable(
                name: "Passions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
