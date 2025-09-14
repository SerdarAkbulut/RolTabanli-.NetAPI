using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class Department : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartments_Departments_DepartmentId",
                table: "UserDepartments");

            migrationBuilder.DropIndex(
                name: "IX_UserDepartments_DepartmentId",
                table: "UserDepartments");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_UserDepartments_UserDepartmentId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_UserDepartmentId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UserDepartmentId",
                table: "Departments");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartments_DepartmentId",
                table: "UserDepartments",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartments_Departments_DepartmentId",
                table: "UserDepartments",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
