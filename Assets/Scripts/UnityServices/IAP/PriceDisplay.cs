using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

/// <summary>
/// Simple script to display product price
/// Connect this to IAP Button's OnProductFetched event
/// </summary>
public class PriceDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI priceText;

    [Header("Optional Formatting")]
    public string pricePrefix = "";
    public string priceSuffix = "";
    public bool filterPrice = false;

    /// <summary>
    /// Call this from IAP Button's OnProductFetched event
    /// </summary>
    public void UpdatePrice(Product product)
    {
        if (priceText == null || product == null)
            return;

        string priceString = product.metadata.localizedPriceString;

        if (filterPrice)
        {
            priceString = FilterPrice(priceString);
        }

        priceText.text = $"{pricePrefix}{priceString}{priceSuffix}";
    }

    private string FilterPrice(string price)
    {
        string filtered = "";
        foreach (char c in price)
        {
            if (char.IsDigit(c) || c == '.' || c == ',' || IsCurrencySymbol(c))
            {
                filtered += c;
            }
        }
        return filtered;
    }

    private bool IsCurrencySymbol(char c)
    {
        return (c >= '\u20A0' && c <= '\u20CF') ||  // Currency Symbols block
               (c == '\u0024') || // $ (Dollar)
               (c == '\u00A3') || // £ (Pound)
               (c == '\u00A5') || // ¥ (Yen)
               (c == '\u20AC') || // € (Euro)
               (c == '\u09F3') || // ৳ (Bangladeshi Taka)
               (c == '\u0E3F') || // ฿ (Thai Baht)
               (c == '\u17DB') || // ៛ (Cambodian Riel)
               (c == '\u20AA') || // ₪ (Israeli Shekel)
               (c == '\u20BA') || // ₺ (Turkish Lira)
               (c == '\u20BD') || // ₽ (Russian Ruble)
               (c == '\u20B9') || // ₹ (Indian Rupee)
               (c == '\uFDFC');   // ﷼ (Rial)
    }
}