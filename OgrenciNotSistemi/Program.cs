using System.Text.Json;

namespace OgrenciNotSistemi
{
    internal class Program
    {
        static List<Ogrenci> ogrenciler = new();
        const string DosyaYolu = "ogrenciler.json";
        static void Main(string[] args)
        {
            Yukle();

            int secim = 0;

            while (secim != 3)
            {
                Console.WriteLine("ANA MENÜ\n1- Öğrenci Ekle\n2- Listele\n3- Çıkış");
                Console.Write("Seçiminizi giriniz: ");
                secim = SayiOku();
                Console.WriteLine();

                switch (secim)
                {
                    case 1:
                        OgrenciEkle();
                        break;

                    case 2:
                        Listele();
                        break;

                    case 3:
                        Console.WriteLine("Çıkış Yapıldı.");
                        break;

                    default:
                        Console.WriteLine("Hatalı seçim yaptınız!\n");
                        break;
                }
            }
        }

        static void OgrenciEkle()
        {
            Ogrenci ogr = new();

            Console.Write("Ad: ");
            ogr.Ad = Console.ReadLine() ?? "";
            Console.Write("Soyad: ");
            ogr.Soyad = Console.ReadLine() ?? "";

            Console.Write("Kaç not gireceksiniz: ");
            int notSayisi = SayiOku();

            while (notSayisi <= 0)
            {
                Console.WriteLine("En az 1 not girmelisiniz.");
                Console.Write("Kaç not gireceksiniz: ");
                notSayisi = SayiOku();
            }

            int say = 1;

            while (say <= notSayisi)
            {
                Console.Write($"{say}. Sınav notunu giriniz: ");
                int girilenNot = SayiOku();

                if (girilenNot >= 0 && girilenNot <= 100)
                {
                    ogr.Notlar.Add(girilenNot);
                    say++;
                }

                else
                    Console.WriteLine("Not 0 ile 100 arasında olmalı!");
            }

            Console.WriteLine();
            ogrenciler.Add(ogr);
            Kaydet();
        }

        static void Listele()
        {
            double toplam = 0;

            if (ogrenciler.Count == 0)
                Console.WriteLine("Henüz öğrenci yok\n");

            else
            {
                foreach (var ogr in ogrenciler)
                {
                    double ort = ogr.Ortalama();

                    Console.WriteLine($"Ad: {ogr.Ad}\nSoyad: {ogr.Soyad}");
                    for (int i = 0; i < ogr.Notlar.Count; i++)
                        Console.WriteLine($"{i + 1}. Sınav: {ogr.Notlar[i]}");
                    Console.WriteLine($"Ortalama: {ort:F2}\n");
                    toplam += ort;
                }

                Console.WriteLine($"Sınıf ortalaması: {toplam / ogrenciler.Count:F2}\n");
            }
        }

        static int SayiOku()
        {
            int sayi;
            while (!int.TryParse(Console.ReadLine(), out sayi))
                Console.Write("Lütfen bir sayı girin: ");
            return sayi;
        }

        static void Yukle()
        {
            if (!File.Exists(DosyaYolu)) return;
            // ogrenciler = JsonSerializer.Deserialize<List<Ogrenci>>(File.ReadAllText(DosyaYolu)) ?? new();
            string json = File.ReadAllText(DosyaYolu);
            ogrenciler = JsonSerializer.Deserialize<List<Ogrenci>>(json) ?? new();
        }

        static void Kaydet()
        {
            // File.WriteAllText("ogrenciler.json", JsonSerializer.Serialize(ogrenciler));
            string json = JsonSerializer.Serialize(ogrenciler);
            File.WriteAllText(DosyaYolu, json);
        }
    }
}
