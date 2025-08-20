using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Persistance.DataAccess.Migrations;

/// <inheritdoc />
public partial class ResetPassword : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "password_reset_tokens",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				user_id = table.Column<Guid>(type: "uuid", nullable: false),
				token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
				created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				is_used = table.Column<bool>(type: "boolean", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_password_reset_tokens", x => x.id);
				table.ForeignKey(
					name: "fk_password_reset_tokens_users_user_id",
					column: x => x.user_id,
					principalTable: "users",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "ix_password_reset_tokens_user_id",
			table: "password_reset_tokens",
			column: "user_id");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "password_reset_tokens");
	}
}