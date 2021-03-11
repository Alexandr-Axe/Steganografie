using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Steganografie
{
    class Program
    {
        //Pokud chceme otestovat obě varianty, musíme v argumentech příkazového řádku změnit následující text :
        //stega --hide "ZasifrovatText" "test.png" = natvrdo vybraný text, který chceme zašifrovat do natvrdo vybraného obrázku, text musí být bez háčků a čárek
        //stega --show "test_zakodovano.png" = natvrdo vybraný obrázek, ze kterého chceme dosta zašifrovaný text
        //Program funguje s jakýmkoliv obrázkem formátu PNG. Stačí, aby se jmenoval test.png

        static void Main(string[] args)
        {
            string Zasifrovat = args[2]; //Zpráva, kterou chceme zašifrovat
            Color PixelObrazku;
            if (args[1] == "--hide")
            {
                if (args[2].Length != 0)
                {
                    if (args[3].Contains(".png"))
                    {
                        Bitmap Obrazek = new Bitmap("test.png"); //Převedení obrázku ze zadané adresy na bitmapu
                        for (int i = 0; i < Obrazek.Width; i++) //Šířka obrázku
                        {
                            for (int j = 0; j < Obrazek.Height; j++) //Výška obrázku
                            {
                                PixelObrazku = Obrazek.GetPixel(i, j); //Získám hodnotu jednotlivých barevných složek z každého pixelu obrázku
                                if (i < 1 && j < Zasifrovat.Length)
                                {
                                    char Znak = Convert.ToChar(Zasifrovat.Substring(j, 1)); //Vybrání jednoho znaku ze zašifrované zprávy, každým cyklem se vybere další písmeno, 1 kvůli jednomu znaku
                                    int HodnotaZnaku = Convert.ToInt32(Znak); //Převedení znaku na číslo, které budeme 
                                    Obrazek.SetPixel(i, j, Color.FromArgb(PixelObrazku.R, PixelObrazku.G, HodnotaZnaku)); //Zavedení hodnoty pixelu na pozici, ze které jsme ji vyjmuli. Dle konvence se mění modrá barva
                                }
                                else if (i == Obrazek.Width - 1 && j == Obrazek.Height - 1) //Pozice posledního pixelu na obrázku
                                    Obrazek.SetPixel(i, j, Color.FromArgb(PixelObrazku.R, PixelObrazku.G, Zasifrovat.Length)); //Uložení délky zprávy do posledního pixelu
                            }
                        }
                        Obrazek.Save(args[3]); //Uložení nového obrázku
                    }
                }
            }
            else if (args[1].Contains("--show"))
            {
                if (args[2].Contains(".png"))
                {
                    Bitmap ZasifrovanyObrazek = new Bitmap(args[2]); //Převedení obrázku ze zadané adresy na bitmapu
                    Color PosledniPixel = ZasifrovanyObrazek.GetPixel(ZasifrovanyObrazek.Width - 1, ZasifrovanyObrazek.Height - 1); //Pozice posledního pixelu, odečítá se 1 kvůli tomu, že pole začína na 0
                    Color PixelZasifrovanehoObrazku;
                    int DelkaZpravy = PosledniPixel.B; //Zjištění délky zprávy z hodnoty modré barvy úplně posledního pixelu
                    string VyslednaZprava = string.Empty; //string, do kterého budeme ukládat zašifrovanou zprávu
                    for (int i = 0; i < ZasifrovanyObrazek.Width; i++) //Šířka obrázku
                    {
                        for (int j = 0; j < ZasifrovanyObrazek.Height; j++) //Výška obrázku
                        {
                            PixelZasifrovanehoObrazku = ZasifrovanyObrazek.GetPixel(i, j); //Zjištění barev pixelu, který je na řadě
                            if (i < 1 && j < DelkaZpravy)
                            {
                                int HodnotaZasifrovanehoZnaku = PixelZasifrovanehoObrazku.B; //Zjištění hodnoty zašifrovaného znaku
                                char ZasifrovanyZnak = Convert.ToChar(HodnotaZasifrovanehoZnaku); //Převedení hodnoty zašifrovaného znaku na znak
                                string PrevedenyZnak = System.Text.Encoding.ASCII.GetString(new byte[] { Convert.ToByte(ZasifrovanyZnak) });
                                VyslednaZprava += PrevedenyZnak; //Ukládání jednotlivých znaků do stringu
                            }
                        }
                    }
                    Console.WriteLine(VyslednaZprava); //Vypsání zprávy
                    Console.ReadKey();
                }
            }
        }
    }
}
