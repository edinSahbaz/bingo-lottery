﻿using System;

namespace BingoLotteryConsole;

public static class Izvlacenje
{
    // ISSUE # 5
   public static List<int[]> ProcitajTxtFajl()
   {
        List<int[]> listici = new List<int[]>();

        // Otvaranje txt datoteke i čitanje linija teksta
        string[] linije = File.ReadAllLines("listici.txt");

        // Parsiranje brojeva za svaki listić
        foreach (string linija in linije)
        {
            if (!linija.Contains("Listic"))
            {
                continue;
            }

            var pozicijaKaraktera = linija.IndexOf(':');

            var linijaBrojeva = linija.Substring(pozicijaKaraktera + 2);
            int[] brojevi = Array.ConvertAll(linijaBrojeva.Split(','), int.Parse);

            // Provjera da li se na listiću nalazi tačno 6 brojeva
            if (brojevi.Length == 6)
            {
                listici.Add(brojevi);
            }
        }

        return listici;
    }

    // ISSUE # 6
    // https://www.lutrijabih.ba/igre/loto-639/privremeni-izvjestaj/
    public static void GenerisiIzvjestaj(List<int[]> listici, int[] dobitnaKombinacija, int dopunskiBroj)
    {
        // Dobitne kombinacje predstavljenje kao key value parovi
        // Key predstavlja broj pogodaka, dok db označava dopunski broj
        var kombinacije = new Dictionary<string, int> {
            {"6", 0}, {"5db", 0}, {"5", 0}, {"4db", 0}, {"4", 0}, {"3db", 0}, {"3", 0}, {"2db", 0}
        };

        // Provjera pogodaka i dopunskog broja za sve listice
        foreach (var listic in listici)
        {
            int brojPogodaka = 0;
            bool sadrziDopunskiBroj = listic.Contains(dopunskiBroj);

            foreach (var broj in listic)
            {
                if (dobitnaKombinacija.Contains(broj))
                {
                    brojPogodaka++;
                }
            }

            if (brojPogodaka < 2) continue;
            if (brojPogodaka == 2 && !sadrziDopunskiBroj) continue;

            var kljuc = brojPogodaka.ToString() + (sadrziDopunskiBroj && brojPogodaka != 6 ? "db" : "");
            kombinacije[kljuc] = kombinacije[kljuc] + 1;
        }

        // Generisanje izvjestaja
        string izvjestaj = $"Sestice: {kombinacije["6"]}\nPetice + dopunski broj: {kombinacije["5db"]}" +
            $"\nPetice: {kombinacije["5"]}\nCetvorke + dopunski broj: {kombinacije["4db"]}\nCetvorke: {kombinacije["4"]}" +
            $"\nTrojke + dopunski broj: {kombinacije["3db"]}\nTrojke: {kombinacije["3"]}\nDvojke + dopunski broj: {kombinacije["2db"]}";

        Console.WriteLine(izvjestaj);

        // Spremanje izvjestaja u izvjestaj.txt fajl
        using (StreamWriter writer = new StreamWriter("izvjestaj.txt"))
        {
            writer.WriteLine(izvjestaj);
        }
    }
}