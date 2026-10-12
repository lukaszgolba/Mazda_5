using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class DtcDatabase
{
    private readonly Dictionary<string, string> pcm;
    private readonly Dictionary<string, string> abs;
    private readonly Dictionary<string, string> srs;
    private readonly Dictionary<string, string> bcm;
    private readonly Dictionary<string, string> ipc;
    private readonly Dictionary<string, string> network;

    public DtcDatabase()
    {
        pcm = LoadJson("dtc/dtc_pcm.json");
        abs = LoadJson("dtc/dtc_abs.json");
        srs = LoadJson("dtc/dtc_srs.json");
        bcm = LoadJson("dtc/dtc_bcm.json");
        ipc = LoadJson("dtc/dtc_ipc.json");
        network = LoadJson("dtc/dtc_network.json");
    }

    private Dictionary<string, string> LoadJson(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
    }

    public string GetDescription(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return "Nieznany kod DTC";

        code = code.ToUpper();
        char prefix = code[0];

        return prefix switch
        {
            'P' => pcm.TryGetValue(code, out var pDesc) ? pDesc : "Nieznany kod DTC",
            'C' => abs.TryGetValue(code, out var cDesc) ? cDesc : "Nieznany kod DTC",
            'B' => GetBodyDescription(code),
            'U' => network.TryGetValue(code, out var uDesc) ? uDesc : "Nieznany kod DTC",
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
}
