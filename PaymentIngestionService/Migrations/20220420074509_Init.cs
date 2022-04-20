using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentIngestionService.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.CreateTable(
                name: "IdempotencyTokens",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Consumer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdempotencyTokens", x => new { x.MessageId, x.Consumer });
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MerchantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryMonth = table.Column<int>(type: "int", nullable: false),
                    ExpiryYear = table.Column<int>(type: "int", nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.Sql(@"
            CREATE PROCEDURE dbo.sp_RetrievePaymentById @Id UNIQUEIDENTIFIER
            AS
             SELECT 
                a.PaymentId,
                p.Currency,
                p.Amount,
                a.Approved,
                a.Status,
                a.ResponseCode,
                a.ResponseSummary,
                a.Type,
                a.Scheme,
                p.ExpiryMonth,
                p.ExpiryYear,
                a.Last4,
                a.Bin,
                a.CardType,
                a.Issuer,
                a.IssuerCountry,
                p.CreatedOn as RequestedOn,
                a.CreatedOn as ProcessedOn
                FROM dbo.Payments p
                JOIN dbo.Authorizations a
                ON p.Id = a.PaymentId
                WHERE p.Id = @Id
            GO
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdempotencyTokens");

            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
