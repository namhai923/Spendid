using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spendid.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Delete_Behaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_expenses_user_user_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "fk_household_users_user_user_id",
                table: "household_users");

            migrationBuilder.DropForeignKey(
                name: "fk_households_user_admin_id",
                table: "households");

            migrationBuilder.AddForeignKey(
                name: "fk_expenses_user_user_id",
                table: "expenses",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_household_users_user_user_id",
                table: "household_users",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_households_user_admin_id",
                table: "households",
                column: "admin_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_expenses_user_user_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "fk_household_users_user_user_id",
                table: "household_users");

            migrationBuilder.DropForeignKey(
                name: "fk_households_user_admin_id",
                table: "households");

            migrationBuilder.AddForeignKey(
                name: "fk_expenses_user_user_id",
                table: "expenses",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_household_users_user_user_id",
                table: "household_users",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_households_user_admin_id",
                table: "households",
                column: "admin_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
