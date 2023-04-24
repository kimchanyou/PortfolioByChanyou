using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GemInfo
{
    // public을 붙이거나 [serializefield] 해야 한다.
    // Json 파일과 똑같은 이름의 변수를 사용해야한다.
    public int id;
    public float attack;
    public string name;
    public string spriteName;
}

[Serializable]
public class GemData : ILoader<int, GemInfo>
{
    public List<GemInfo> gemData = new List<GemInfo>();

    public Dictionary<int, GemInfo> MakeDict()
    {
        Dictionary<int, GemInfo> dict = new Dictionary<int, GemInfo>();
        foreach (GemInfo gem in gemData)
            dict.Add(gem.id, gem);
        return dict;
    }
}