using mazda_5.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Mazda_5.Services
{
    public class DtcService
    {
        private readonly ObdService _obd;
        // moduły JSON
        private readonly Dictionary<string, string> pcm;
        private readonly Dictionary<string, string> abs;
        private readonly Dictionary<string, string> srs;
        private readonly Dictionary<string, string> bcm;
        private readonly Dictionary<string, string> ipc;
        private readonly Dictionary<string, string> network;

        private Dictionary<string, string> LoadJson(string path)
        {
            if (!File.Exists(path))
                return new Dictionary<string, string>();

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        public DtcService(ObdService obd)
        {
            _obd = obd;
        }

        public List<string> ReadDtc()
        {
            try
            {
                string raw = _obd.GetDtcRaw();   // używamy Twojej metody
                return ParseDtc(raw);
            }
            catch (Exception ex)
            {
                return new List<string> { $"Błąd odczytu DTC: {ex.Message}" };
            }
        }

        public bool ClearDtc()
        {
            try
            {
                _obd.ClearDtcRaw();   // używamy Twojej metody
                return true;
            }
            catch
            {
                return false;
            }
        }

        private List<string> ParseDtc(string raw)
        {
            var list = new List<string>();

            if (string.IsNullOrWhiteSpace(raw))
            {
                list.Add("Brak odpowiedzi z ECU");
                return list;
            }

            raw = raw.Replace(" ", "").Replace("\n", "").Replace("\r", "");

            if (!raw.StartsWith("43"))
            {
                list.Add("Brak błędów");
                return list;
            }

            raw = raw.Substring(2);

            for (int i = 0; i < raw.Length; i += 4)
            {
                if (i + 4 > raw.Length) break;

                string codeHex = raw.Substring(i, 4);
                string dtc = ConvertDtc(codeHex);

                string desc = GetDescription(dtc);

                
                list.Add($"{dtc} - {desc}");
            }

            return list;
        }
        private string GetDescription(string code)
        {
            code = code.ToUpper();
            char prefix = code[0];

            return prefix switch
            {
                'P' => pcm.TryGetValue(code, out var p) ? p : "Nieznany kod DTC",
                'C' => abs.TryGetValue(code, out var c) ? c : "Nieznany kod DTC",
                'B' => GetBodyDescription(code),
                'U' => network.TryGetValue(code, out var u) ? u : "Nieznany kod DTC",
                _ => "Nieznany kod DTC"
            };
        }

        private string GetBodyDescription(string code)
        {
            if (srs.TryGetValue(code, out var srsDesc)) return srsDesc;
            if (bcm.TryGetValue(code, out var bcmDesc)) return bcmDesc;
            if (ipc.TryGetValue(code, out var ipcDesc)) return ipcDesc;

            return "Nieznany kod DTC";
        }
        private string ConvertDtc(string hex)
        {
            int b1 = Convert.ToInt32(hex.Substring(0, 2), 16);
            int b2 = Convert.ToInt32(hex.Substring(2, 2), 16);

            int type = (b1 & 0xC0) >> 6;
            char prefix = type switch
            {
                0 => 'P',
                1 => 'C',
                2 => 'B',
                3 => 'U',
                _ => 'P'
            };

            int code = ((b1 & 0x3F) << 8) | b2;

            return $"{prefix}{code:D4}";
        }
    }
}
