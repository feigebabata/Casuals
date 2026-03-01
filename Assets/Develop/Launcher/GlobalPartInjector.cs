using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FGUFW;
using FGUFW.Gameplay;
using UnityEngine;

public class GlobalPartInjector : PartFieldInjector
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
        defaultFieldDatas.Add(typeof(IAssetLoader),assetLoader);
        defaultFieldDatas.Add(typeof(IOrderedMessenger<string>),GlobalMessenger.M);
        defaultFieldDatas.Add(typeof(ILoadingUI),Loading);

        addConfigToDefault();
    }

    private void addConfigToDefault()
    {
        GlobalConfig.I.Load();
        var configs = GlobalConfig.Configs;

        foreach (FieldInfo f_info in configs.GetType().GetFields (BindingFlags.Instance | BindingFlags.Public  | BindingFlags.NonPublic))
        {
            defaultFieldDatas.Add(f_info.FieldType,f_info.GetValue(configs));
        }
    }

    protected override void injectSaveField(Part part, FieldInfo f_info)
    {
        var type = f_info.FieldType; 

        if(!type.ContainsAttribute<SerializableAttribute>())
        {
            Debug.LogError($"{type.Name}必须添加特性[Serializable]");
        }

        var data = PartConfigUtility.Get(type);

        f_info.SetValue(part,data);
    }

    protected override async UniTask injectUIField(Part part, FieldInfo f_info, CancellationToken partTaskCancellationToken)
    {
        var type = f_info.FieldType; 
        var key = type.Name;

        var ui = await assetLoader.CopyAsync(key,default,partTaskCancellationToken);
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