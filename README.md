# Todo MVC Uygulaması

Bu proje, .NET 9.0 kullanılarak geliştirilmiş bir Todo yönetim uygulamasıdır. Uygulama, Model-View-Controller (MVC) mimarisi kullanarak geliştirilmiştir.

## 🚀 Özellikler

- **Görev Yönetimi**: Görev oluşturma, düzenleme, silme ve tamamlama
- **Öncelik Seviyeleri**: 3 farklı öncelik seviyesi (Düşük, Orta, Yüksek)
- **Kullanıcı Yönetimi**: Kayıt olma, giriş yapma ve profil yönetimi
- **Responsive Tasarım**: Bootstrap 5 ile modern ve mobil uyumlu arayüz
- **Session-based Authentication**: Güvenli kimlik doğrulama sistemi
- **Entity Framework**: PostgreSQL veritabanı ile veri yönetimi

## 🏗️ Mimari

### MVC Yapısı
- **Models**: Veri modelleri (Todo, User)
- **Views**: Razor sayfaları (HTML + C#)
- **Controllers**: MVC controller'ları ve API controller'ları

### Katmanlı Mimari
- **Controllers**: HTTP isteklerini işleme
- **Services**: İş mantığı katmanı
- **Repositories**: Veri erişim katmanı
- **DTOs**: Veri transfer objeleri

## 🛠️ Teknolojiler

- **Backend**: .NET 9.0, ASP.NET Core MVC
- **Veritabanı**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: Session-based Authentication
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5
- **Icons**: Font Awesome

## 📁 Proje Yapısı

```
TodoNVC/
├── Controllers/          # MVC ve API Controller'ları
├── Data/                # Entity Framework context
├── DTOs/                # Veri transfer objeleri
├── Migrations/          # Veritabanı migration'ları
├── Models/              # Veri modelleri
├── Repositories/        # Veri erişim katmanı
├── Services/            # İş mantığı katmanı
├── Views/               # Razor view'ları
│   ├── Home/           # Ana sayfa view'ları
│   ├── Todo/           # Todo yönetimi view'ları
│   ├── User/           # Kullanıcı yönetimi view'ları
│   └── Shared/         # Paylaşılan layout ve partial'lar
├── wwwroot/            # Statik dosyalar (CSS, JS, images)
└── Program.cs          # Uygulama giriş noktası
```

## 🚀 Kurulum

### Gereksinimler
- .NET 9.0 SDK
- PostgreSQL veritabanı
- Docker (opsiyonel)

### 1. Projeyi Klonlayın
```bash
git clone <repository-url>
cd TodoNVC
```

### 2. Veritabanını Kurun
PostgreSQL veritabanını kurun ve connection string'i `appsettings.json` dosyasında güncelleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=tododb;Username=your_username;Password=your_password"
  }
}
```

### 3. Veritabanı Migration'larını Çalıştırın
```bash
dotnet ef database update
```

### 4. Uygulamayı Çalıştırın
```bash
dotnet run
```

Uygulama `https://localhost:5001` adresinde çalışacaktır.

## 📱 Kullanım

### Ana Sayfa
- Uygulama hakkında genel bilgiler
- Hızlı erişim linkleri
- Özellik açıklamaları

### Görev Yönetimi
- **Görevleri Görüntüleme**: Tüm görevleri listeleme
- **Yeni Görev Oluşturma**: Başlık, açıklama ve öncelik ile görev ekleme
- **Görev Düzenleme**: Mevcut görevleri güncelleme
- **Görev Tamamlama**: Görevleri tamamlandı olarak işaretleme
- **Görev Silme**: Görevleri kalıcı olarak kaldırma

### Kullanıcı Yönetimi
- **Kayıt Olma**: Yeni hesap oluşturma
- **Giriş Yapma**: Mevcut hesapla giriş yapma
- **Profil Görüntüleme**: Kullanıcı bilgilerini görme
- **Çıkış Yapma**: Güvenli çıkış

## 🔧 API Endpoints

### Todo API (Frontend için)
- `GET /api/todo` - Tüm görevleri listele
- `PATCH /api/todo/{id}/complete` - Görevi tamamla
- `DELETE /api/todo/{id}` - Görevi sil

### User API (Frontend için)
- `POST /api/user/register` - Kullanıcı kaydı
- `POST /api/user/login` - Kullanıcı girişi

### MVC Controller Endpoints
- **Todo Controller**: Görev oluşturma, düzenleme, detay görüntüleme, filtreleme ve arama
- **User Controller**: Kullanıcı profil görüntüleme ve çıkış işlemleri

## 🎨 Özelleştirme

### CSS Stilleri
`wwwroot/css/styles.css` dosyasında özel stilleri düzenleyebilirsiniz.

### JavaScript
`wwwroot/js/` klasöründeki dosyalarda frontend mantığını özelleştirebilirsiniz.

### View'lar
`Views/` klasöründeki Razor sayfalarında arayüzü değiştirebilirsiniz.

## 🔒 Güvenlik

- Session tabanlı kimlik doğrulama
- Anti-forgery token koruması
- Session yönetimi
- Güvenli şifre hash'leme

## 🧪 Test

Uygulamayı test etmek için:

1. Uygulamayı çalıştırın
2. Tarayıcıda `https://localhost:5001` adresine gidin
3. Kayıt olun veya giriş yapın
4. Todo yönetimi özelliklerini test edin

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## 🤝 Katkıda Bulunma

1. Projeyi fork edin
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📞 İletişim

Proje hakkında sorularınız için issue açabilir veya iletişime geçebilirsiniz.

---

**Not**: Bu uygulama geliştirme amaçlıdır. Production ortamında kullanmadan önce güvenlik testleri yapılmalıdır.
