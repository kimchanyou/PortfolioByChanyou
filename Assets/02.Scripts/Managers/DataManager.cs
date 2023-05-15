using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ILoader<key, value>
{
    Dictionary<key, value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, GemInfo> dicGemData { get; private set; } = new Dictionary<int, GemInfo>();

    public void Init()
    {
        dicGemData = LoadJson<GemData, int, GemInfo>("GemData").MakeDict();
    }

    Loader LoadJson<Loader, key, value>(string path) where Loader : ILoader<key, value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
