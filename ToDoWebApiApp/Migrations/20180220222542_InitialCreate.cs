using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ToDoWebApiApp.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
					name: "TodoTasks",
					columns: table => new
					{
						TodoTaskId = table.Column<int>(nullable: false)
									.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
						IsComplete = table.Column<bool>(nullable: false),
						Name = table.Column<string>(maxLength: 100, nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_TodoTasks", x => x.TodoTaskId);
					});

			migrationBuilder.CreateTable(
					name: "TodoItems",
					columns: table => new
					{
						TotoItemId = table.Column<int>(nullable: false)
									.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
						IsComplete = table.Column<bool>(nullable: false),
						Name = table.Column<string>(maxLength: 100, nullable: false),
						TodoTaskId = table.Column<int>(nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_TodoItems", x => x.TotoItemId);
						table.ForeignKey(
											name: "FK_TodoItems_TodoTasks_TodoTaskId",
											column: x => x.TodoTaskId,
											principalTable: "TodoTasks",
											principalColumn: "TodoTaskId",
											onDelete: ReferentialAction.Cascade);
					});

			migrationBuilder.CreateIndex(
					name: "IX_TodoItems_TodoTaskId",
					table: "TodoItems",
					column: "TodoTaskId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "TodoItems");

			migrationBuilder.DropTable(
					name: "TodoTasks");
		}
	}
}
