using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
public class IAPFetchButton : MonoBehaviour
{
    public TextMeshProUGUI price; 
    public string id;

    public void FetchProduct(Product product) 
    {

        if(price != null && product.definition != null && product.definition.id == id)
        {
            //decimal roundedPrice = product.metadata.localizedPrice;//roundUp(product.metadata.localizedPrice, 3);
            //string priceString = product.metadata.localizedPriceString;
            //string res = roundedPrice + priceString[priceString.Length - 1].ToString();
            string priceString = product.metadata.localizedPriceString;

            // Filter the price string to only include allowed characters
            string filteredPriceString = new string(priceString
                .Where(c => char.IsDigit(c) || c == '.' || IsCurrencySymbol(c))
                .ToArray());

            // Set the filtered price string as the text
            price.text = filteredPriceString;
        }
    }

    // Helper method to check if a character is a currency symbol
    private bool IsCurrencySymbol(char c)
    {
        return (c >= '\u20A0' && c <= '\u20CF') ||  // Currency Symbols block
               (c == '\u0024') || // $
               (c == '\u00A3') || // £
               (c == '\u00A5') || // ¥
               (c == '\u09F3') || // ৳
               (c == '\u0E3F') || // ฿
               (c == '\u17DB') || // ៛
               (c == '\u20BA') || // ₺
               (c == '\u20BD') || // ₽
               (c == '\u20B9') || // ₹
               (c == '\uFDFC');   // ﷼
    }

    /*
    private decimal roundUp(decimal n, int d)
    {
        double x = 0;
        decimal r = Math.Round(n, d);
        if (r < n)
            x = Math.Pow(10, -1 * d);
        //Console.WriteLine("x: " + x);
        decimal res = r + (decimal)x;
        //Console.WriteLine("r: " + r);
        //Console.WriteLine("(" + n + "," + d + ")" + ":" + res);
        return res;
    }*/
}
