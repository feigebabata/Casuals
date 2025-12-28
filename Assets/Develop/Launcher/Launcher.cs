using System.Collections;
using FGUFW;
using FGUFW.MonoGameplay;
using Lobby;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    IEnumerator Start()
    {
        GlobalLoading.I.Show();
        yield return GlobalConfig.I.Load();
        Part.Create<LobbyPlay>(default).OnCreating(default,default).StartByGCS();
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Launcher/Start")]
    static void launcher()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Develop/Launcher/Launcher.unity");
        UnityEditor.EditorApplication.EnterPlaymode();
    }
#endif

}
