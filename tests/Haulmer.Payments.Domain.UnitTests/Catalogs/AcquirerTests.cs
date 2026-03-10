using Haulmer.Payments.Domain.Catalogs;

namespace Haulmer.Payments.Domain.UnitTests.Catalogs;

public class AcquirerTests
{
    private const string ValidCode = "ACQ001";
    private const string ValidName = "Bank of Chile";

    private static DateTime ValidCreatedAtUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidCreatedAtLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);
    private static DateTime ValidUpdatedAtUtc => new DateTime(2023, 1, 2, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidUpdatedAtLocal => new DateTime(2023, 1, 2, 9, 0, 0, DateTimeKind.Local);

    [Fact]
    public void Create_Should_SetInitialValues_When_InputIsValid()
    {
        // Arrange
        var code = ValidCode;
        var name = ValidName;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var acquirer = Acquirer.Create(
            code,
            name,
            createdAtUtc,
            createdAtLocal);

        // Assert
        acquirer.AcquirerId.Should().Be(0); // Default for new entity
        acquirer.Code.Should().Be(code);
        acquirer.Name.Should().Be(name);
        acquirer.Status.Should().Be(AcquirerStatus.Active);
        acquirer.CreatedAtUtc.Should().Be(createdAtUtc);
        acquirer.CreatedAtLocal.Should().Be(createdAtLocal);
        acquirer.UpdatedAtUtc.Should().BeNull();
        acquirer.UpdatedAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CodeIsEmpty()
    {
        // Arrange
        var code = string.Empty;

        // Act
        var act = () => Acquirer.Create(
            code,
            ValidName,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("code is required. (Parameter 'code')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_NameIsEmpty()
    {
        // Arrange
        var name = string.Empty;

        // Act
        var act = () => Acquirer.Create(
            ValidCode,
            name,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("name is required. (Parameter 'name')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtUtcIsDefault()
    {
        // Arrange
        var createdAtUtc = default(DateTime);

        // Act
        var act = () => Acquirer.Create(
            ValidCode,
            ValidName,
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
        var act = () => Acquirer.Create(
            ValidCode,
            ValidName,
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
        var acquirer = CreateValidAcquirer();
        acquirer.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        acquirer.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        acquirer.Status.Should().Be(AcquirerStatus.Active);
        acquirer.UpdatedAtUtc.Should().Be(updatedAtUtc);
        acquirer.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_SetStatusToInactive_When_CurrentStatusIsNotInactive()
    {
        // Arrange
        var acquirer = CreateValidAcquirer(); // Starts as Active
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        acquirer.Deactivate(updatedAtUtc, updatedAtLocal);

        // Assert
        acquirer.Status.Should().Be(AcquirerStatus.Inactive);
        acquirer.UpdatedAtUtc.Should().Be(updatedAtUtc);
        acquirer.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Activate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyActive()
    {
        // Arrange
        var acquirer = CreateValidAcquirer(); // Starts as Active
        var initialUpdatedAtUtc = acquirer.UpdatedAtUtc;
        var initialUpdatedAtLocal = acquirer.UpdatedAtLocal;
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        acquirer.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        acquirer.Status.Should().Be(AcquirerStatus.Active);
        acquirer.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        acquirer.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyInactive()
    {
        // Arrange
        var acquirer = CreateValidAcquirer();
        acquirer.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var initialUpdatedAtUtc = acquirer.UpdatedAtUtc;
        var initialUpdatedAtLocal = acquirer.UpdatedAtLocal;
        var newUpdatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var newUpdatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        acquirer.Deactivate(newUpdatedAtUtc, newUpdatedAtLocal);

        // Assert
        acquirer.Status.Should().Be(AcquirerStatus.Inactive);
        acquirer.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        acquirer.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    private static Acquirer CreateValidAcquirer()
    {
        return Acquirer.Create(
            ValidCode,
            ValidName,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);
    }
}