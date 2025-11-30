# 🚀 Struvio — .NET 10 SaaS Boilerplate

![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-green)
![License](https://img.shields.io/badge/License-MIT-blue)
![Status](https://img.shields.io/badge/Status-In%20Development-orange)

**Struvio**; tamamen .NET ekosistemi üzerinde çalışan, çok kuruluşlu (Multi-Tenant), çok dilli, hem web hem mobil için tek kod tabanına sahip modern bir SaaS uygulama çatısıdır. 

Performans, sade kod ve **"Sıfır 3. Parti Bağımlılık" (Zero Third-Party Dependency)** ilkesiyle tasarlanmıştır.

---

## 🌟 Özellikler

* **Zero Dependency:** AutoMapper, FluentValidation veya MediatR gibi harici kütüphaneler kullanılmaz. Saf .NET performansı.
* **Clean Architecture:** Servis odaklı, bağımsız ve test edilebilir katmanlı mimari.
* **Multi-Tenant:** Veritabanı seviyesinde `Global Query Filter` ile garanti edilmiş veri izolasyonu.
* **Hybrid ID:** Performans ve güvenlik için `Sequential GUID` (UUID v7) kullanımı.
* **Code Once, Run Everywhere:** Blazor WASM ve .NET MAUI ile tek UI kod tabanı (RCL).
* **Structured Logging:** Serilog ile detaylı ve sorgulanabilir loglama.
* **Developer Experience:** Tek tıkla kurulum sağlayan `Setup` projesi.
* **Modern Docs:** Swagger yerine **Scalar UI** ve Native OpenAPI desteği.

---

## 🏗 Mimari ve Veri Stratejisi

### Kimlik ve İlişkiler
* **ID Stratejisi:** UUID v7 (Zaman bazlı sıralı GUID).
* **Party Pattern:** `Person` (Gerçek Kişi) ve `User` (Hesap) ayrımı (1:1 ilişki).
* **Multi-Tenancy:** Paylaşımlı Veritabanı + TenantId Ayrıştırıcı Kolon.
* **Audit & History:**
    * Application seviyesinde Audit Log (`CreatedBy`, `CreatedAt`).
    * Database seviyesinde SQL Server **Temporal Tables** (Tarihçe).

### Altyapı Kararları
* **Validasyon:** `DataAnnotations` (Yapısal) + Servis İçi Kontroller (Business).
* **Exception:** Global Middleware ile merkezi hata yönetimi (400/404/500).
* **UI:** Razor Class Library (RCL) üzerinde **MudBlazor** bileşen kütüphanesi.

---

## 📂 Proje Yapısı

```text
Struvio.Solution
├── 01.Core
│   ├── Struvio.Domain          # Entities, Enums, Constants
│   ├── Struvio.Application     # Interfaces, DTOs, Services, Exceptions
├── 02.Infrastructure
│   ├── Struvio.Persistence     # EF Core, DbContext, Migrations
│   ├── Struvio.Infrastructure  # Services Impl (File, Email, Cache)
├── 03.Presentation
│   ├── Struvio.API             # Controllers (Thin Layer)
│   ├── Clients
│       ├── Struvio.UI.RCL      # TÜM UI Kodu (Pages, Components)
│       ├── Struvio.UI.Wasm     # Web Host
│       └── Struvio.UI.Maui     # Mobile Host
├── 04.Shared
│   ├── Struvio.Shared          # Ortak DTOs, Localization (.resx)
└── 05.Tools
    └── Struvio.Setup           # Geliştirici Kurulum Aracı