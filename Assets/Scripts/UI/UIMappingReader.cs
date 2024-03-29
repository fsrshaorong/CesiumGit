using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UIMappingReader
{
    Dictionary<string, Dictionary<string, string>> uiCustom;

    public void ReadMapping(string filePath)
    {
        string uimapping = File.ReadAllText(filePath);
        ResolveMapping(uimapping);
    }

    public void ResolveMapping(string json)
    {
        uiCustom = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
    }

    public Dictionary<string, string> getCustiomData(string key)
    {
        return uiCustom[key];
    }
}
