namespace ProductManager.Domain.Constansts
{
    public enum CurrencyType
    {
        USD = 0,
        CAD,
        EUR,
        GBP
    }

    public static class CurrencyExtensions
    {
        public static string GetString(this CurrencyType source)
        {
            switch (source)
            {
                case CurrencyType.USD:
                    return "USD";
                case CurrencyType.CAD:
                    return "CAD";
                case CurrencyType.EUR:
                    return "EUR";
                case CurrencyType.GBP:
                    return "GBP";
                default:
                    return "USD";
            }
        }
    }
}
