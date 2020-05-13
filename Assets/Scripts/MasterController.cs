using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649 

//Will also play music.
public class MasterController : MonoBehaviour
{
    private static MasterController _instance;
    public static MasterController Instance
    {
        get { return _instance; }
    }


    [SerializeField]
    private GameObject HUD_canvas;
    [SerializeField]
    GameObject ENDGAME_canvas;
    [SerializeField]
    GameObject TUTORIAL_canvas;
    Animator tutorial_canvas_anim;

    //Sounds
    [SerializeField]
    AudioSource fx_aSource;

    public AudioClip hitSound;
    public AudioClip endGameSound;
    public AudioClip scoreSound;

    [HideInInspector]
    public int tempScore;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        tutorial_canvas_anim = TUTORIAL_canvas.GetComponent<Animator>();


        if(PlayerPrefs.GetInt("TutorialDone") == 0) //if the tutorial has not been shown
        {
            tutorial_canvas_anim.SetTrigger("in");
            Cursor.visible = true;
        }
        else //if tutorial has already been shown - start playing immediately
        {
            Spawner.Instance.canSpawn = true;
            Invoke("SpawnFirstBlock", 1);
            Cursor.visible = false;
        }
    }

    void SpawnFirstBlock()
    {
        Spawner.Instance.SpawnNewBlock();
        Timer.Instance.StartTime();
    }//Invoked.

    
    public void EndGame()
    {
        Spawner.Instance.canSpawn = false;
        PlaySound(endGameSound);
        Camera.main.GetComponent<Orbit>().enabled = false;
        Cursor.visible = true;

        HUD_canvas.SetActive(false);
        ENDGAME_canvas.SetActive(true);
    }

    //BUTTONS:-----------------------------------------------
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Play_tut_acc()
    {       
        tutorial_canvas_anim.SetTrigger("out");
        Spawner.Instance.canSpawn = true;
        Spawner.Instance.SpawnNewBlock();
        Timer.Instance.StartTime();
        Cursor.visible = true;

        PlayerPrefs.SetInt("TutorialDone", 1);
        PlayerPrefs.Save();
    }
    //------------------------------------------------------

    public void PlaySound(AudioClip audioClip)
    {
        fx_aSource.PlayOneShot(audioClip);
    }
}
