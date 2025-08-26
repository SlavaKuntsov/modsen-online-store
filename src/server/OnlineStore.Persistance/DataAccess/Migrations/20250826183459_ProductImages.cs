using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Persistance.DataAccess.Migrations;

/// <inheritdoc />
public partial class ProductImages : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameTable(
			name: "Orders",
			newName: "orders");

		migrationBuilder.RenameTable(
			name: "Carts",
			newName: "сarts");

		migrationBuilder.CreateTable(
			name: "product_images",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				product_id = table.Column<Guid>(type: "uuid", nullable: false),
				object_name = table.Column<string>(type: "text", nullable: false),
				created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                        },
                        constraints: table =>
                        {
                                table.PrimaryKey("pk_product_images", x => x.id);
                                table.ForeignKey(
                                        name: "fk_product_images_products_product_id",
                                        column: x => x.product_id,
                                        principalTable: "products",
                                        principalColumn: "id",
                                        onDelete: ReferentialAction.Cascade);
                        });

                migrationBuilder.CreateIndex(
                        name: "ix_product_images_product_id",
                        table: "product_images",
                        column: "product_id",
                        unique: true);
        }

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "product_images");

		migrationBuilder.RenameTable(
			name: "orders",
			newName: "Orders");

		migrationBuilder.RenameTable(
			name: "сarts",
			newName: "Carts");

		migrationBuilder.AddPrimaryKey(
			name: "pk_carts",
			table: "Carts",
			column: "id");
	}
}