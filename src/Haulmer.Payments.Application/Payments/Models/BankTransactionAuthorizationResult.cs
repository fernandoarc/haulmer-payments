namespace Haulmer.Payments.Application.Payments.Models;

public class BankTransactionAuthorizationResult
{
    public bool IsApproved { get; set; }
    public string AcquirerReference { get; set; } = string.Empty;
    public string ResponseCode { get; set; } = string.Empty;
    public string ResponseMessage { get; set; } = string.Empty;
    public string IssuingBankName { get; set; } = string.Empty;
    public string CardBrand { get; set; } = string.Empty;
    public string CardLast4 { get; set; } = string.Empty;
    public string MaskedPan { get; set; } = string.Empty;
}
