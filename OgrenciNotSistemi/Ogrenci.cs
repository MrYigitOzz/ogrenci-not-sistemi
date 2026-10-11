using System;
using System.Collections.Generic;
using System.Text;

namespace OgrenciNotSistemi
{
    internal class Ogrenci
    {
        public string Ad { get; set; } = "";
        public string Soyad { get; set; } = "";
        public List<int> Notlar { get; set; } = new();

        public double Ortalama()
        {
            int toplam = 0;

            if (Notlar.Count == 0)
                return 0;

            foreach (int n in Notlar)
                toplam += n;

            return toplam / (double)Notlar.Count;
        }
    }
}
