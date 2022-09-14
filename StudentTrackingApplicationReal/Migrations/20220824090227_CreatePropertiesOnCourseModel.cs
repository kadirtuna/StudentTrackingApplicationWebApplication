using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentTrackingApplicationReal.Migrations
{
    public partial class CreatePropertiesOnCourseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagerName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ManagerSurname = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ManagerUserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ManagerEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManagerPhone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ManagerCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.ManagerId);
                });

            migrationBuilder.CreateTable(
                name: "SchoolUsers",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolUsers", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    StudentSurname = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    StudentUserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    StudentEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudentPhone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StudentTotalCourseCredits = table.Column<int>(type: "int", nullable: false),
                    StudentGeneralAverageMark = table.Column<int>(type: "int", nullable: false),
                    StudentDebts = table.Column<int>(type: "int", nullable: false),
                    StudentCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TeacherSurname = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TeacherUserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TeacherEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TeacherPhone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TeacherCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.TeacherId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseCredits = table.Column<int>(type: "int", nullable: false),
                    CourseNumbers = table.Column<int>(type: "int", nullable: false),
                    CourseTotalPrice = table.Column<int>(type: "int", nullable: false),
                    CourseCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseDays = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    TimeFirst2Digits = table.Column<int>(type: "int", nullable: false),
                    TimeLast2Digits = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentsCourses",
                columns: table => new
                {
                    StudentCourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentMidTermMark = table.Column<int>(type: "int", nullable: false),
                    StudentFinalMark = table.Column<int>(type: "int", nullable: false),
                    StudentAverageMark = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsCourses", x => x.StudentCourseId);
                    table.ForeignKey(
                        name: "FK_StudentsCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsCourses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendancePrice = table.Column<int>(type: "int", nullable: false),
                    AttendanceState = table.Column<bool>(type: "bit", nullable: false),
                    AttendancePaymentState = table.Column<bool>(type: "bit", nullable: false),
                    StudentCourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_Attendances_StudentsCourses_StudentCourseId",
                        column: x => x.StudentCourseId,
                        principalTable: "StudentsCourses",
                        principalColumn: "StudentCourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentCourseId",
                table: "Attendances",
                column: "StudentCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsCourses_CourseId",
                table: "StudentsCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsCourses_StudentId",
                table: "StudentsCourses",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropTable(
                name: "SchoolUsers");

            migrationBuilder.DropTable(
                name: "StudentsCourses");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
