using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Persistance.DataAccess.Migrations;

/// <inheritdoc />
public partial class Product_Status : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<Guid>(
			name: "user_id",
			table: "Orders",
			type: "uuid",
			nullable: true,
			oldClrType: typeof(Guid),
			oldType: "uuid");

		migrationBuilder.AddColumn<int>(
			name: "status",
			table: "Orders",
			type: "integer",
			nullable: false,
			defaultValue: 0);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "status",
			table: "Orders");

		migrationBuilder.AlterColumn<Guid>(
			name: "user_id",
			table: "Orders",
			type: "uuid",
			nullable: false,
			defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
			oldClrType: typeof(Guid),
			oldType: "uuid",
			oldNullable: true);
	}
}