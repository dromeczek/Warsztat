using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshoManager.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletionDateToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceTaskId1",
                table: "UsedParts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsedParts_ServiceTaskId1",
                table: "UsedParts",
                column: "ServiceTaskId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedParts_ServiceTasks_ServiceTaskId1",
                table: "UsedParts",
                column: "ServiceTaskId1",
                principalTable: "ServiceTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedParts_ServiceTasks_ServiceTaskId1",
                table: "UsedParts");

            migrationBuilder.DropIndex(
                name: "IX_UsedParts_ServiceTaskId1",
                table: "UsedParts");

            migrationBuilder.DropColumn(
                name: "ServiceTaskId1",
                table: "UsedParts");

            migrationBuilder.DropColumn(
                name: "CompletionDate",
                table: "Orders");
        }
    }
}
