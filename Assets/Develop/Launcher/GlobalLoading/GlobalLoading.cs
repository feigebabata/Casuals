using System.Collections;
using FGUFW;
using UnityEngine;
using UnityEngine.UI;

public class GlobalLoading : MonoSingleton<GlobalLoading>,ILoadingUI
{
    public Image Mask;

    protected override bool IsDontDestroyOnLoad()=>true;

    protected override void Init()
    {
        base.Init();

        gameObject.SetActive(false);
    }

    public void Show()
    {
        if(gameObject.activeSelf)return;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        if(!gameObject.activeSelf)return;
        hide(1f).StartByGCS();
    }

    IEnumerator hide(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);

    }

}
