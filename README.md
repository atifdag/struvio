# .NET 10 Clean Architecture & Multi-Tenant SaaS Framework

**Versiyon:** 1.0  
**Lisans:** MIT  
**Teknoloji:** .NET 10 • Clean Architecture • Pure .NET • Blazor WASM • MAUI • EF Core  

Struvio, tamamen .NET ekosistemi üzerinde çalışan, çok kuruluşlu (Multi-Tenant), çok dilli, hem web hem mobil için tek kod tabanına sahip modern bir SaaS uygulama çatısıdır. Performans, sade kod ve üçüncü parti bağımlılıksız bir mimari hedeflenmiştir.

---

# Özellikler
- ✔ Zero Third-Party Dependency  
- ✔ Clean Architecture  
- ✔ Multi-Tenant veri izolasyonu  
- ✔ Sequential GUID (UUID v7)  
- ✔ Blazor WASM + MAUI tek kod tabanı  
- ✔ Serilog structured logging  
- ✔ Setup projesi  
- ✔ Scalar UI API dokümantasyonu  

---

# Mimari Genel Yapı

```
Presentation → Application → Domain → Infrastructure
```

---

# Veri Yönetimi Stratejileri

## ID Stratejisi
- Sequential GUID (UUID v7)

## Party Pattern
- Person ↔ User (1:1)

## Multi-Tenant
- Shared DB + TenantId + Global Query Filter

## Audit & History
- Audit log + Temporal history

---

# Validasyon ve Hata Yönetimi

## Validasyon
- DataAnnotations  
- Servis içinde business validation  

## Exception Yönetimi
- Global Exception Middleware  
- 400 / 404 / 500 yapılandırması  

---

# UI Tasarım Yaklaşımı
- Razor Class Library  
- MudBlazor UI Kit  
- Responsive grid  
- Çoklu tema desteği  

---

# Lisans
MIT License

---

# Katkı Sağlama
Pull Request ve Issue katkıları açıktır.

