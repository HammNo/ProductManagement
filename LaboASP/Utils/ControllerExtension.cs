using Microsoft.AspNetCore.Mvc;
using ProductManagement.ASP.Exceptions;

namespace ProductManagement.ASP.Utils
{
    public static class ControllerExtension
    {
        public static decimal PriceConversion(this Controller controller, string inputPrice)
        {
            bool is_dec = false;
            decimal price;
            if (inputPrice.Contains(".") ^ inputPrice.Contains(","))
            {
                int price_length = 0;
                if (inputPrice.Count(c => c == '.') > 1 || inputPrice.Count(c => c == ',') > 1)
                {
                    throw new ModelException("Price", "Format du prix incorrect");
                }
                if (inputPrice.Contains(".")) price_length = inputPrice.Length - inputPrice.IndexOf(".") - 1;
                if (inputPrice.Contains(",")) price_length = inputPrice.Length - inputPrice.IndexOf(",") - 1;
                if (price_length > 2)
                {
                    throw new ModelException("Price", "Format du prix incorrect");
                }
                else if (price_length == 1)
                {
                    inputPrice += "0";
                }
                inputPrice.Replace(".", "");
                inputPrice.Replace(",", "");
                is_dec = true;
            }
            try
            {
                price = decimal.Parse(inputPrice);
                if (is_dec) price /= 100;
            }
            catch
            {
                throw new ModelException("Price", "Format du prix incorrect");
            }
            return price;
        }
    }
}
