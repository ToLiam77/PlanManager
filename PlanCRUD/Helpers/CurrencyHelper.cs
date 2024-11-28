namespace PlanCRUD.Helpers
{
    public class CurrencyHelper
    {
        public static string GetCurrencySymbol(string currencyCode)
        {
            return currencyCode switch
            {
                "GBP" => "£", 
                "EUR" => "€", 
                "USD" => "$", 
                "JPY" => "¥", 
                _ => string.Empty 
            };
        }
    }
}
