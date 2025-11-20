# 🛒 Hepsiburada Satıcı Fiyat Karşılaştırma

Hepsiburada'dan ürün satıcılarını ve fiyatlarını otomatik olarak çeken web uygulaması.

<img width="1910" height="763" alt="image" src="https://github.com/user-attachments/assets/904d0d36-92b0-4af9-b0b5-f0836fa117e8" />

## 📝 Proje Hakkında

Bu proje, Hepsiburada'daki bir ürünün farklı satıcılar tarafından satılan fiyatlarını otomatik olarak toplar ve en ucuzdan en pahalıya sıralayarak gösterir.

**Örnek Ürün:** [Mamajoo Damlatmaz Eğitici Kulplu Bardak](https://www.hepsiburada.com/mamajoo-damlatmaz-egitici-kulplu-bardak-160-ml-powder-green-1-adet-p-HBCV00000UY1HP)

## 🚀 Özellikler

- ✅ Gerçek zamanlı fiyat karşılaştırma
- ✅ Otomatik sıralama (ucuzdan pahalıya)
- ✅ Hepsiburada temalı modern tasarım
- ✅ Headless browser ile veri çekme

## 🛠 Kullanılan Teknolojiler

- **.NET 10 Minimal API** - Backend API (Tek bir GET endpoint için Minimal API mantıklı ve yeterli olacaktır. Kısaca tek endpoint yöneten bir REST servisidir.)
- **Selenium WebDriver** - Web scraping
- **ASP.NET Core MVC** - Frontend
- **Bootstrap 5** - UI tasarımı
- **jQuery** - AJAX istekleri

## 📦 Kurulum

### Gereksinimler
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Chrome tarayıcı
- **ÖNEMLİ:** NuGet paket yöneticisi artık HTTPS bağlantısı gerektirmektedir. Projenin çalışması için internet bağlantınızın HTTPS protokolünü desteklemesi gerekmektedir.



### Projeyi Çalıştırma

1. **Projeyi indirin:**
```bash
git clone https://github.com/mertgogersin/DataSyncCaseStudy.git
cd DataSyncCaseStudy
```

2. **API'yi başlatın** (Terminal 1):
```bash
cd DataSync.Api
dotnet run --launch-profile http    # HTTP için
# veya
dotnet run --launch-profile https   # HTTPS için
```
- **HTTP:** http://localhost:5293
- **HTTPS:** https://localhost:7090


3. **Web uygulamasını başlatın** (Terminal 2):
```bash
cd DataSync.Web
dotnet run --launch-profile http    # HTTP için
# veya
dotnet run --launch-profile https   # HTTPS için
```
- **HTTP:** http://localhost:5267
- **HTTPS:** https://localhost:7172


4. **Tarayıcıda açın:**
- HTTP: `http://localhost:5267`
- HTTPS: `https://localhost:7172`

### 🔒 HTTPS/HTTP Protokol Yönetimi

Bu projede protokol uyumluluğu otomatik olarak yönetilir:
- Web uygulaması **HTTP** ile çalışıyorsa, API'ye **HTTP** ile bağlanır
- Web uygulaması **HTTPS** ile çalışıyorsa, API'ye **HTTPS** ile bağlanır

**CORS (Cross-Origin Resource Sharing) Yapılandırması:**
- API'de CORS tüm originlere açıktır (development için)
- Development ortamında HTTPS yönlendirmesi devre dışı bırakılmıştır
- Bu sayede hem HTTP hem HTTPS protokolleri sorunsuz çalışır

**Önemli:** Her iki uygulamayı da aynı protokol ile (ikisi de HTTP veya ikisi de HTTPS) çalıştırmanız önerilir. Mixed content (HTTPS'ten HTTP'ye istek) güvenlik nedeniyle tarayıcılar tarafından engellenebilir.


## 💡 Nasıl Çalışır?

1. Kullanıcı "Fiyatları Getir" butonuna tıklar
2. Frontend, API'ye istek gönderir
3. Selenium, headless Chrome ile Hepsiburada'yı açar
4. "Tümünü Gör" butonuna tıklar ve satıcı listesini açar
5. Satıcı isimlerini ve fiyatları toplar
6. Fiyata göre sıralar ve JSON olarak döner
7. Frontend, verileri tabloda gösterir

## 📁 Proje Yapısı

## API Endpoint : /api/sellers
## 🎯 Örnek API Response

```json
[
  {
    "name": "Minycenter",
    "price": 251.94,
    "sortOrder": 1
  },
  {
    "name": "MANDAŞ GROUP",
    "price": 313.00,
    "sortOrder": 2
  }
]
```

## ⚙️ Selenium Ayarları

Proje, Selenium'u şu şekilde yapılandırır:
- **Headless mod:** Görünmez tarayıcı (arka planda çalışır)
- **Bot tespitini aşma:** User-Agent ve özel ayarlar
- **Otomatik bekleme:** Elementler yüklenene kadar bir süre bekler
- **Implicit Wait:** 10 saniye - Elementleri bulmak için otomatik bekleme
- **Page Load Timeout:** 30 saniye - Sayfa yükleme için maksimum süre
- **Explicit Wait:** 5 saniye - Satıcı elementlerinin yüklenmesini bekler

### Bekleme Süreleri
- Sayfa ilk yüklendikten sonra: **3 saniye** (JavaScript içerikleri için)
- "Tümünü Gör" butonuna scroll sonrası: **500ms**
- "Tümünü Gör" butonuna tıklama sonrası: **2 saniye** (satıcı listesi açılması için)
- Satıcı elementleri aranmadan önce: **1 saniye** (ek güvenlik)

Bu bekleme süreleri, ilk çalıştırmada tüm satıcıların eksiksiz gelmesini sağlar.

## 🐛 Sorun Giderme

**API çalışmıyor?**
- Chrome yüklü olduğundan emin olun
- Port 5293 boşta olmalı

**Veri gelmiyor?**
- Her iki projeyi de çalıştırdığınızdan emin olun
- API'yi test edin:
  - HTTP: `http://localhost:5293/api/sellers`
  - HTTPS: `https://localhost:7090/api/sellers`

**CORS hatası alıyorsanız:**
- Her iki uygulamayı da aynı protokol ile çalıştırın (ikisi de HTTP veya ikisi de HTTPS)
- API'nin çalıştığından emin olun
- Tarayıcı konsolunda hata mesajını kontrol edin

**İlk tıklamada sadece birkaç satıcı geliyor, ikinci tıklamada hepsi geliyor?**
- Bu durum, Hepsiburada'nın JavaScript ile dinamik içerik yüklemesi nedeniyle normaldir
- Kod güncellemeleri ile bekleme süreleri optimize edildi
- İlk çalıştırmada ChromeDriver'ın başlatılması daha uzun sürebilir
- Eğer sorun devam ediyorsa, Program.cs'deki Thread.Sleep sürelerini artırabilirsiniz

