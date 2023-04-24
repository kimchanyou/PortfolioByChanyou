using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GemInfo
{
    // public�� ���̰ų� [serializefield] �ؾ� �Ѵ�.
    // Json ���ϰ� �Ȱ��� �̸��� ������ ����ؾ��Ѵ�.
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