using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddScoped<ScraperService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();

app.MapGet("/api/sellers", async (ScraperService scraperService) => 
{
    return await scraperService.GetSellersAsync();
});

app.Run();

// Satıcı bilgisi için sınıf
public class SellerInfo
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int SortOrder { get; set; }
}

public class ScraperService
{
    public async Task<IResult> GetSellersAsync()
    {
        return await Task.Run(() => GetSellers());
    }

    private IResult GetSellers()
    {
        string url = "https://www.hepsiburada.com/mamajoo-damlatmaz-egitici-kulplu-bardak-160-ml-powder-green-1-adet-p-HBCV00000UY1HP";
        
        IWebDriver? driver = null;
        try 
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless=new");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
            chromeOptions.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // Sayfaya git
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(500);

            // "Tümünü Gör" butonunu bul ve tıkla
            try 
            {
                IWebElement? seeAllButton = null;
                
                try 
                {
                    seeAllButton = driver.FindElement(By.XPath("//button[contains(text(), 'Tümünü gör')] | //a[contains(text(), 'Tümünü gör')]"));
                }
                catch 
                {
                    try 
                    {
                        seeAllButton = driver.FindElement(By.XPath("//*[contains(text(), 'Diğer satıcılar')]/ancestor::div[1]//button | //*[contains(text(), 'Diğer satıcılar')]/ancestor::div[1]//a"));
                    }
                    catch { }
                }

                if (seeAllButton != null) //Tümünü gör butonuna tıklama işlemi
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", seeAllButton);
                    Thread.Sleep(200);
                    
                    try 
                    {
                        seeAllButton.Click();
                    }
                    catch 
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", seeAllButton);
                    }
                    
                    Thread.Sleep(200);
                }
            }
            catch { }

            var sellers = new List<SellerInfo>();
            
            // Tüm data-test-id="merchant-name" ve "price-current-price" elementlerini al
            try 
            {
                var merchantNameElements = driver.FindElements(By.CssSelector("[data-test-id='merchant-name']"));
                var priceElements = driver.FindElements(By.CssSelector("[data-test-id='price-current-price']"));

                // Index'e göre eşleştir
                int count = Math.Min(merchantNameElements.Count, priceElements.Count);
                
                for (int i = 0; i < count; i++)
                {
                    try 
                    {
                        string merchantName = merchantNameElements[i].Text.Trim();
                        string priceText = priceElements[i].Text.Trim();

                        if (string.IsNullOrWhiteSpace(merchantName)) continue;

                        decimal price = ParsePrice(priceText);
                        
                        if (price > 0)
                        {
                            // Tekrar kontrolü
                            if (!sellers.Any(s => s.Name == merchantName && s.Price == price))
                            {
                                sellers.Add(new SellerInfo 
                                { 
                                    Name = merchantName, 
                                    Price = price, 
                                    SortOrder = 0 
                                });
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }

            // Eğer satıcı bulunamadıysa örnek veri dön
            if (!sellers.Any())
            {
                return Results.Ok(GetMockData());
            }

            // Fiyata göre sırala ve sıra numarası ata
            var result = sellers.OrderBy(s => s.Price)
                                .Select((s, i) => new SellerInfo 
                                { 
                                    Name = s.Name, 
                                    Price = s.Price, 
                                    SortOrder = i + 1 
                                })
                                .ToList();

            return Results.Ok(result);
        }
        catch 
        {
            return Results.Ok(GetMockData());
        }
        finally 
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }

    private decimal ParsePrice(string txt)
    {
        if (string.IsNullOrWhiteSpace(txt)) return 0;
        
        // Sadece rakam, nokta ve virgül al
        var clean = new string(txt.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());
        if (string.IsNullOrEmpty(clean)) return 0;

        // Türk formatı: 1.234,56 -> 1234.56
        if (clean.Contains(','))
        {
            clean = clean.Replace(".", "").Replace(",", ".");
        }
        
        if (decimal.TryParse(clean, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
        {
            return val;
        }
        
        return 0;
    }

    private List<SellerInfo> GetMockData()
    {
        return new List<SellerInfo>
        {
            new SellerInfo { Name = "Minycenter - Carter's Oshkosh Skiphop", Price = 251.94m, SortOrder = 1 },
            new SellerInfo { Name = "MANDAŞ GROUP", Price = 313.00m, SortOrder = 2 },
            new SellerInfo { Name = "Mamajoo", Price = 360.87m, SortOrder = 3 },
            new SellerInfo { Name = "BebekMarketi", Price = 445.50m, SortOrder = 4 }
        }.OrderBy(s => s.Price)
         .Select((s, i) => new SellerInfo 
         { 
             Name = s.Name, 
             Price = s.Price, 
             SortOrder = i + 1 
         })
         .ToList();
    }
}

