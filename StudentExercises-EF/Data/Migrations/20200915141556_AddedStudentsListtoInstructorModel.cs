using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentExercises_EF.Data.Migrations
{
    public partial class AddedStudentsListtoInstructorModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Student",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_InstructorId",
                table: "Student",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Instructor_InstructorId",
                table: "Student",
                column: "InstructorId",
                principalTable: "Instructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Instructor_InstructorId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_InstructorId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Student");
        }
    }
}
