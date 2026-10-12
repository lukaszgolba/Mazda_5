using System;
using System.Collections.Generic;

namespace Mazda_5.Services
{
    public static class VinDecoder
    {
        private static readonly Dictionary<string, string> ManufacturerMap = new()
        {
            { "JM1", "Mazda (Japan)" },
            { "JM3", "Mazda SUV (Japan)" },
            { "JMB", "Mazda (Japan)" },
            { "JSA", "Suzuki (Japan)" },
            { "JTD", "Toyota (Japan)" },
            { "1F", "Ford (USA)" },
            { "1G", "General Motors (USA)" },
            { "3M", "Mazda (Mexico)" },
            { "JMZ", "Mazda (Japonia)" }
        };

        private static readonly Dictionary<char, int> YearMap = new()
        {
            { 'A', 2010 }, { 'B', 2011 }, { 'C', 2012 }, { 'D', 2013 },
            { 'E', 2014 }, { 'F', 2015 }, { 'G', 2016 }, { 'H', 2017 },
            { 'J', 2018 }, { 'K', 2019 }, { 'L', 2020 }, { 'M', 2021 },
            { 'N', 2022 }, { 'P', 2023 }, { 'R', 2024 }, { 'S', 2025 },
            { 'T', 2026 }, { '5', 2005 }, { '6', 2006 }, { '7', 2007 },
            { '8', 2008 }, { '9', 2009 }
        };
        private static readonly Dictionary<string, string> MazdaModelMap = new()
        {
             // Mazda 3
            { "BK", "Mazda 3 BK (2003–2009)" },
            { "BL", "Mazda 3 BL (2009–2013)" },
            { "BM", "Mazda 3 BM (2013–2019)" },
            { "BP", "Mazda 3 BP (2019–)" },

            // Mazda 6
            { "GG", "Mazda 6 GG (2002–2008)" },
            { "GH", "Mazda 6 GH (2007–2012)" },
            { "GJ", "Mazda 6 GJ (2012–2023)" },
            { "GL", "Mazda 6 GL (facelift 2018–2023)" },

            // CX-5
            { "KE", "Mazda CX-5 KE (2012–2017)" },
            { "KF", "Mazda CX-5 KF (2017–)" },

            // CX-7 / CX-9
            { "ER", "Mazda CX-7 ER (2006–2012)" },
            { "TB", "Mazda CX-9 TB (2006–2015)" },
            { "TC", "Mazda CX-9 TC (2016–)" },

            // Mazda 2
            { "DE", "Mazda 2 DE (2007–2014)" },
            { "DJ", "Mazda 2 DJ (2014–)" },

            // MX-5
            { "NC", "Mazda MX-5 NC (2005–2015)" },
            { "ND", "Mazda MX-5 ND (2015–)" },

            // Mazda 5
            { "CR", "Mazda 5 CR (2005–2010)" },
            { "CW", "Mazda 5 CW (2010–2015)" }
        };



        public static VinInfo Decode(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin) || vin.Length != 17)
                return new VinInfo { Error = "Nieprawidłowy VIN" };

            string wmi = vin.Substring(0, 3);
            string vds = vin.Substring(3, 6);
            string vis = vin.Substring(9, 8);

            string manufacturer = ManufacturerMap.ContainsKey(wmi) ? ManufacturerMap[wmi] : "Nieznany producent";

            char yearCode = vin[9];
            int year = YearMap.ContainsKey(yearCode) ? YearMap[yearCode] : -1;

            return new VinInfo
            {
                VIN = vin,
                Manufacturer = manufacturer,
                VehicleDescriptor = vds,
                VehicleIdentifier = vis,
                ModelYear = year,
                Model = DecodeMazdaModel(vds),
                Error = null
            };
        }
        private static string DecodeMazdaModel(string vds)
        {
            if (string.IsNullOrWhiteSpace(vds) || vds.Length < 2)
                return "Nieznany model";

            // Model Mazdy to zwykle pierwsze 2 znaki VDS
            string code = vds.Substring(0, 2).ToUpper();

            return MazdaModelMap.ContainsKey(code)
                ? MazdaModelMap[code]
                : "Nieznany model";
        }





    }

    public class VinInfo
    {
        public string VIN { get; set; }
        public string Manufacturer { get; set; }
        public string VehicleDescriptor { get; set; }
        public string VehicleIdentifier { get; set; }
        public int ModelYear { get; set; }
        public string Error { get; set; }
        public string Model { get; set; }
    }
}
