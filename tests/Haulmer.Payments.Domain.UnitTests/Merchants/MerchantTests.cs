using Haulmer.Payments.Domain.Merchants;

namespace Haulmer.Payments.Domain.UnitTests.Merchants;

public class MerchantTests
{
    private const string ValidMerchantCode = "MERCH001";
    private const string ValidCompanyRut = "76.123.456-7";
    private const string ValidBusinessLegalName = "Test Company Ltda.";
    private const string ValidTradeName = "Test Trade Name";
    private const string ValidHeadOfficeAddress = "Av. Providencia 123, Santiago, Chile";
    private const string ValidContactEmail = "contact@testcompany.cl";
    private const string ValidContactPhone = "+56 9 1234 5678";

    private static DateTime ValidCreatedAtUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidCreatedAtLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);
    private static DateTime ValidUpdatedAtUtc => new DateTime(2023, 1, 2, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidUpdatedAtLocal => new DateTime(2023, 1, 2, 9, 0, 0, DateTimeKind.Local);

    [Fact]
    public void Create_Should_SetInitialValues_When_InputIsValid()
    {
        // Arrange
        var merchantCode = ValidMerchantCode;
        var companyRut = ValidCompanyRut;
        var businessLegalName = ValidBusinessLegalName;
        var tradeName = ValidTradeName;
        var headOfficeAddress = ValidHeadOfficeAddress;
        var contactEmail = ValidContactEmail;
        var contactPhone = ValidContactPhone;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var merchant = Merchant.Create(
            merchantCode,
            companyRut,
            businessLegalName,
            tradeName,
            headOfficeAddress,
            contactEmail,
            contactPhone,
            createdAtUtc,
            createdAtLocal);

        // Assert
        merchant.MerchantId.Should().Be(0); // Default for new entity
        merchant.MerchantCode.Should().Be(merchantCode);
        merchant.CompanyRut.Should().Be(companyRut);
        merchant.BusinessLegalName.Should().Be(businessLegalName);
        merchant.TradeName.Should().Be(tradeName);
        merchant.HeadOfficeAddress.Should().Be(headOfficeAddress);
        merchant.ContactEmail.Should().Be(contactEmail);
        merchant.ContactPhone.Should().Be(contactPhone);
        merchant.Status.Should().Be(MerchantStatus.Active);
        merchant.CreatedAtUtc.Should().Be(createdAtUtc);
        merchant.CreatedAtLocal.Should().Be(createdAtLocal);
        merchant.UpdatedAtUtc.Should().BeNull();
        merchant.UpdatedAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantCodeIsEmpty()
    {
        // Arrange
        var merchantCode = string.Empty;

        // Act
        var act = () => Merchant.Create(
            merchantCode,
            ValidCompanyRut,
            ValidBusinessLegalName,
            ValidTradeName,
            ValidHeadOfficeAddress,
            ValidContactEmail,
            ValidContactPhone,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("merchantCode is required. (Parameter 'merchantCode')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CompanyRutIsEmpty()
    {
        // Arrange
        var companyRut = string.Empty;

        // Act
        var act = () => Merchant.Create(
            ValidMerchantCode,
            companyRut,
            ValidBusinessLegalName,
            ValidTradeName,
            ValidHeadOfficeAddress,
            ValidContactEmail,
            ValidContactPhone,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("companyRut is required. (Parameter 'companyRut')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_BusinessLegalNameIsEmpty()
    {
        // Arrange
        var businessLegalName = string.Empty;

        // Act
        var act = () => Merchant.Create(
            ValidMerchantCode,
            ValidCompanyRut,
            businessLegalName,
            ValidTradeName,
            ValidHeadOfficeAddress,
            ValidContactEmail,
            ValidContactPhone,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("businessLegalName is required. (Parameter 'businessLegalName')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_TradeNameIsEmpty()
    {
        // Arrange
        var tradeName = string.Empty;

        // Act
        var act = () => Merchant.Create(
            ValidMerchantCode,
            ValidCompanyRut,
            ValidBusinessLegalName,
            tradeName,
            ValidHeadOfficeAddress,
            ValidContactEmail,
            ValidContactPhone,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("tradeName is required. (Parameter 'tradeName')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_HeadOfficeAddressIsEmpty()
    {
        // Arrange
        var headOfficeAddress = string.Empty;

        // Act
        var act = () => Merchant.Create(
            ValidMerchantCode,
            ValidCompanyRut,
            ValidBusinessLegalName,
            ValidTradeName,
            headOfficeAddress,
            ValidContactEmail,
            ValidContactPhone,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("headOfficeAddress is required. (Parameter 'headOfficeAddress')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_ContactEmailIsEmpty()
    {
        // Arrange
        var contactEmail = string.Empty;

        // Act
        var act = () => Merchant.Create(
            ValidMerchantCode,
            ValidCompanyRut,
            ValidBusinessLegalName,
            ValidTradeName,
            ValidHeadOfficeAddress,
            contactEmail,
            ValidContactPhone,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("contactEmail is required. (Parameter 'contactEmail')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_ContactPhoneIsEmpty()
    {
        // Arrange
        var contactPhone = string.Empty;

        // Act
        var act = () => Merchant.Create(
            ValidMerchantCode,
            ValidCompanyRut,
            ValidBusinessLegalName,
            ValidTradeName,
            ValidHeadOfficeAddress,
            ValidContactEmail,
            contactPhone,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("contactPhone is required. (Parameter 'contactPhone')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtUtcIsDefault()
    {
        // Arrange
        var createdAtUtc = default(DateTime);

        // Act
        var act = () => Merchant.Create(
            ValidMerchantCode,
            ValidCompanyRut,
            ValidBusinessLegalName,
            ValidTradeName,
            ValidHeadOfficeAddress,
            ValidContactEmail,
            ValidContactPhone,
            createdAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtUtc is required. (Parameter 'createdAtUtc')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtLocalIsDefault()
    {
        // Arrange
        var createdAtLocal = default(DateTime);

        // Act
        var act = () => Merchant.Create(
            ValidMerchantCode,
            ValidCompanyRut,
            ValidBusinessLegalName,
            ValidTradeName,
            ValidHeadOfficeAddress,
            ValidContactEmail,
            ValidContactPhone,
            ValidCreatedAtUtc,
            createdAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtLocal is required. (Parameter 'createdAtLocal')");
    }

    [Fact]
    public void UpdateContactInformation_Should_UpdateFieldsAndTimestamps_When_InputIsValid()
    {
        // Arrange
        var merchant = CreateValidMerchant();
        var newHeadOfficeAddress = "New Address 456, Santiago, Chile";
        var newContactEmail = "newcontact@testcompany.cl";
        var newContactPhone = "+56 9 8765 4321";
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchant.UpdateContactInformation(
            newHeadOfficeAddress,
            newContactEmail,
            newContactPhone,
            updatedAtUtc,
            updatedAtLocal);

        // Assert
        merchant.HeadOfficeAddress.Should().Be(newHeadOfficeAddress);
        merchant.ContactEmail.Should().Be(newContactEmail);
        merchant.ContactPhone.Should().Be(newContactPhone);
        merchant.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchant.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void UpdateContactInformation_Should_ThrowArgumentException_When_HeadOfficeAddressIsEmpty()
    {
        // Arrange
        var merchant = CreateValidMerchant();
        var headOfficeAddress = string.Empty;

        // Act
        var act = () => merchant.UpdateContactInformation(
            headOfficeAddress,
            ValidContactEmail,
            ValidContactPhone,
            ValidUpdatedAtUtc,
            ValidUpdatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("headOfficeAddress is required. (Parameter 'headOfficeAddress')");
    }

    [Fact]
    public void UpdateContactInformation_Should_ThrowArgumentException_When_ContactEmailIsEmpty()
    {
        // Arrange
        var merchant = CreateValidMerchant();
        var contactEmail = string.Empty;

        // Act
        var act = () => merchant.UpdateContactInformation(
            ValidHeadOfficeAddress,
            contactEmail,
            ValidContactPhone,
            ValidUpdatedAtUtc,
            ValidUpdatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("contactEmail is required. (Parameter 'contactEmail')");
    }

    [Fact]
    public void UpdateContactInformation_Should_ThrowArgumentException_When_ContactPhoneIsEmpty()
    {
        // Arrange
        var merchant = CreateValidMerchant();
        var contactPhone = string.Empty;

        // Act
        var act = () => merchant.UpdateContactInformation(
            ValidHeadOfficeAddress,
            ValidContactEmail,
            contactPhone,
            ValidUpdatedAtUtc,
            ValidUpdatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("contactPhone is required. (Parameter 'contactPhone')");
    }

    [Fact]
    public void Activate_Should_SetStatusToActive_When_CurrentStatusIsNotActive()
    {
        // Arrange
        var merchant = CreateValidMerchant();
        merchant.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchant.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchant.Status.Should().Be(MerchantStatus.Active);
        merchant.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchant.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_SetStatusToInactive_When_CurrentStatusIsNotInactive()
    {
        // Arrange
        var merchant = CreateValidMerchant(); // Starts as Active
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchant.Deactivate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchant.Status.Should().Be(MerchantStatus.Inactive);
        merchant.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchant.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Suspend_Should_SetStatusToSuspended_When_CurrentStatusIsNotSuspended()
    {
        // Arrange
        var merchant = CreateValidMerchant(); // Starts as Active
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchant.Suspend(updatedAtUtc, updatedAtLocal);

        // Assert
        merchant.Status.Should().Be(MerchantStatus.Suspended);
        merchant.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchant.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Activate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyActive()
    {
        // Arrange
        var merchant = CreateValidMerchant(); // Starts as Active
        var initialUpdatedAtUtc = merchant.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchant.UpdatedAtLocal;
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchant.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchant.Status.Should().Be(MerchantStatus.Active);
        merchant.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchant.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyInactive()
    {
        // Arrange
        var merchant = CreateValidMerchant();
        merchant.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var initialUpdatedAtUtc = merchant.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchant.UpdatedAtLocal;
        var newUpdatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var newUpdatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchant.Deactivate(newUpdatedAtUtc, newUpdatedAtLocal);

        // Assert
        merchant.Status.Should().Be(MerchantStatus.Inactive);
        merchant.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchant.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    [Fact]
    public void Suspend_Should_NotChangeUpdatedDates_When_StatusIsAlreadySuspended()
    {
        // Arrange
        var merchant = CreateValidMerchant();
        merchant.Suspend(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Suspended
        var initialUpdatedAtUtc = merchant.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchant.UpdatedAtLocal;
        var newUpdatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var newUpdatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchant.Suspend(newUpdatedAtUtc, newUpdatedAtLocal);

        // Assert
        merchant.Status.Should().Be(MerchantStatus.Suspended);
        merchant.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchant.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    private static Merchant CreateValidMerchant()
    {
        return Merchant.Create(
            ValidMerchantCode,
            ValidCompanyRut,
            ValidBusinessLegalName,
            ValidTradeName,
            ValidHeadOfficeAddress,
            ValidContactEmail,
            ValidContactPhone,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);
    }
}