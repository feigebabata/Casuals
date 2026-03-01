using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FGUFW;
using FGUFW.Gameplay;
using Lobby;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    void Start()
    {
        GlobalLoading.I.Show();

        AssetHelper.LoadSceneAsync("Assets/Develop/Lobby/Lobby.unity");
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
