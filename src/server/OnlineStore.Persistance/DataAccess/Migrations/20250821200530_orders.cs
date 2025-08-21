using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Persistance.DataAccess.Migrations;

/// <inheritdoc />
public partial class orders : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "cart_items");

		migrationBuilder.RenameTable(
			name: "carts",
			newName: "Carts");

		migrationBuilder.CreateTable(
			name: "CartItems",
			columns: table => new
			{
				product_id = table.Column<Guid>(type: "uuid", nullable: false),
				cart_id = table.Column<Guid>(type: "uuid", nullable: false),
				product_name = table.Column<string>(type: "text", nullable: false),
				unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
				quantity = table.Column<int>(type: "integer", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_cart_items", x => new { x.cart_id, x.product_id });
				table.ForeignKey(
					name: "fk_cart_items_carts_cart_id",
					column: x => x.cart_id,
					principalTable: "Carts",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "Orders",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				user_id = table.Column<Guid>(type: "uuid", nullable: false),
				delivery_method = table.Column<int>(type: "integer", nullable: false),
				shipping_address = table.Column<string>(type: "text", nullable: false),
				created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_orders", x => x.id);
			});

		migrationBuilder.CreateTable(
			name: "OrderItems",
			columns: table => new
			{
				product_id = table.Column<Guid>(type: "uuid", nullable: false),
				order_id = table.Column<Guid>(type: "uuid", nullable: false),
				product_name = table.Column<string>(type: "text", nullable: false),
				unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
				quantity = table.Column<int>(type: "integer", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_order_items", x => new { x.order_id, x.product_id });
				table.ForeignKey(
					name: "fk_order_items_orders_order_id",
					column: x => x.order_id,
					principalTable: "Orders",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "CartItems");

		migrationBuilder.DropTable(
			name: "OrderItems");

		migrationBuilder.DropTable(
			name: "Orders");

		migrationBuilder.RenameTable(
			name: "Carts",
			newName: "carts");

		migrationBuilder.CreateTable(
			name: "cart_items",
			columns: table => new
			{
				cart_id = table.Column<Guid>(type: "uuid", nullable: false),
				product_id = table.Column<Guid>(type: "uuid", nullable: false),
				product_name = table.Column<string>(type: "text", nullable: false),
				quantity = table.Column<int>(type: "integer", nullable: false),
				unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_cart_items", x => new { x.cart_id, x.product_id });
				table.ForeignKey(
					name: "fk_cart_items_carts_cart_id",
					column: x => x.cart_id,
					principalTable: "carts",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});
	}
}