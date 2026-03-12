namespace Haulmer.Payments.Domain.Merchants;

public class MerchantBranch
{
    public long MerchantBranchId { get; private set; }
    public long MerchantId { get; private set; }
    public string BranchCode { get; private set; } = string.Empty;
    public string BranchName { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Region { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public MerchantBranchStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? UpdatedAtLocal { get; private set; }

    private MerchantBranch()
    {
    }

    public static MerchantBranch Create(
        long merchantId,
        string branchCode,
        string branchName,
        string address,
        string city,
        string region,
        string country,
        DateTime createdAtUtc,
        DateTime createdAtLocal)
    {
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new MerchantBranch
        {
            MerchantId = ValidateMerchantId(merchantId),
            BranchCode = ValidateRequired(branchCode, nameof(branchCode)),
            BranchName = ValidateRequired(branchName, nameof(branchName)),
            Address = ValidateRequired(address, nameof(address)),
            City = ValidateRequired(city, nameof(city)),
            Region = ValidateRequired(region, nameof(region)),
            Country = ValidateRequired(country, nameof(country)),
            Status = MerchantBranchStatus.Active,
            CreatedAtUtc = createdAtUtc,
            CreatedAtLocal = createdAtLocal
        };
    }

    public void Activate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantBranchStatus.Active)
            return;

        Status = MerchantBranchStatus.Active;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void Deactivate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantBranchStatus.Inactive)
            return;

        Status = MerchantBranchStatus.Inactive;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    private void MarkAsUpdated(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        UpdatedAtUtc = updatedAtUtc;
        UpdatedAtLocal = updatedAtLocal;
    }

    private static long ValidateMerchantId(long merchantId)
    {
        if (merchantId <= 0)
            throw new ArgumentException("MerchantId must be greater than zero.", nameof(merchantId));

        return merchantId;
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