using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ExcelConfig;
using FGUFW;
using LitJson;
using UnityEngine;

public class GlobalConfig : MonoSingleton<GlobalConfig>
{
    public static GameConfigEC Configs => I.m_Configs;
    public GameConfigEC m_Configs;

    private IAssetLoader assetLoader = new Addressable_AssetLoader();

    protected override bool IsDontDestroyOnLoad()=>true;


    public void Load()
    {
        var textAsset = AssetHelper.Load<TextAsset>("Assets/ECJsonData/GameConfigEC.json");
        
        m_Configs = textAsset.text.ToObject<GameConfigEC>();
    } 
}
