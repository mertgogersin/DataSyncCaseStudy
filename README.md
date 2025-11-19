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

- **.NET 10** - Backend API (Minimal API)
- **Selenium WebDriver** - Web scraping
- **ASP.NET Core MVC** - Frontend
- **Bootstrap 5** - UI tasarımı
- **jQuery** - AJAX istekleri

## 📦 Kurulum

### Gereksinimler
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Chrome tarayıcı

### Projeyi Çalıştırma

1. **Projeyi indirin:**
```bash
git clone https://github.com/mertgogersin/DataSyncCaseStudy.git
cd DataSyncCaseStudy
```

2. **API'yi başlatın** (Terminal 1):

 "http://localhost:5293"


3. **Web uygulamasını başlatın** (Terminal 2):

 "http://localhost:5267"


4. **Tarayıcıda açın:**
```
http://localhost:5267
```

## 💡 Nasıl Çalışır?

1. Kullanıcı "Fiyatları Getir" butonuna tıklar
2. Frontend, API'ye istek gönderir
3. Selenium, headless Chrome ile Hepsiburada'yı açar
4. "Tümünü Gör" butonuna tıklar ve satıcı listesini açar
5. Satıcı isimlerini ve fiyatları toplar
6. Fiyata göre sıralar ve JSON olarak döner
7. Frontend, verileri tabloda gösterir

## 📁 Proje Yapısı


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

## 🐛 Sorun Giderme

**API çalışmıyor?**
- Chrome yüklü olduğundan emin olun
- Port 5293 boşta olmalı

**Veri gelmiyor?**
- Her iki projeyi de çalıştırdığınızdan emin olun
- API'yi test edin: `http://localhost:5293/api/sellers`


