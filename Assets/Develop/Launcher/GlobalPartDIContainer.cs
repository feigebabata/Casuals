using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FGUFW;
using FGUFW.Gameplay;
using UnityEngine;

public class GlobalPartDIContainer : PartDIContainer
{
    private IAssetLoader assetLoader = new Addressable_AssetLoader();

    public GlobalLoading Loading;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        addDefaultFieldDatas();
    }

    private void addDefaultFieldDatas()
    {
        defaultDependency.Add(typeof(IAssetLoader),assetLoader);
        defaultDependency.Add(typeof(IOrderedMessenger<string>),GlobalMessenger.M);
        defaultDependency.Add(typeof(ILoadingUI),Loading);

        addConfigToDefault();
    }

    private void addConfigToDefault()
    {
        GlobalConfig.I.Load();
        var configs = GlobalConfig.Configs;

        foreach (FieldInfo f_info in configs.GetType().GetFields (BindingFlags.Instance | BindingFlags.Public  | BindingFlags.NonPublic))
        {
            defaultDependency.Add(f_info.FieldType,f_info.GetValue(configs));
        }
    }


    protected override async UniTask injectUIField(Part part, FieldInfo f_info, CancellationToken partTaskCancellationToken)
    {
        var type = f_info.FieldType; 
        var key = type.Name;

        var ui = await assetLoader.InstantiateAsync(key,default,partTaskCancellationToken);
        DontDestroyOnLoad(ui);
        ui.SetActive(false);

        var fieldData = ui.GetComponent(type);
        if(fieldData==default)
        {
            Debug.LogError($"{key}不包含{type}");
        }

        f_info.SetValue(part,fieldData);
    }

}