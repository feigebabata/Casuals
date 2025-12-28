using System.IO;
using System.Text;
using FGUFW;
using UnityEngine;

[ExecuteInEditMode]
public class Test : MonoBehaviour
{

    void OnEnable()
    {
        var path = Path.Combine(Application.dataPath, "in.txt");

        var outText = new StringBuilder();

        var lines = File.ReadAllLines(path);

        foreach (var item in lines)
        {
            var line = item.Trim();
            if (line.StartsWith('<'))
            {
                continue;
            }
            outText.AppendLine(line);
            outText.AppendLine();
        }

        path = Path.Combine(Application.dataPath, "out.txt");

        File.WriteAllText(path, outText.ts());

        Debug.Log("执行结束");

    }

}
