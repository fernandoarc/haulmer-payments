using Haulmer.Payments.Domain.Merchants;

namespace Haulmer.Payments.Domain.UnitTests.Merchants;

public class MerchantBranchTests
{
    private const long ValidMerchantId = 1;
    private const string ValidBranchCode = "BR001";
    private const string ValidBranchName = "Main Branch";
    private const string ValidAddress = "Av. Providencia 123, Santiago, Chile";
    private const string ValidCity = "Santiago";
    private const string ValidRegion = "Metropolitana";
    private const string ValidCountry = "Chile";

    private static DateTime ValidCreatedAtUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidCreatedAtLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);
    private static DateTime ValidUpdatedAtUtc => new DateTime(2023, 1, 2, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidUpdatedAtLocal => new DateTime(2023, 1, 2, 9, 0, 0, DateTimeKind.Local);

    [Fact]
    public void Create_Should_SetInitialValues_When_InputIsValid()
    {
        // Arrange
        var merchantId = ValidMerchantId;
        var branchCode = ValidBranchCode;
        var branchName = ValidBranchName;
        var address = ValidAddress;
        var city = ValidCity;
        var region = ValidRegion;
        var country = ValidCountry;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var merchantBranch = MerchantBranch.Create(
            merchantId,
            branchCode,
            branchName,
            address,
            city,
            region,
            country,
            createdAtUtc,
            createdAtLocal);

        // Assert
        merchantBranch.MerchantBranchId.Should().Be(0); // Default for new entity
        merchantBranch.MerchantId.Should().Be(merchantId);
        merchantBranch.BranchCode.Should().Be(branchCode);
        merchantBranch.BranchName.Should().Be(branchName);
        merchantBranch.Address.Should().Be(address);
        merchantBranch.City.Should().Be(city);
        merchantBranch.Region.Should().Be(region);
        merchantBranch.Country.Should().Be(country);
        merchantBranch.Status.Should().Be(MerchantBranchStatus.Active);
        merchantBranch.CreatedAtUtc.Should().Be(createdAtUtc);
        merchantBranch.CreatedAtLocal.Should().Be(createdAtLocal);
        merchantBranch.UpdatedAtUtc.Should().BeNull();
        merchantBranch.UpdatedAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantIdIsInvalid()
    {
        // Arrange
        var merchantId = 0L;

        // Act
        var act = () => MerchantBranch.Create(
            merchantId,
            ValidBranchCode,
            ValidBranchName,
            ValidAddress,
            ValidCity,
            ValidRegion,
            ValidCountry,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("MerchantId must be greater than zero. (Parameter 'merchantId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_BranchCodeIsEmpty()
    {
        // Arrange
        var branchCode = string.Empty;

        // Act
        var act = () => MerchantBranch.Create(
            ValidMerchantId,
            branchCode,
            ValidBranchName,
            ValidAddress,
            ValidCity,
            ValidRegion,
            ValidCountry,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("branchCode is required. (Parameter 'branchCode')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_BranchNameIsEmpty()
    {
        // Arrange
        var branchName = string.Empty;

        // Act
        var act = () => MerchantBranch.Create(
            ValidMerchantId,
            ValidBranchCode,
            branchName,
            ValidAddress,
            ValidCity,
            ValidRegion,
            ValidCountry,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("branchName is required. (Parameter 'branchName')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_AddressIsEmpty()
    {
        // Arrange
        var address = string.Empty;

        // Act
        var act = () => MerchantBranch.Create(
            ValidMerchantId,
            ValidBranchCode,
            ValidBranchName,
            address,
            ValidCity,
            ValidRegion,
            ValidCountry,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("address is required. (Parameter 'address')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CityIsEmpty()
    {
        // Arrange
        var city = string.Empty;

        // Act
        var act = () => MerchantBranch.Create(
            ValidMerchantId,
            ValidBranchCode,
            ValidBranchName,
            ValidAddress,
            city,
            ValidRegion,
            ValidCountry,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("city is required. (Parameter 'city')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_RegionIsEmpty()
    {
        // Arrange
        var region = string.Empty;

        // Act
        var act = () => MerchantBranch.Create(
            ValidMerchantId,
            ValidBranchCode,
            ValidBranchName,
            ValidAddress,
            ValidCity,
            region,
            ValidCountry,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("region is required. (Parameter 'region')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CountryIsEmpty()
    {
        // Arrange
        var country = string.Empty;

        // Act
        var act = () => MerchantBranch.Create(
            ValidMerchantId,
            ValidBranchCode,
            ValidBranchName,
            ValidAddress,
            ValidCity,
            ValidRegion,
            country,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("country is required. (Parameter 'country')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtUtcIsDefault()
    {
        // Arrange
        var createdAtUtc = default(DateTime);

        // Act
        var act = () => MerchantBranch.Create(
            ValidMerchantId,
            ValidBranchCode,
            ValidBranchName,
            ValidAddress,
            ValidCity,
            ValidRegion,
            ValidCountry,
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
        var act = () => MerchantBranch.Create(
            ValidMerchantId,
            ValidBranchCode,
            ValidBranchName,
            ValidAddress,
            ValidCity,
            ValidRegion,
            ValidCountry,
            ValidCreatedAtUtc,
            createdAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtLocal is required. (Parameter 'createdAtLocal')");
    }

    [Fact]
    public void Activate_Should_SetStatusToActive_When_CurrentStatusIsNotActive()
    {
        // Arrange
        var merchantBranch = CreateValidMerchantBranch();
        merchantBranch.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchantBranch.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantBranch.Status.Should().Be(MerchantBranchStatus.Active);
        merchantBranch.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchantBranch.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_SetStatusToInactive_When_CurrentStatusIsNotInactive()
    {
        // Arrange
        var merchantBranch = CreateValidMerchantBranch(); // Starts as Active
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchantBranch.Deactivate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantBranch.Status.Should().Be(MerchantBranchStatus.Inactive);
        merchantBranch.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchantBranch.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Activate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyActive()
    {
        // Arrange
        var merchantBranch = CreateValidMerchantBranch(); // Starts as Active
        var initialUpdatedAtUtc = merchantBranch.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchantBranch.UpdatedAtLocal;
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchantBranch.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantBranch.Status.Should().Be(MerchantBranchStatus.Active);
        merchantBranch.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchantBranch.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyInactive()
    {
        // Arrange
        var merchantBranch = CreateValidMerchantBranch();
        merchantBranch.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var initialUpdatedAtUtc = merchantBranch.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchantBranch.UpdatedAtLocal;
        var newUpdatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var newUpdatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchantBranch.Deactivate(newUpdatedAtUtc, newUpdatedAtLocal);

        // Assert
        merchantBranch.Status.Should().Be(MerchantBranchStatus.Inactive);
        merchantBranch.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchantBranch.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    private static MerchantBranch CreateValidMerchantBranch()
    {
        return MerchantBranch.Create(
            ValidMerchantId,
            ValidBranchCode,
            ValidBranchName,
            ValidAddress,
            ValidCity,
            ValidRegion,
            ValidCountry,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);
    }
}