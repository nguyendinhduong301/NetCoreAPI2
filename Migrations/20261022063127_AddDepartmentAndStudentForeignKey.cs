
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoMVC.Migrations
{
    public partial class AddDepartmentAndStudentForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa cột University cũ.
            migrationBuilder.DropColumn(
                name: "University",
                table: "Student");

            // Giới hạn họ tên tối đa 50 ký tự.
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Student",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Chuyển Address thành NOT NULL.
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // Tạo bảng Employees theo Model hiện tại.
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmpId = table.Column<string>(
                        type: "nvarchar(450)",
                        nullable: false),

                    FullName = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmpId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Khôi phục cột University.
            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);

            // Xóa bảng Employees nếu rollback.
            migrationBuilder.DropTable(
                name: "Employees");

            // Khôi phục kiểu dữ liệu FullName.
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            // Khôi phục Address cho phép NULL.
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
