SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @MerchantCode NVARCHAR(50) = 'MRC-001';
    DECLARE @BranchCode NVARCHAR(50) = 'BR-001';
    DECLARE @PaymentMethodCode NVARCHAR(50) = 'CARD';
    DECLARE @PaymentChannelCode NVARCHAR(50) = 'ECOMMERCE';
    DECLARE @AcquirerCode NVARCHAR(50) = 'SIM';

    DECLARE @Txn1 NVARCHAR(50) = 'TXN-DEMO-0001';
    DECLARE @Txn2 NVARCHAR(50) = 'TXN-DEMO-0002';
    DECLARE @Txn3 NVARCHAR(50) = 'TXN-DEMO-0003';

    DECLARE @Idempotency1 NVARCHAR(100) = 'IDEMP-DEMO-0001';
    DECLARE @Idempotency2 NVARCHAR(100) = 'IDEMP-DEMO-0002';
    DECLARE @Idempotency3 NVARCHAR(100) = 'IDEMP-DEMO-0003';

    DECLARE @RequestHash1 NVARCHAR(200) = 'HASH-DEMO-0001';
    DECLARE @RequestHash2 NVARCHAR(200) = 'HASH-DEMO-0002';
    DECLARE @RequestHash3 NVARCHAR(200) = 'HASH-DEMO-0003';

    DECLARE @CorrelationId1 UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
    DECLARE @CorrelationId2 UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';
    DECLARE @CorrelationId3 UNIQUEIDENTIFIER = '33333333-3333-3333-3333-333333333333';

    DECLARE @CreatedUtc DATETIME2 = '2024-01-01T12:00:00';
    DECLARE @CreatedLocal DATETIME2 = '2024-01-01T09:00:00';

    -- ============================================================
    -- CLEANUP DEMO DATA
    -- ============================================================

    DELETE FROM [payments].[PaymentTraceLogs]
    WHERE [PaymentTransactionId] IN
    (
        SELECT [PaymentTransactionId]
        FROM [payments].[PaymentTransactions]
        WHERE [TransactionNumber] IN (@Txn1, @Txn2, @Txn3)
    );

    DELETE FROM [payments].[PaymentRequests]
    WHERE [PaymentTransactionId] IN
    (
        SELECT [PaymentTransactionId]
        FROM [payments].[PaymentTransactions]
        WHERE [TransactionNumber] IN (@Txn1, @Txn2, @Txn3)
    );

    DELETE FROM [payments].[PaymentIdempotencies]
    WHERE [IdempotencyKey] IN (@Idempotency1, @Idempotency2, @Idempotency3)
       OR [PaymentTransactionId] IN
       (
           SELECT [PaymentTransactionId]
           FROM [payments].[PaymentTransactions]
           WHERE [TransactionNumber] IN (@Txn1, @Txn2, @Txn3)
       );

    DELETE FROM [payments].[PaymentTransactionStatusHistories]
    WHERE [PaymentTransactionId] IN
    (
        SELECT [PaymentTransactionId]
        FROM [payments].[PaymentTransactions]
        WHERE [TransactionNumber] IN (@Txn1, @Txn2, @Txn3)
    );

    DELETE FROM [payments].[PaymentTransactions]
    WHERE [TransactionNumber] IN (@Txn1, @Txn2, @Txn3);

    DELETE FROM [payments].[MerchantPricings]
    WHERE [MerchantId] IN
    (
        SELECT [MerchantId]
        FROM [payments].[Merchants]
        WHERE [MerchantCode] = @MerchantCode
    );

    DELETE FROM [payments].[MerchantAcquirerConfigurations]
    WHERE [MerchantId] IN
    (
        SELECT [MerchantId]
        FROM [payments].[Merchants]
        WHERE [MerchantCode] = @MerchantCode
    );

    DELETE FROM [payments].[MerchantPaymentMethods]
    WHERE [MerchantId] IN
    (
        SELECT [MerchantId]
        FROM [payments].[Merchants]
        WHERE [MerchantCode] = @MerchantCode
    );

    DELETE FROM [payments].[MerchantBranches]
    WHERE [MerchantId] IN
    (
        SELECT [MerchantId]
        FROM [payments].[Merchants]
        WHERE [MerchantCode] = @MerchantCode
    )
      AND [BranchCode] = @BranchCode;

    DELETE FROM [payments].[Merchants]
    WHERE [MerchantCode] = @MerchantCode;

    DELETE FROM [payments].[PaymentMethods]
    WHERE [Code] = @PaymentMethodCode;

    DELETE FROM [payments].[PaymentChannels]
    WHERE [Code] = @PaymentChannelCode;

    DELETE FROM [payments].[Acquirers]
    WHERE [Code] = @AcquirerCode;

    -- ============================================================
    -- MASTER DATA
    -- ============================================================

    DECLARE @MerchantId BIGINT;
    DECLARE @MerchantBranchId BIGINT;
    DECLARE @PaymentMethodId BIGINT;
    DECLARE @PaymentChannelId BIGINT;
    DECLARE @AcquirerId BIGINT;
    DECLARE @MerchantPaymentMethodId BIGINT;
    DECLARE @MerchantAcquirerConfigurationId BIGINT;
    DECLARE @MerchantPricingId BIGINT;

    INSERT INTO [payments].[Merchants]
    (
        [MerchantCode],
        [CompanyRut],
        [BusinessLegalName],
        [TradeName],
        [HeadOfficeAddress],
        [ContactEmail],
        [ContactPhone],
        [Status],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @MerchantCode,
        '76000001-5',
        'Demo Merchant SpA',
        'Demo Merchant',
        'Av. Principal 123, Santiago',
        'contacto@demo.cl',
        '+56912345678',
        1,
        @CreatedUtc,
        @CreatedLocal
    );

    SET @MerchantId = SCOPE_IDENTITY();

    INSERT INTO [payments].[MerchantBranches]
    (
        [MerchantId],
        [BranchCode],
        [BranchName],
        [Address],
        [City],
        [Region],
        [Country],
        [Status],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @MerchantId,
        @BranchCode,
        'Casa Matriz',
        'Av. Principal 123, Santiago',
        'Santiago',
        'RM',
        'Chile',
        1,
        @CreatedUtc,
        @CreatedLocal
    );

    SET @MerchantBranchId = SCOPE_IDENTITY();

    INSERT INTO [payments].[PaymentMethods]
    (
        [Code],
        [Name],
        [Status],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @PaymentMethodCode,
        'Card',
        1,
        @CreatedUtc,
        @CreatedLocal
    );

    SET @PaymentMethodId = SCOPE_IDENTITY();

    INSERT INTO [payments].[PaymentChannels]
    (
        [Code],
        [Name],
        [Status],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @PaymentChannelCode,
        'Ecommerce',
        1,
        @CreatedUtc,
        @CreatedLocal
    );

    SET @PaymentChannelId = SCOPE_IDENTITY();

    INSERT INTO [payments].[Acquirers]
    (
        [Code],
        [Name],
        [Status],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @AcquirerCode,
        'Simulated Bank',
        1,
        @CreatedUtc,
        @CreatedLocal
    );

    SET @AcquirerId = SCOPE_IDENTITY();

    INSERT INTO [payments].[MerchantPaymentMethods]
    (
        [MerchantId],
        [PaymentMethodId],
        [Status],
        [EnabledFromUtc],
        [EnabledFromLocal],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @MerchantId,
        @PaymentMethodId,
        1,
        @CreatedUtc,
        @CreatedLocal,
        @CreatedUtc,
        @CreatedLocal
    );

    SET @MerchantPaymentMethodId = SCOPE_IDENTITY();

    INSERT INTO [payments].[MerchantAcquirerConfigurations]
    (
        [MerchantId],
        [PaymentMethodId],
        [PaymentChannelId],
        [AcquirerId],
        [Currency],
        [MerchantTerminalCode],
        [AcquirerMerchantCode],
        [TimeoutSeconds],
        [Priority],
        [Status],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @MerchantId,
        @PaymentMethodId,
        @PaymentChannelId,
        @AcquirerId,
        'CLP',
        'TERM-001',
        'ACQ-001',
        30,
        1,
        1,
        @CreatedUtc,
        @CreatedLocal
    );

    SET @MerchantAcquirerConfigurationId = SCOPE_IDENTITY();

    INSERT INTO [payments].[MerchantPricings]
    (
        [MerchantId],
        [PaymentMethodId],
        [PaymentChannelId],
        [Currency],
        [FixedFeeAmount],
        [VariableFeePercentage],
        [VatPercentage],
        [TaxPercentage],
        [OtherChargePercentage],
        [ValidFromUtc],
        [ValidFromLocal],
        [Status],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @MerchantId,
        @PaymentMethodId,
        @PaymentChannelId,
        'CLP',
        150.00,
        2.5000,
        19.0000,
        0.0000,
        0.0000,
        @CreatedUtc,
        @CreatedLocal,
        1,
        @CreatedUtc,
        @CreatedLocal
    );

    SET @MerchantPricingId = SCOPE_IDENTITY();

    -- ============================================================
    -- SAMPLE PAYMENT 1 - APPROVED
    -- ============================================================

    DECLARE @PaymentTransactionId1 BIGINT;

    INSERT INTO [payments].[PaymentTransactions]
    (
        [TransactionNumber],
        [MerchantId],
        [MerchantBranchId],
        [PaymentMethodId],
        [PaymentChannelId],
        [MerchantPaymentMethodId],
        [MerchantAcquirerConfigurationId],
        [AcquirerId],
        [IdempotencyKey],
        [Currency],
        [BaseAmount],
        [TipAmount],
        [GrossAmount],
        [FeeAmount],
        [VatAmount],
        [TaxAmount],
        [OtherChargesAmount],
        [NetAmount],
        [PayerFullName],
        [PayerRut],
        [IssuingBankName],
        [CardBrand],
        [CardLast4],
        [MaskedPan],
        [CurrentStatus],
        [AcquirerReference],
        [CorrelationId],
        [CreatedAtUtc],
        [CreatedAtLocal],
        [ProcessedAtUtc],
        [ProcessedAtLocal]
    )
    VALUES
    (
        @Txn1,
        @MerchantId,
        @MerchantBranchId,
        @PaymentMethodId,
        @PaymentChannelId,
        @MerchantPaymentMethodId,
        @MerchantAcquirerConfigurationId,
        @AcquirerId,
        @Idempotency1,
        'CLP',
        10000.00,
        0.00,
        10000.00,
        400.00,
        76.00,
        0.00,
        0.00,
        9524.00,
        N'Juan Pérez',
        '11111111-1',
        'Banco Simulado',
        'VISA',
        '1234',
        '****-****-****-1234',
        3,
        'SIM-TXN-DEMO-0001',
        @CorrelationId1,
        '2024-01-02T12:00:00',
        '2024-01-02T09:00:00',
        '2024-01-02T12:02:00',
        '2024-01-02T09:02:00'
    );

    SET @PaymentTransactionId1 = SCOPE_IDENTITY();

    INSERT INTO [payments].[PaymentTransactionStatusHistories]
    (
        [PaymentTransactionId],
        [Status],
        [StatusReasonCode],
        [StatusReasonDetail],
        [AcquirerResponseCode],
        [AcquirerResponseMessage],
        [CorrelationId],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (@PaymentTransactionId1, 1, '', 'Payment created.', '', '', @CorrelationId1, '2024-01-02T12:00:00', '2024-01-02T09:00:00'),
    (@PaymentTransactionId1, 2, '', 'Sending transaction to bank authorization service.', '', '', @CorrelationId1, '2024-01-02T12:01:00', '2024-01-02T09:01:00'),
    (@PaymentTransactionId1, 3, '', 'Payment approved by bank authorization service.', '00', 'Approved', @CorrelationId1, '2024-01-02T12:02:00', '2024-01-02T09:02:00');

    INSERT INTO [payments].[PaymentIdempotencies]
    (
        [MerchantId],
        [IdempotencyKey],
        [RequestHash],
        [PaymentTransactionId],
        [Status],
        [CreatedAtUtc],
        [CreatedAtLocal],
        [UpdatedAtUtc],
        [UpdatedAtLocal]
    )
    VALUES
    (
        @MerchantId,
        @Idempotency1,
        @RequestHash1,
        @PaymentTransactionId1,
        2,
        '2024-01-02T12:00:00',
        '2024-01-02T09:00:00',
        '2024-01-02T12:02:00',
        '2024-01-02T09:02:00'
    );

    INSERT INTO [payments].[PaymentRequests]
    (
        [PaymentTransactionId],
        [MerchantId],
        [RequestPayloadJson],
        [RequestHash],
        [CorrelationId],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @PaymentTransactionId1,
        @MerchantId,
        '{"orderId":"ORDER-DEMO-0001","amount":10000,"currency":"CLP"}',
        @RequestHash1,
        @CorrelationId1,
        '2024-01-02T12:00:00',
        '2024-01-02T09:00:00'
    );

    INSERT INTO [payments].[PaymentTraceLogs]
    (
        [PaymentTransactionId],
        [MerchantId],
        [CorrelationId],
        [EventType],
        [EventSource],
        [EventDescription],
        [EventDataJson],
        [Severity],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (@PaymentTransactionId1, @MerchantId, @CorrelationId1, 'PaymentCreated', 'SeedScript', 'Payment processing initiated.', '{"transactionNumber":"TXN-DEMO-0001","status":"Pending"}', 1, '2024-01-02T12:00:00', '2024-01-02T09:00:00'),
    (@PaymentTransactionId1, @MerchantId, @CorrelationId1, 'PaymentApproved', 'SeedScript', 'Payment approved by simulated acquirer.', '{"acquirerReference":"SIM-TXN-DEMO-0001","responseCode":"00","responseMessage":"Approved"}', 1, '2024-01-02T12:02:00', '2024-01-02T09:02:00');

    -- ============================================================
    -- SAMPLE PAYMENT 2 - DECLINED
    -- ============================================================

    DECLARE @PaymentTransactionId2 BIGINT;

    INSERT INTO [payments].[PaymentTransactions]
    (
        [TransactionNumber],
        [MerchantId],
        [MerchantBranchId],
        [PaymentMethodId],
        [PaymentChannelId],
        [MerchantPaymentMethodId],
        [MerchantAcquirerConfigurationId],
        [AcquirerId],
        [IdempotencyKey],
        [Currency],
        [BaseAmount],
        [TipAmount],
        [GrossAmount],
        [FeeAmount],
        [VatAmount],
        [TaxAmount],
        [OtherChargesAmount],
        [NetAmount],
        [PayerFullName],
        [PayerRut],
        [IssuingBankName],
        [CardBrand],
        [CardLast4],
        [MaskedPan],
        [CurrentStatus],
        [AcquirerReference],
        [CorrelationId],
        [CreatedAtUtc],
        [CreatedAtLocal],
        [ProcessedAtUtc],
        [ProcessedAtLocal]
    )
    VALUES
    (
        @Txn2,
        @MerchantId,
        @MerchantBranchId,
        @PaymentMethodId,
        @PaymentChannelId,
        @MerchantPaymentMethodId,
        @MerchantAcquirerConfigurationId,
        @AcquirerId,
        @Idempotency2,
        'CLP',
        2000000.00,
        0.00,
        2000000.00,
        50150.00,
        9528.50,
        0.00,
        0.00,
        1940321.50,
        N'María López',
        '22222222-2',
        'Banco Simulado',
        'MASTERCARD',
        '5678',
        '****-****-****-5678',
        4,
        'SIM-TXN-DEMO-0002',
        @CorrelationId2,
        '2024-01-03T12:00:00',
        '2024-01-03T09:00:00',
        '2024-01-03T12:02:00',
        '2024-01-03T09:02:00'
    );

    SET @PaymentTransactionId2 = SCOPE_IDENTITY();

    INSERT INTO [payments].[PaymentTransactionStatusHistories]
    (
        [PaymentTransactionId],
        [Status],
        [StatusReasonCode],
        [StatusReasonDetail],
        [AcquirerResponseCode],
        [AcquirerResponseMessage],
        [CorrelationId],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (@PaymentTransactionId2, 1, '', 'Payment created.', '', '', @CorrelationId2, '2024-01-03T12:00:00', '2024-01-03T09:00:00'),
    (@PaymentTransactionId2, 2, '', 'Sending transaction to bank authorization service.', '', '', @CorrelationId2, '2024-01-03T12:01:00', '2024-01-03T09:01:00'),
    (@PaymentTransactionId2, 4, '', 'Payment declined by bank authorization service.', '05', 'Declined', @CorrelationId2, '2024-01-03T12:02:00', '2024-01-03T09:02:00');

    INSERT INTO [payments].[PaymentIdempotencies]
    (
        [MerchantId],
        [IdempotencyKey],
        [RequestHash],
        [PaymentTransactionId],
        [Status],
        [CreatedAtUtc],
        [CreatedAtLocal],
        [UpdatedAtUtc],
        [UpdatedAtLocal]
    )
    VALUES
    (
        @MerchantId,
        @Idempotency2,
        @RequestHash2,
        @PaymentTransactionId2,
        2,
        '2024-01-03T12:00:00',
        '2024-01-03T09:00:00',
        '2024-01-03T12:02:00',
        '2024-01-03T09:02:00'
    );

    INSERT INTO [payments].[PaymentRequests]
    (
        [PaymentTransactionId],
        [MerchantId],
        [RequestPayloadJson],
        [RequestHash],
        [CorrelationId],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @PaymentTransactionId2,
        @MerchantId,
        '{"orderId":"ORDER-DEMO-0002","amount":2000000,"currency":"CLP"}',
        @RequestHash2,
        @CorrelationId2,
        '2024-01-03T12:00:00',
        '2024-01-03T09:00:00'
    );

    INSERT INTO [payments].[PaymentTraceLogs]
    (
        [PaymentTransactionId],
        [MerchantId],
        [CorrelationId],
        [EventType],
        [EventSource],
        [EventDescription],
        [EventDataJson],
        [Severity],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (@PaymentTransactionId2, @MerchantId, @CorrelationId2, 'PaymentCreated', 'SeedScript', 'Payment processing initiated.', '{"transactionNumber":"TXN-DEMO-0002","status":"Pending"}', 1, '2024-01-03T12:00:00', '2024-01-03T09:00:00'),
    (@PaymentTransactionId2, @MerchantId, @CorrelationId2, 'PaymentDeclined', 'SeedScript', 'Payment declined by simulated acquirer.', '{"acquirerReference":"SIM-TXN-DEMO-0002","responseCode":"05","responseMessage":"Declined"}', 2, '2024-01-03T12:02:00', '2024-01-03T09:02:00');

    -- ============================================================
    -- SAMPLE PAYMENT 3 - FAILED
    -- ============================================================

    DECLARE @PaymentTransactionId3 BIGINT;

    INSERT INTO [payments].[PaymentTransactions]
    (
        [TransactionNumber],
        [MerchantId],
        [MerchantBranchId],
        [PaymentMethodId],
        [PaymentChannelId],
        [MerchantPaymentMethodId],
        [MerchantAcquirerConfigurationId],
        [AcquirerId],
        [IdempotencyKey],
        [Currency],
        [BaseAmount],
        [TipAmount],
        [GrossAmount],
        [FeeAmount],
        [VatAmount],
        [TaxAmount],
        [OtherChargesAmount],
        [NetAmount],
        [PayerFullName],
        [PayerRut],
        [IssuingBankName],
        [CardBrand],
        [CardLast4],
        [MaskedPan],
        [CurrentStatus],
        [AcquirerReference],
        [CorrelationId],
        [CreatedAtUtc],
        [CreatedAtLocal],
        [ProcessedAtUtc],
        [ProcessedAtLocal]
    )
    VALUES
    (
        @Txn3,
        @MerchantId,
        @MerchantBranchId,
        @PaymentMethodId,
        @PaymentChannelId,
        @MerchantPaymentMethodId,
        @MerchantAcquirerConfigurationId,
        @AcquirerId,
        @Idempotency3,
        'CLP',
        50000.00,
        0.00,
        50000.00,
        1400.00,
        266.00,
        0.00,
        0.00,
        48334.00,
        N'Pedro González',
        '33333333-3',
        'Banco Simulado',
        'AMEX',
        '9012',
        '****-****-****-9012',
        5,
        '',
        @CorrelationId3,
        '2024-01-04T12:00:00',
        '2024-01-04T09:00:00',
        '2024-01-04T12:02:00',
        '2024-01-04T09:02:00'
    );

    SET @PaymentTransactionId3 = SCOPE_IDENTITY();

    INSERT INTO [payments].[PaymentTransactionStatusHistories]
    (
        [PaymentTransactionId],
        [Status],
        [StatusReasonCode],
        [StatusReasonDetail],
        [AcquirerResponseCode],
        [AcquirerResponseMessage],
        [CorrelationId],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (@PaymentTransactionId3, 1, '', 'Payment created.', '', '', @CorrelationId3, '2024-01-04T12:00:00', '2024-01-04T09:00:00'),
    (@PaymentTransactionId3, 2, '', 'Sending transaction to bank authorization service.', '', '', @CorrelationId3, '2024-01-04T12:01:00', '2024-01-04T09:01:00'),
    (@PaymentTransactionId3, 5, 'AUTHORIZATION_ERROR', 'Bank authorization failed.', '', '', @CorrelationId3, '2024-01-04T12:02:00', '2024-01-04T09:02:00');

    INSERT INTO [payments].[PaymentIdempotencies]
    (
        [MerchantId],
        [IdempotencyKey],
        [RequestHash],
        [PaymentTransactionId],
        [Status],
        [CreatedAtUtc],
        [CreatedAtLocal],
        [UpdatedAtUtc],
        [UpdatedAtLocal]
    )
    VALUES
    (
        @MerchantId,
        @Idempotency3,
        @RequestHash3,
        @PaymentTransactionId3,
        4,
        '2024-01-04T12:00:00',
        '2024-01-04T09:00:00',
        '2024-01-04T12:02:00',
        '2024-01-04T09:02:00'
    );

    INSERT INTO [payments].[PaymentRequests]
    (
        [PaymentTransactionId],
        [MerchantId],
        [RequestPayloadJson],
        [RequestHash],
        [CorrelationId],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (
        @PaymentTransactionId3,
        @MerchantId,
        '{"orderId":"ORDER-DEMO-0003","amount":50000,"currency":"CLP"}',
        @RequestHash3,
        @CorrelationId3,
        '2024-01-04T12:00:00',
        '2024-01-04T09:00:00'
    );

    INSERT INTO [payments].[PaymentTraceLogs]
    (
        [PaymentTransactionId],
        [MerchantId],
        [CorrelationId],
        [EventType],
        [EventSource],
        [EventDescription],
        [EventDataJson],
        [Severity],
        [CreatedAtUtc],
        [CreatedAtLocal]
    )
    VALUES
    (@PaymentTransactionId3, @MerchantId, @CorrelationId3, 'PaymentCreated', 'SeedScript', 'Payment processing initiated.', '{"transactionNumber":"TXN-DEMO-0003","status":"Pending"}', 1, '2024-01-04T12:00:00', '2024-01-04T09:00:00'),
    (@PaymentTransactionId3, @MerchantId, @CorrelationId3, 'PaymentAuthorizationFailed', 'SeedScript', 'Technical failure during authorization.', '{"statusReasonCode":"AUTHORIZATION_ERROR"}', 3, '2024-01-04T12:02:00', '2024-01-04T09:02:00');

    COMMIT TRANSACTION;

    PRINT 'Seed ejecutado correctamente.';
    PRINT 'Merchant: MRC-001';
    PRINT 'Branch: BR-001';
    PRINT 'PaymentMethod: CARD';
    PRINT 'PaymentChannel: ECOMMERCE';
    PRINT 'Acquirer: SIM';
    PRINT 'Transacciones demo: TXN-DEMO-0001, TXN-DEMO-0002, TXN-DEMO-0003';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;