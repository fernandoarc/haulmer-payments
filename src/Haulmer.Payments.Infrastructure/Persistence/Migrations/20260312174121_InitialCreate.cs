using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haulmer.Payments.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "payments");

            migrationBuilder.CreateTable(
                name: "Acquirers",
                schema: "payments",
                columns: table => new
                {
                    AcquirerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acquirers", x => x.AcquirerId);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                schema: "payments",
                columns: table => new
                {
                    MerchantId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompanyRut = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    BusinessLegalName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HeadOfficeAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.MerchantId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentChannels",
                schema: "payments",
                columns: table => new
                {
                    PaymentChannelId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentChannels", x => x.PaymentChannelId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                schema: "payments",
                columns: table => new
                {
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.PaymentMethodId);
                });

            migrationBuilder.CreateTable(
                name: "MerchantBranches",
                schema: "payments",
                columns: table => new
                {
                    MerchantBranchId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<long>(type: "bigint", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantBranches", x => x.MerchantBranchId);
                    table.ForeignKey(
                        name: "FK_MerchantBranches_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "payments",
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MerchantAcquirerConfigurations",
                schema: "payments",
                columns: table => new
                {
                    MerchantAcquirerConfigurationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentChannelId = table.Column<long>(type: "bigint", nullable: false),
                    AcquirerId = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    MerchantTerminalCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AcquirerMerchantCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TimeoutSeconds = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantAcquirerConfigurations", x => x.MerchantAcquirerConfigurationId);
                    table.ForeignKey(
                        name: "FK_MerchantAcquirerConfigurations_Acquirers_AcquirerId",
                        column: x => x.AcquirerId,
                        principalSchema: "payments",
                        principalTable: "Acquirers",
                        principalColumn: "AcquirerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MerchantAcquirerConfigurations_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "payments",
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MerchantAcquirerConfigurations_PaymentChannels_PaymentChannelId",
                        column: x => x.PaymentChannelId,
                        principalSchema: "payments",
                        principalTable: "PaymentChannels",
                        principalColumn: "PaymentChannelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MerchantAcquirerConfigurations_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "payments",
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MerchantPaymentMethods",
                schema: "payments",
                columns: table => new
                {
                    MerchantPaymentMethodId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EnabledFromUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnabledFromLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnabledToUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnabledToLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantPaymentMethods", x => x.MerchantPaymentMethodId);
                    table.ForeignKey(
                        name: "FK_MerchantPaymentMethods_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "payments",
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MerchantPaymentMethods_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "payments",
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MerchantPricings",
                schema: "payments",
                columns: table => new
                {
                    MerchantPricingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentChannelId = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    FixedFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VariableFeePercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    VatPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TaxPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OtherChargePercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ValidFromUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidFromLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidToUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidToLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantPricings", x => x.MerchantPricingId);
                    table.ForeignKey(
                        name: "FK_MerchantPricings_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "payments",
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MerchantPricings_PaymentChannels_PaymentChannelId",
                        column: x => x.PaymentChannelId,
                        principalSchema: "payments",
                        principalTable: "PaymentChannels",
                        principalColumn: "PaymentChannelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MerchantPricings_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "payments",
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                schema: "payments",
                columns: table => new
                {
                    PaymentTransactionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MerchantId = table.Column<long>(type: "bigint", nullable: false),
                    MerchantBranchId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentChannelId = table.Column<long>(type: "bigint", nullable: false),
                    MerchantPaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    MerchantAcquirerConfigurationId = table.Column<long>(type: "bigint", nullable: false),
                    AcquirerId = table.Column<long>(type: "bigint", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    BaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherChargesAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayerFullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PayerRut = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    IssuingBankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CardBrand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CardLast4 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    MaskedPan = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CurrentStatus = table.Column<int>(type: "int", nullable: false),
                    AcquirerReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.PaymentTransactionId);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Acquirers_AcquirerId",
                        column: x => x.AcquirerId,
                        principalSchema: "payments",
                        principalTable: "Acquirers",
                        principalColumn: "AcquirerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_MerchantAcquirerConfigurations_MerchantAcquirerConfigurationId",
                        column: x => x.MerchantAcquirerConfigurationId,
                        principalSchema: "payments",
                        principalTable: "MerchantAcquirerConfigurations",
                        principalColumn: "MerchantAcquirerConfigurationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_MerchantBranches_MerchantBranchId",
                        column: x => x.MerchantBranchId,
                        principalSchema: "payments",
                        principalTable: "MerchantBranches",
                        principalColumn: "MerchantBranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_MerchantPaymentMethods_MerchantPaymentMethodId",
                        column: x => x.MerchantPaymentMethodId,
                        principalSchema: "payments",
                        principalTable: "MerchantPaymentMethods",
                        principalColumn: "MerchantPaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "payments",
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_PaymentChannels_PaymentChannelId",
                        column: x => x.PaymentChannelId,
                        principalSchema: "payments",
                        principalTable: "PaymentChannels",
                        principalColumn: "PaymentChannelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "payments",
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentIdempotencies",
                schema: "payments",
                columns: table => new
                {
                    PaymentIdempotencyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<long>(type: "bigint", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PaymentTransactionId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentIdempotencies", x => x.PaymentIdempotencyId);
                    table.ForeignKey(
                        name: "FK_PaymentIdempotencies_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "payments",
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentIdempotencies_PaymentTransactions_PaymentTransactionId",
                        column: x => x.PaymentTransactionId,
                        principalSchema: "payments",
                        principalTable: "PaymentTransactions",
                        principalColumn: "PaymentTransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRequests",
                schema: "payments",
                columns: table => new
                {
                    PaymentRequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTransactionId = table.Column<long>(type: "bigint", nullable: false),
                    MerchantId = table.Column<long>(type: "bigint", nullable: false),
                    RequestPayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequests", x => x.PaymentRequestId);
                    table.ForeignKey(
                        name: "FK_PaymentRequests_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "payments",
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentRequests_PaymentTransactions_PaymentTransactionId",
                        column: x => x.PaymentTransactionId,
                        principalSchema: "payments",
                        principalTable: "PaymentTransactions",
                        principalColumn: "PaymentTransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTraceLogs",
                schema: "payments",
                columns: table => new
                {
                    PaymentTraceLogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTransactionId = table.Column<long>(type: "bigint", nullable: true),
                    MerchantId = table.Column<long>(type: "bigint", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventSource = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EventDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EventDataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTraceLogs", x => x.PaymentTraceLogId);
                    table.ForeignKey(
                        name: "FK_PaymentTraceLogs_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "payments",
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTraceLogs_PaymentTransactions_PaymentTransactionId",
                        column: x => x.PaymentTransactionId,
                        principalSchema: "payments",
                        principalTable: "PaymentTransactions",
                        principalColumn: "PaymentTransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactionStatusHistories",
                schema: "payments",
                columns: table => new
                {
                    PaymentTransactionStatusHistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTransactionId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusReasonCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StatusReasonDetail = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AcquirerResponseCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AcquirerResponseMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtLocal = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactionStatusHistories", x => x.PaymentTransactionStatusHistoryId);
                    table.ForeignKey(
                        name: "FK_PaymentTransactionStatusHistories_PaymentTransactions_PaymentTransactionId",
                        column: x => x.PaymentTransactionId,
                        principalSchema: "payments",
                        principalTable: "PaymentTransactions",
                        principalColumn: "PaymentTransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acquirers_Code",
                schema: "payments",
                table: "Acquirers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Acquirers_Status",
                schema: "payments",
                table: "Acquirers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantAcquirerConfigurations_AcquirerId",
                schema: "payments",
                table: "MerchantAcquirerConfigurations",
                column: "AcquirerId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantAcquirerConfigurations_MerchantId_PaymentMethodId_PaymentChannelId_Currency_Status",
                schema: "payments",
                table: "MerchantAcquirerConfigurations",
                columns: new[] { "MerchantId", "PaymentMethodId", "PaymentChannelId", "Currency", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantAcquirerConfigurations_PaymentChannelId",
                schema: "payments",
                table: "MerchantAcquirerConfigurations",
                column: "PaymentChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantAcquirerConfigurations_PaymentMethodId",
                schema: "payments",
                table: "MerchantAcquirerConfigurations",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantBranches_MerchantId",
                schema: "payments",
                table: "MerchantBranches",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantBranches_MerchantId_BranchCode",
                schema: "payments",
                table: "MerchantBranches",
                columns: new[] { "MerchantId", "BranchCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MerchantBranches_Status",
                schema: "payments",
                table: "MerchantBranches",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantPaymentMethods_MerchantId_PaymentMethodId",
                schema: "payments",
                table: "MerchantPaymentMethods",
                columns: new[] { "MerchantId", "PaymentMethodId" });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantPaymentMethods_PaymentMethodId",
                schema: "payments",
                table: "MerchantPaymentMethods",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantPricings_MerchantId_PaymentMethodId_PaymentChannelId_Currency_Status",
                schema: "payments",
                table: "MerchantPricings",
                columns: new[] { "MerchantId", "PaymentMethodId", "PaymentChannelId", "Currency", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantPricings_PaymentChannelId",
                schema: "payments",
                table: "MerchantPricings",
                column: "PaymentChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantPricings_PaymentMethodId",
                schema: "payments",
                table: "MerchantPricings",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_CompanyRut",
                schema: "payments",
                table: "Merchants",
                column: "CompanyRut",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_MerchantCode",
                schema: "payments",
                table: "Merchants",
                column: "MerchantCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_Status",
                schema: "payments",
                table: "Merchants",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentChannels_Code",
                schema: "payments",
                table: "PaymentChannels",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentChannels_Status",
                schema: "payments",
                table: "PaymentChannels",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIdempotencies_MerchantId_IdempotencyKey",
                schema: "payments",
                table: "PaymentIdempotencies",
                columns: new[] { "MerchantId", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIdempotencies_PaymentTransactionId",
                schema: "payments",
                table: "PaymentIdempotencies",
                column: "PaymentTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIdempotencies_Status",
                schema: "payments",
                table: "PaymentIdempotencies",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Code",
                schema: "payments",
                table: "PaymentMethods",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Status",
                schema: "payments",
                table: "PaymentMethods",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_CorrelationId",
                schema: "payments",
                table: "PaymentRequests",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_MerchantId",
                schema: "payments",
                table: "PaymentRequests",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_PaymentTransactionId",
                schema: "payments",
                table: "PaymentRequests",
                column: "PaymentTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTraceLogs_CorrelationId",
                schema: "payments",
                table: "PaymentTraceLogs",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTraceLogs_CreatedAtUtc",
                schema: "payments",
                table: "PaymentTraceLogs",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTraceLogs_MerchantId",
                schema: "payments",
                table: "PaymentTraceLogs",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTraceLogs_PaymentTransactionId",
                schema: "payments",
                table: "PaymentTraceLogs",
                column: "PaymentTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_AcquirerId",
                schema: "payments",
                table: "PaymentTransactions",
                column: "AcquirerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_CorrelationId",
                schema: "payments",
                table: "PaymentTransactions",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_CurrentStatus",
                schema: "payments",
                table: "PaymentTransactions",
                column: "CurrentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_MerchantAcquirerConfigurationId",
                schema: "payments",
                table: "PaymentTransactions",
                column: "MerchantAcquirerConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_MerchantBranchId",
                schema: "payments",
                table: "PaymentTransactions",
                column: "MerchantBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_MerchantId",
                schema: "payments",
                table: "PaymentTransactions",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_MerchantId_CurrentStatus",
                schema: "payments",
                table: "PaymentTransactions",
                columns: new[] { "MerchantId", "CurrentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_MerchantPaymentMethodId",
                schema: "payments",
                table: "PaymentTransactions",
                column: "MerchantPaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_PaymentChannelId",
                schema: "payments",
                table: "PaymentTransactions",
                column: "PaymentChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_PaymentMethodId",
                schema: "payments",
                table: "PaymentTransactions",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_TransactionNumber",
                schema: "payments",
                table: "PaymentTransactions",
                column: "TransactionNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactionStatusHistories_PaymentTransactionId",
                schema: "payments",
                table: "PaymentTransactionStatusHistories",
                column: "PaymentTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactionStatusHistories_PaymentTransactionId_CreatedAtUtc",
                schema: "payments",
                table: "PaymentTransactionStatusHistories",
                columns: new[] { "PaymentTransactionId", "CreatedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MerchantPricings",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "PaymentIdempotencies",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "PaymentRequests",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "PaymentTraceLogs",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "PaymentTransactionStatusHistories",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "PaymentTransactions",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "MerchantAcquirerConfigurations",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "MerchantBranches",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "MerchantPaymentMethods",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "Acquirers",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "PaymentChannels",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "Merchants",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "PaymentMethods",
                schema: "payments");
        }
    }
}
