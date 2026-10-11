# Öğrenci Not Sistemi

C# ile yazılmış bir konsol uygulaması: öğrencileri ve sınav notlarını kaydeder, ortalamaları hesaplar. Veriler bir JSON dosyasında saklanır; program kapatılıp yeniden açıldığında kayıtlar kaldığı yerden devam eder.

## Özellikler

- **Öğrenci ekleme:** Ad, soyad ve istenen sayıda sınav notu girilir.
- **Girdi doğrulama:** Sayı beklenen yere harf girilirse ya da not 0–100 aralığının dışındaysa program yeniden sorar.
- **Listeleme:** Her öğrencinin notları, ortalaması ve sınıf ortalaması gösterilir.
- **Kalıcı kayıt:** Veriler her yeni öğrenciden sonra `ogrenciler.json` dosyasına yazılır ve program açılırken otomatik yüklenir.

## Başlarken

**Gereksinim:** [.NET SDK](https://dotnet.microsoft.com/download) 6.0 veya üzeri (projenin hedeflediği sürüm `.csproj` dosyasındaki `TargetFramework` alanında yazar).

**Terminalden çalıştırma** — `.csproj` dosyasının bulunduğu klasörde:

```bash
dotnet run
```

**Visual Studio ile:** `.sln` dosyasını açıp `Ctrl + F5` ile çalıştırın.

## Kullanım

Program açılınca ana menü görünür:

```text
ANA MENÜ
1- Öğrenci Ekle
2- Listele
3- Çıkış
Seçiminizi giriniz:
```

| Seçim | Ne yapar? |
|:-----:|-----------|
| `1` | Yeni öğrenci ekler (ad, soyad, not sayısı ve notlar) |
| `2` | Kayıtlı öğrencileri notları, ortalamaları ve sınıf ortalamasıyla listeler |
| `3` | Programdan çıkar |

Bunların dışında bir sayı girilirse `Hatalı seçim yaptınız!` uyarısı gösterilir.

### Öğrenci ekleme

```text
Seçiminizi giriniz: 1

Ad: Ayşe
Soyad: Yılmaz
Kaç not gireceksiniz: 3
1. Sınav notunu giriniz: 85
2. Sınav notunu giriniz: 150
Not 0 ile 100 arasında olmalı!
2. Sınav notunu giriniz: 90
3. Sınav notunu giriniz: 78
```

Geçersiz bir not girildiğinde aynı sıradaki not yeniden istenir.

### Listeleme

İki öğrenci kayıtlıyken:

```text
Seçiminizi giriniz: 2

Ad: Ayşe
Soyad: Yılmaz
1. Sınav: 85
2. Sınav: 90
3. Sınav: 78
Ortalama: 84,33

Ad: Mehmet
Soyad: Demir
1. Sınav: 70
2. Sınav: 65
Ortalama: 67,50

Sınıf ortalaması: 75,92
```

- **Sınıf ortalaması**, öğrencilerin ortalamalarının ortalamasıdır: (84,33 + 67,50) / 2 = 75,92. Öğrencilerin not sayıları farklıysa bu değer, tüm notların ortalamasından (örnekte 77,60) farklı çıkar.
- Ondalık ayırıcı (virgül veya nokta) bilgisayarın bölge ayarlarına göre değişir.

### Girdi kuralları

- Menü seçimi, not sayısı ve notlar **tam sayı** olmalıdır; aksi hâlde `Lütfen bir sayı girin:` uyarısıyla yeniden sorulur.
- En az **1 not** girilmelidir.
- Her not **0–100** aralığında olmalıdır.

## Veri Saklama

Öğrenciler `ogrenciler.json` dosyasında tutulur:

```json
[{"Ad":"Ay\u015Fe","Soyad":"Y\u0131lmaz","Notlar":[85,90,78]},{"Ad":"Mehmet","Soyad":"Demir","Notlar":[70,65]}]
```

- Dosya, her öğrenci eklendiğinde güncellenir ve program açılırken (varsa) otomatik okunur.
- Dosya yolu göreli (`ogrenciler.json`) olduğundan dosya programın çalışma klasöründe oluşur: terminalde `dotnet run` komutunu verdiğiniz klasör, Visual Studio'da genellikle `bin/Debug/net.../` klasörü.
- Türkçe karakterler `\u015F` gibi kaçış dizileri olarak yazılır; program dosyayı okurken bunları doğru çözer.
- Kayıtları sıfırlamak için `ogrenciler.json` dosyasını silmeniz yeterlidir.
- Dosyayı elle düzenlerseniz JSON biçimini bozmamaya dikkat edin; geçersiz bir dosya programın açılışta hata vermesine yol açar.

## Kod Yapısı

```text
.
├── OgrenciNotSistemi.csproj
├── Program.cs        # Menü, kullanıcı girişi, listeleme, JSON kaydetme/yükleme
├── Ogrenci.cs        # Öğrenci sınıfı ve ortalama hesabı
└── ogrenciler.json   # Çalışırken otomatik oluşur
```

| Dosya | Üye | Görevi |
|-------|-----|--------|
| `Program.cs` | `Main` | Menüyü gösterir, seçime göre ilgili metodu çağırır; `3` seçilene kadar döngüde kalır |
| | `OgrenciEkle()` | Ad, soyad ve notları alır; öğrenciyi listeye ekleyip kaydeder |
| | `Listele()` | Öğrencileri, notlarını ve ortalamalarını yazdırır; sınıf ortalamasını hesaplar |
| | `SayiOku()` | Geçerli bir tam sayı girilene kadar tekrar sorar (`int.TryParse`) |
| | `Yukle()` / `Kaydet()` | `ogrenciler.json` dosyasını `System.Text.Json` ile okur / yazar |
| `Ogrenci.cs` | `Ogrenci` | `Ad`, `Soyad` ve `Notlar` (`List<int>`) özelliklerini tutar |
| | `Ortalama()` | Notların aritmetik ortalamasını döndürür; hiç not yoksa `0` |

Bu projede kullanılan başlıca kavramlar: sınıf ve özellikler (property), `List<T>`, `switch` / `while` / `foreach` yapıları, `int.TryParse` ile girdi doğrulama, `System.Text.Json` ile serileştirme, dosya işlemleri (`File` sınıfı) ve string interpolation.

## Geliştirme Fikirleri

- [ ] Öğrenci silme ve not güncelleme
- [ ] Ada göre arama ve ortalamaya göre sıralama
- [ ] Geçti/kaldı durumu veya harf notu hesaplama
- [ ] Ad/soyad doğrulaması ve bozuk JSON dosyası için hata yönetimi (`try-catch`)
- [ ] Okunaklı JSON çıktısı (`WriteIndented`) ve Türkçe karakterlerin dosyada düzgün görünmesi
