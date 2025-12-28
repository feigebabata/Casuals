using System.Collections;
using ExcelConfig;
using FGUFW;
using LitJson;
using UnityEngine;

public class GlobalConfig : MonoSingleton<GlobalConfig>
{
    public static GameConfigEC Configs => I.m_Configs;
    public GameConfigEC m_Configs;

    protected override bool IsDontDestroyOnLoad()=>true;


    public IEnumerator Load()
    {
        var loader = AssetHelper.LoadAsync<TextAsset>("Assets/ECJsonData/GameConfigEC.json");
        yield return loader;
        m_Configs = loader.Result.text.ToObject<GameConfigEC>();
        yield return default;
    } 
}
