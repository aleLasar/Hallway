using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTextController : MonoBehaviour
{
    private TextMeshProUGUI _TextLabel;

    private List<string> _Scores = new List<string>();

    private void Awake()
    {
        _TextLabel = GetComponent<TextMeshProUGUI>();


        int i = 1;
        foreach(string key in Helper.PlayerScores.Keys)
        {
            _Scores.Add($"Play {i}: {Helper.PlayerScores[key]}");
            i++;
        }

        if (Helper.PlayerScores.Keys.Count == 0)
            _Scores.Add("Nobody won");
    }

    // Start is called before the first frame update
    void Start()
    {
        _TextLabel.text = string.Join(" \n", _Scores);
    }


}
