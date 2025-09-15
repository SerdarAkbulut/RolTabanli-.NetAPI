using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_UserDepartments_UserDepartmentId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartments_AspNetUsers_UserId",
                table: "UserDepartments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_UserDepartmentId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UserDepartmentId",
                table: "Departments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserDepartments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "UserDepartments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartments_DepartmentId",
                table: "UserDepartments",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartments_AspNetUsers_UserId",
                table: "UserDepartments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartments_Departments_DepartmentId",
                table: "UserDepartments",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartments_AspNetUsers_UserId",
                table: "UserDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartments_Departments_DepartmentId",
                table: "UserDepartments");

            migrationBuilder.DropIndex(
                name: "IX_UserDepartments_DepartmentId",
                table: "UserDepartments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserDepartments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "UserDepartments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserDepartmentId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UserDepartmentId",
                table: "Departments",
                column: "UserDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_UserDepartments_UserDepartmentId",
                table: "Departments",
                column: "UserDepartmentId",
                principalTable: "UserDepartments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartments_AspNetUsers_UserId",
                table: "UserDepartments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
