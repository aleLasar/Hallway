using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayActionBtn()
    {
        var extensions = new[] {
            new ExtensionFilter("Json", "json")
        };

        string[] path = StandaloneFileBrowser.OpenFilePanel("Choose the score", "", extensions, false);
        if (path.Length > 0)
        {
            Helper.PathScore = path[0];
            SceneManager.LoadScene(1);
        }
    }

    public void ReturnToMenù()
    {
        SceneManager.LoadScene(0);
    }
}
