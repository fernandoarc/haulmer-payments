namespace Haulmer.Payments.Domain.Merchants;

public class Merchant
{
    public long MerchantId { get; private set; }
    public string MerchantCode { get; private set; } = string.Empty;
    public string CompanyRut { get; private set; } = string.Empty;
    public string BusinessLegalName { get; private set; } = string.Empty;
    public string TradeName { get; private set; } = string.Empty;
    public string HeadOfficeAddress { get; private set; } = string.Empty;
    public string ContactEmail { get; private set; } = string.Empty;
    public string ContactPhone { get; private set; } = string.Empty;
    public MerchantStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? UpdatedAtLocal { get; private set; }

    private Merchant()
    {
    }

    public static Merchant Create(
        string merchantCode,
        string companyRut,
        string businessLegalName,
        string tradeName,
        string headOfficeAddress,
        string contactEmail,
        string contactPhone,
        DateTime createdAtUtc,
        DateTime createdAtLocal)
    {
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new Merchant
        {
            MerchantCode = ValidateRequired(merchantCode, nameof(merchantCode)),
            CompanyRut = ValidateRequired(companyRut, nameof(companyRut)),
            BusinessLegalName = ValidateRequired(businessLegalName, nameof(businessLegalName)),
            TradeName = ValidateRequired(tradeName, nameof(tradeName)),
            HeadOfficeAddress = ValidateRequired(headOfficeAddress, nameof(headOfficeAddress)),
            ContactEmail = ValidateRequired(contactEmail, nameof(contactEmail)),
            ContactPhone = ValidateRequired(contactPhone, nameof(contactPhone)),
            Status = MerchantStatus.Active,
            CreatedAtUtc = createdAtUtc,
            CreatedAtLocal = createdAtLocal
        };
    }

    public void UpdateContactInformation(
        string headOfficeAddress,
        string contactEmail,
        string contactPhone,
        DateTime updatedAtUtc,
        DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        HeadOfficeAddress = ValidateRequired(headOfficeAddress, nameof(headOfficeAddress));
        ContactEmail = ValidateRequired(contactEmail, nameof(contactEmail));
        ContactPhone = ValidateRequired(contactPhone, nameof(contactPhone));

        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void Activate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantStatus.Active)
            return;

        Status = MerchantStatus.Active;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void Deactivate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantStatus.Inactive)
            return;

        Status = MerchantStatus.Inactive;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void Suspend(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantStatus.Suspended)
            return;

        Status = MerchantStatus.Suspended;
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