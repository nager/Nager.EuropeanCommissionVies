namespace Nager.EuropeanCommissionVies.Models
{
    public class VatCheckErrorResponse
    {
        public bool ActionSucceed { get; set; }
        public ErrorMessage[]? ErrorWrappers { get; set; }
    }
}