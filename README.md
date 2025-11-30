# Struvio — .NET 10 Enterprise SaaS Framework

![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20%26%20Service%20Oriented-green)
![UI Strategy](https://img.shields.io/badge/UI-Blazor%20United%20(RCL)-purple)
![License](https://img.shields.io/badge/License-MIT-blue)

**Struvio**; .NET 10 ekosistemi üzerine inşa edilmiş, "Clean Architecture" prensiplerine sadık, çok kuruluşlu (Multi-Tenant), çok dilli ve hem web hem mobil platformlarda **tek kod tabanıyla** çalışabilen modern bir SaaS uygulama çatısıdır.

Projenin öncelikli hedefi, **Ortak Admin Paneli** (Unified Admin Panel) mimarisini kurmaktır.

---

## Mimari Prensipler ve "Pure .NET" Felsefesi

Struvio, **"Zero Third-Party Dependency" (Sıfır 3. Parti Bağımlılık)** ilkesiyle tasarlanmıştır. Projenin sürdürülebilirliği ve performansı için aşağıdaki kurallar esastır:

**Bağımlılık Yok:**
   - **AutoMapper** kullanılmaz → Yerine compile-time çalışan `Extension Methods`
   - **FluentValidation** kullanılmaz → Yerine `DataAnnotations` + servis içi iş kuralları
   - **MediatR** kullanılmaz → Servis odaklı (Service-Oriented) doğrudan DI

**Thin Controller:**  
   API Controller'lar sadece trafiği yönetir, iş mantığı Servis katmanındadır.

**Code Once, Run Everywhere:**  
   Tüm UI kodları (`Pages`, `Components`) `Struvio.UI.RCL` içinde yer alır.

---

## Proje Yapısı

```
Struvio.Solution
│
├── Core                        # Projenin Kalbi (Bağımlılık Yok)
│   ├── Struvio.Domain          # Entities, Enums, Constants (POCO)
│   ├── Struvio.Application     # Interfaces, Services Logic, DTOs, Exceptions
│
├── Infrastructure              # Altyapı
│   ├── Struvio.Persistence     # EF Core, DbContext, Migrations
│   ├── Struvio.Infrastructure  # File, Email, Cache, Identity Services
│
├── UI                          # Sunum Katmanı
│   ├── Struvio.API             # Backend API (Thin Controllers)
│   ├── Struvio.Web.Common      # MVC ve API ortak servisleri
│   ├── Struvio.Web.Mvc         # Server-Side Blazor Host (Admin Panel)
│   └── Clients                 # Frontend Host Uygulamaları
│       ├── Struvio.UI.RCL      # ⭐️ TÜM UI KODU (Admin Pages, Components)
│       ├── Struvio.UI.Wasm     # Blazor WebAssembly Host
│       └── Struvio.UI.Maui     # .NET MAUI (Mobile/Desktop) Host
│
├── Shared                      # Ortak Kaynaklar
│   ├── Struvio.Shared          # DTOs, Localization (.resx), Wrappers
│
├── Tools                       # Araçlar
│   └── Struvio.Setup           # Tek tıkla kurulum (Dev Only)
│
└── Tests                       # Testler
    └── Struvio.Application.UnitTests
```

---

## Teknik Stratejiler

### Kimlik ve Veri Yönetimi

- **ID Stratejisi:** Sequential GUID (UUID v7)
- **Party Pattern:** User (Hesap) ↔ Person (Gerçek kişi)
- **Multi-Tenancy:** Paylaşımlı DB + TenantId
  - Token claim → tenant belirleme
  - Global Query Filter → veri izolasyonu
- **Audit & History**
  - Application Level → `AuditableEntityInterceptor`
  - DB Level → SQL Server **Temporal Tables**

---

### Konfigürasyon Yönetimi

- **Global Ayarlar:** DB'de `GlobalConfiguration` tablosu  
  .NET IConfiguration ile entegre, Hot Reload destekli
- **Tenant Ayarları:** `TenantSettings` tablosu  
  Tenant bazlı cache mekanizması
- **Fallback:** Tenant ayarı yoksa global değer kullanılır.

---

### Validasyon ve Hata Yönetimi

**Validasyon:**

- Yapısal → DataAnnotations  
- İş Kuralı → Servis içi manuel kontroller

**Error Pipeline:**

- BusinessException → **400**
- NotFoundException → **404**
- System Exception → **500**

---

### Unified Admin Panel (UI Stratejisi)

Admin Panel 3 farklı şekilde host edilir:

| Platform | Çalışma Modu | Proje |
|---------|---------------|--------|
| Web (Server-Side) | Blazor Server | `Struvio.Web.Mvc` |
| Web (Client-Side) | Blazor WASM | `Struvio.UI.Wasm` |
| Mobil/Desktop | MAUI Hybrid | `Struvio.UI.Maui` |

> Tüm UI kodları → `Struvio.UI.RCL`

---

### Localization (Çoklu Dil)

- `.resx` tabanlı yapısal çeviri sistemi  
- `LanguageTexts.Key` ile tip güvenli erişim  
- Otomatik dil seçimi:
  - Anonim → tarayıcı dili  
  - Login → kullanıcı profilindeki dil  

---

### Performans ve Altyapı

- **Cache:** .NET 9 HybridCache (Memory + Redis)
- **Loglama:** Serilog JSON loglama
- **File Storage:** Local / S3 / MinIO seçilebilir yapı
- **API Docs:** Swagger yerine **Scalar UI**

---

# Kurulum (Getting Started)

### Repoyu klonlayın
```
git clone https://github.com/struvio/struvio.git
```

### Connection String’i düzenleyin  
`UI/Struvio.API/appsettings.json`

### API’yi başlatın  
`Struvio.API` → F5

### Setup ekranı  
```
https://localhost:{port}/setup
```
Bu ekran otomatik olarak:

- Veritabanı oluşturur  
- Migration'ları uygular  
- Host organizasyonunu kurar  
- Admin kullanıcısını oluşturur  
- Varsayılan ayarları yükler  

### API Test  
```
/scalar/v1
```

---

# Lisans

Bu proje **MIT Lisansı** ile lisanslanmıştır.
