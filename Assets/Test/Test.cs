using System.Collections;
using System.IO;
using System.Text;
using FGUFW;
using UnityEngine;
using UnityEngine.Networking;

public class Test : MonoBehaviour
{

    // https://ncode.syosetu.com/n6169dz/478/

    IEnumerator Start()
    {
        for (int i = 478; i < 480; i++)
        {
            var uwr = new UnityWebRequest($"https://ncode.syosetu.com/n6169dz/{i}");
            uwr.downloadHandler = new DownloadHandlerBuffer();
            yield return uwr.SendWebRequest();

            Debug.Log(uwr.downloadHandler.text);
        }
    }

}
