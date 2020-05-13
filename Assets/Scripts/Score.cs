using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static int score;

    static TextMeshProUGUI tmUgui;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        tmUgui = GetComponent<TextMeshProUGUI>();
    }

    
    public static void AddScore(int amount)
    {
        score += amount;
        tmUgui.text = score.ToString();
    }
    private void OnDisable()
    {
        MasterController.Instance.tempScore = score;
    }
}
