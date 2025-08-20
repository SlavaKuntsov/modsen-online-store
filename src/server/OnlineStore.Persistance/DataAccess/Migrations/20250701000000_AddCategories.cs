using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

public partial class AddCategories : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "categories",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
				parent_category_id = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_categories", x => x.id);
				table.ForeignKey(
					name: "fk_categories_categories_parent_category_id",
					column: x => x.parent_category_id,
					principalTable: "categories",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "ix_categories_parent_category_id",
			table: "categories",
			column: "parent_category_id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "categories");
	}
}