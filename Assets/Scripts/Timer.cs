using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class Timer : MonoBehaviour
{
    private static Timer _instance;
    public static Timer Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        _instance = this;
    }





    [HideInInspector]
    public float currentTime = 0.0f;

    private TextMeshProUGUI tmPRO_timer;

    private bool canGo;

    // Start is called before the first frame update
    void Start()
    {
        tmPRO_timer = GetComponent<TextMeshProUGUI>();
    }
    [HideInInspector]
    public string minutes;
    [HideInInspector]
    public string seconds;

    // Update is called once per frame
    void Update()
    {
        if (canGo)
        {
            currentTime += Time.deltaTime;
            minutes = Mathf.Floor((currentTime % 3600) / 60).ToString("00");
            seconds = (currentTime % 60).ToString("00");
            tmPRO_timer.text = minutes + ":" + seconds;
        }
    }
    public void StartTime()
    {
        canGo = true;
    }
    public void StopTime()
    {
        canGo = false;
    }
}
