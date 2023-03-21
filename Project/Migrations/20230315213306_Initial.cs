using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "MovieCategories",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_MovieCategories", x => x.Id);
            });

        _ = migrationBuilder.CreateTable(
            name: "StaffRoles",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Powers = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_StaffRoles", x => x.Id);
            });

        _ = migrationBuilder.CreateTable(
            name: "UserAccounts",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RealName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Alias = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_UserAccounts", x => x.Id);
            });

        _ = migrationBuilder.CreateTable(
            name: "Movies",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                ReleaseDate = table.Column<DateTime>(type: "date", nullable: false),
                CategoryId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Movies", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Movies_MovieCategories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "MovieCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "StaffAccounts",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<int>(type: "int", nullable: false),
                RealName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Alias = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_StaffAccounts", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_StaffAccounts_StaffRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "StaffRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_Movies_CategoryId",
            table: "Movies",
            column: "CategoryId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_StaffAccounts_RoleId",
            table: "StaffAccounts",
            column: "RoleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "Movies");

        _ = migrationBuilder.DropTable(
            name: "StaffAccounts");

        _ = migrationBuilder.DropTable(
            name: "UserAccounts");

        _ = migrationBuilder.DropTable(
            name: "MovieCategories");

        _ = migrationBuilder.DropTable(
            name: "StaffRoles");
    }
}
