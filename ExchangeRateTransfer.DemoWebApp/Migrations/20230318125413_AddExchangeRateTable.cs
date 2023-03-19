using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeRateTransfer.DemoWebApp.Migrations
{
    public partial class AddExchangeRateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExchangeRateDate = table.Column<DateTime>(type: "Date", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "Date", nullable: false),
                    BulletinNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForexBuying = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: false),
                    ForexSelling = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: false),
                    BanknoteBuying = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: false),
                    BanknoteSelling = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: false),
                    CrossRateUSD = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: true),
                    CrossRateOther = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_ExchangeRateDate_CurrencyCode",
                table: "ExchangeRates",
                columns: new[] { "ExchangeRateDate", "CurrencyCode" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeRates");
        }
    }
}
