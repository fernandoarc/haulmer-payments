namespace Haulmer.Payments.Application.Payments.Models;

public class BankTransactionAuthorizationResult
{
    public bool IsApproved { get; set; }
    public string AcquirerReference { get; set; }
    public string ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
    public string IssuingBankName { get; set; }
    public string CardBrand { get; set; }
    public string CardLast4 { get; set; }
    public string MaskedPan { get; set; }
}
