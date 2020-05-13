using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class HighScoreManager : MonoBehaviour
{
    private static HighScoreManager _instance;
    public static HighScoreManager Instance
    {
        get { return _instance; }
    }

    public GameObject InputTyperPanel;

    public TextMeshProUGUI nick1;
    public TextMeshProUGUI nick2;
    public TextMeshProUGUI nick3;

    public TextMeshProUGUI score1;
    public TextMeshProUGUI score2;
    public TextMeshProUGUI score3;

    public TMP_InputField inputField;
    [HideInInspector]
    public int currentPlayerScore;
    [Space]
    [Header("Left side score and time:")]
    public TextMeshProUGUI leftSideScore;
    public TextMeshProUGUI leftSideTime;

    private void Awake()
    {
        _instance = this;
    }
    
    private void OnEnable()
    {
        currentPlayerScore = MasterController.Instance.tempScore;

        GetValuesFromPrefs();

        var se = new TMP_InputField.SubmitEvent();
        se.AddListener(SubmitName);
        inputField.onEndEdit = se;

        //Set left side score and time:
        leftSideScore.text = currentPlayerScore.ToString();
        leftSideTime.text = Timer.Instance.minutes + "" + Timer.Instance.seconds;
    }


    void GetValuesFromPrefs()
    {        
        nick1.text = PlayerPrefs.GetString("nick1");
        nick2.text = PlayerPrefs.GetString("nick2");
        nick3.text = PlayerPrefs.GetString("nick3");

        score1.text = PlayerPrefs.GetInt("score1").ToString();
        score2.text = PlayerPrefs.GetInt("score2").ToString();
        score3.text = PlayerPrefs.GetInt("score3").ToString();
    }
    void SubmitName(string name)
    {
        InputTyperPanel.SetActive(false);
        UpdateHighscores(name);
    }
    void UpdateHighscores(string name)
    {
        int s1 = int.Parse(score1.text);
        int s2 = int.Parse(score2.text);
        int s3 = int.Parse(score3.text);

        //If any of positions is empty:
        if (score1.text == "") SetPlayerOnHighscoreList(1, name);
        else if (score2.text == "") SetPlayerOnHighscoreList(2, name);
        else if (score3.text == "") SetPlayerOnHighscoreList(3, name);
        else //If there are 3 entries already:
        {
            if (currentPlayerScore < s3) return;//Player score is too low.
            else if(currentPlayerScore >= s3 && currentPlayerScore < s2)
            {
                SetPlayerOnHighscoreList(3, name);
            }
            else if(currentPlayerScore >= s2 && currentPlayerScore < s1)
            {
                SetPlayerOnHighscoreList(2, name);
            }
            else if(currentPlayerScore > s1)
            {
                SetPlayerOnHighscoreList(1, name);
            }
        }
    }
    void SetPlayerOnHighscoreList(int position, string name)
    {
        switch(position)
        {
            case 1:
                nick1.text = name;
                score1.text = currentPlayerScore.ToString();
                PlayerPrefs.SetInt("score1", int.Parse(score1.text));
                PlayerPrefs.SetString("nick1", name);
                PlayerPrefs.Save();
                break;
            case 2:
                nick2.text = name;
                score2.text = currentPlayerScore.ToString();
                PlayerPrefs.SetInt("score2", int.Parse(score2.text));
                PlayerPrefs.SetString("nick2", name);
                PlayerPrefs.Save();
                break;
            case 3:
                nick3.text = name;
                score3.text = currentPlayerScore.ToString();
                PlayerPrefs.SetInt("score3", int.Parse(score3.text));
                PlayerPrefs.SetString("nick3", name);
                PlayerPrefs.Save();
                break;          
        }
    }
}
