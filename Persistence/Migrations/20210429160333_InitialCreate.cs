using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hates", x => x.Id);
                });

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
                    Sex = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    SexualOrientation = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    YearOfStudy = table.Column<int>(type: "integer", nullable: false),
                    Bio = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    AccessLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CK_ValidSexValue", "\"Sex\" in ('Male', 'Female', 'Other')");
                    table.CheckConstraint("CK_ValidSexualOrientationValue", "\"SexualOrientation\" in ('Straight', 'Gay', 'Lesbian','Bisexual', 'Asexual', 'Demisexual', 'Pansexual', 'Queer', 'Bicurious', 'Aromantic')");
                });

            migrationBuilder.CreateTable(
                name: "HateUser",
                columns: table => new
                {
                    HatesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HateUser", x => new { x.HatesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_HateUser_Hates_HatesId",
                        column: x => x.HatesId,
                        principalTable: "Hates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HateUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    OtherId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Users_OtherId",
                        column: x => x.OtherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_HateUser_UsersId",
                table: "HateUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_OtherId",
                table: "Matches",
                column: "OtherId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_UserId",
                table: "Matches",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PassionUser_UsersId",
                table: "PassionUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HateUser");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "PassionUser");

            migrationBuilder.DropTable(
                name: "Hates");

            migrationBuilder.DropTable(
                name: "Passions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
