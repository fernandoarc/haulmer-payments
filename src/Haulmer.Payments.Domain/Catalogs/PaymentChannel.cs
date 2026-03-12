namespace Haulmer.Payments.Domain.Catalogs;

public class PaymentChannel
{
    public long PaymentChannelId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public PaymentChannelStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? UpdatedAtLocal { get; private set; }

    private PaymentChannel()
    {
    }

    public static PaymentChannel Create(
        string code,
        string name,
        DateTime createdAtUtc,
        DateTime createdAtLocal)
    {
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new PaymentChannel
        {
            Code = ValidateRequired(code, nameof(code)),
            Name = ValidateRequired(name, nameof(name)),
            Status = PaymentChannelStatus.Active,
            CreatedAtUtc = createdAtUtc,
            CreatedAtLocal = createdAtLocal
        };
    }

    public void Activate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == PaymentChannelStatus.Active)
            return;

        Status = PaymentChannelStatus.Active;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void Deactivate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == PaymentChannelStatus.Inactive)
            return;

        Status = PaymentChannelStatus.Inactive;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    private void MarkAsUpdated(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        UpdatedAtUtc = updatedAtUtc;
        UpdatedAtLocal = updatedAtLocal;
    }

    private static string ValidateRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} is required.", paramName);

        return value.Trim();
    }

    private static void ValidateCreationDates(DateTime createdAtUtc, DateTime createdAtLocal)
    {
        if (createdAtUtc == default)
            throw new ArgumentException("createdAtUtc is required.", nameof(createdAtUtc));

        if (createdAtLocal == default)
            throw new ArgumentException("createdAtLocal is required.", nameof(createdAtLocal));
    }

    private static void ValidateUpdateDates(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        if (updatedAtUtc == default)
            throw new ArgumentException("updatedAtUtc is required.", nameof(updatedAtUtc));

        if (updatedAtLocal == default)
            throw new ArgumentException("updatedAtLocal is required.", nameof(updatedAtLocal));
    }
}