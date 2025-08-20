using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Persistance.DataAccess.Migrations;

/// <inheritdoc />
public partial class Product : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<decimal>(
			name: "price",
			table: "products",
			type: "numeric(18,2)",
			nullable: false,
			oldClrType: typeof(decimal),
			oldType: "numeric");

		migrationBuilder.AlterColumn<string>(
			name: "name",
			table: "products",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			oldClrType: typeof(string),
			oldType: "text");

		migrationBuilder.AddColumn<DateTime>(
			name: "created_at",
			table: "products",
			type: "timestamp with time zone",
			nullable: false,
			defaultValueSql: "CURRENT_TIMESTAMP");

		migrationBuilder.AddColumn<int>(
			name: "popularity",
			table: "products",
			type: "integer",
			nullable: false,
			defaultValue: 0);

		migrationBuilder.AddColumn<double>(
			name: "rating",
			table: "products",
			type: "double precision",
			nullable: false,
			defaultValue: 0.0);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "created_at",
			table: "products");

		migrationBuilder.DropColumn(
			name: "popularity",
			table: "products");

		migrationBuilder.DropColumn(
			name: "rating",
			table: "products");

		migrationBuilder.AlterColumn<decimal>(
			name: "price",
			table: "products",
			type: "numeric",
			nullable: false,
			oldClrType: typeof(decimal),
			oldType: "numeric(18,2)");

		migrationBuilder.AlterColumn<string>(
			name: "name",
			table: "products",
			type: "text",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);
	}
}