using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CombatAnalysis.IdentityDAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: "33e2e3d3-9923-4e1b-a207-957b5f0063bb");

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: "f04fb51f-ff00-416f-80e0-40fdc508ece2");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "RefreshToken",
                newName: "TokenSalt");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "RefreshToken",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "ReplacedByTokenId",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenHash",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "AllowedAudiences", "AllowedScopes", "ClientName", "ClientSecret", "ClientType", "CreatedAt", "IsActive", "RedirectUrl", "UpdatedAt" },
                values: new object[,]
                {
                    { "3fc9c6bd-3024-4c44-a081-4ecc3f308042", "user-api,chat-api,communication-api,hubs,notification-api", "api.read,api.write", "desktop", null, 0, new DateTimeOffset(new DateTime(2025, 9, 7, 9, 58, 31, 269, DateTimeKind.Unspecified).AddTicks(9416), new TimeSpan(0, 3, 0, 0, 0)), true, "localhost:45571/callback", new DateTimeOffset(new DateTime(2025, 9, 7, 9, 58, 31, 272, DateTimeKind.Unspecified).AddTicks(4676), new TimeSpan(0, 3, 0, 0, 0)) },
                    { "6a870437-f53a-4983-a7c9-d846238b499f", "user-api,chat-api,communication-api,hubs,notification-api", "api.read,api.write", "web", null, 0, new DateTimeOffset(new DateTime(2025, 9, 7, 9, 58, 31, 272, DateTimeKind.Unspecified).AddTicks(4911), new TimeSpan(0, 3, 0, 0, 0)), true, "localhost:5173/callback", new DateTimeOffset(new DateTime(2025, 9, 7, 9, 58, 31, 272, DateTimeKind.Unspecified).AddTicks(4915), new TimeSpan(0, 3, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: "3fc9c6bd-3024-4c44-a081-4ecc3f308042");

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: "6a870437-f53a-4983-a7c9-d846238b499f");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "ReplacedByTokenId",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "TokenHash",
                table: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "TokenSalt",
                table: "RefreshToken",
                newName: "Token");

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "AllowedAudiences", "AllowedScopes", "ClientName", "ClientSecret", "ClientType", "CreatedAt", "IsActive", "RedirectUrl", "UpdatedAt" },
                values: new object[,]
                {
                    { "33e2e3d3-9923-4e1b-a207-957b5f0063bb", "user-api,chat-api,communication-api,hubs,notification-api", "api.read,api.write", "desktop", null, 0, new DateTimeOffset(new DateTime(2025, 9, 6, 23, 44, 56, 506, DateTimeKind.Unspecified).AddTicks(6071), new TimeSpan(0, 3, 0, 0, 0)), true, "localhost:45571/callback", new DateTimeOffset(new DateTime(2025, 9, 6, 23, 44, 56, 510, DateTimeKind.Unspecified).AddTicks(1533), new TimeSpan(0, 3, 0, 0, 0)) },
                    { "f04fb51f-ff00-416f-80e0-40fdc508ece2", "user-api,chat-api,communication-api,hubs,notification-api", "api.read,api.write", "web", null, 0, new DateTimeOffset(new DateTime(2025, 9, 6, 23, 44, 56, 510, DateTimeKind.Unspecified).AddTicks(1774), new TimeSpan(0, 3, 0, 0, 0)), true, "localhost:5173/callback", new DateTimeOffset(new DateTime(2025, 9, 6, 23, 44, 56, 510, DateTimeKind.Unspecified).AddTicks(1777), new TimeSpan(0, 3, 0, 0, 0)) }
                });
        }
    }
}
