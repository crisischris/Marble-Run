using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class logic_UI : MonoBehaviour
{
    public bool DEBUG = true;

    public Text fpsText;
    public Text touchCount;
    public Text speedText;

    //public AudioSource source;
    //public AudioClip clip;

    static public GameObject menu;

    static public bool clicked = false;


    public Text ScoreBoard;
    public Text highscore;

    //this var helps conform the FPS
    float deviceMultiplier;

    public float deltaTime;

    public int score_int;

    // Start is called before the first frame update
    private void Awake()
    {

        //catch android OS since it has an issue w/ FPS and refresh
        if(Application.platform == RuntimePlatform.Android)
        {
            deviceMultiplier = 1f;
        }
        else
            deviceMultiplier = 1;
    }
    void Start()
    {
        menu = GameObject.Find("menu");

        deltaTime = 0;

        menu.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        fpsText.transform.position = new Vector2(Screen.width/2, Screen.height - Screen.height/4);
        touchCount.transform.position = new Vector2(Screen.width/2, Screen.height - Screen.height/4 - 100);
        speedText.transform.position = new Vector2(Screen.width / 2, Screen.height - Screen.height / 4 - 200);


        ScoreBoard.transform.position = new Vector2(Screen.width/2, Screen.height - Screen.height/10);
        highscore.transform.position = new Vector2(Screen.width / 2, Screen.height - 100);
    }

    // Update is called once per frame
    void Update()
    {
        if(!DEBUG)
        {
            fpsText.enabled = false;
            speedText.enabled = false;
            touchCount.enabled = false;
        }

        else
        {
            fpsText.enabled = true;
            speedText.enabled = true;
            touchCount.enabled = true;
        }

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "fps: " + Mathf.Ceil(fps).ToString();
        touchCount.text = "touch count: " + Input.touchCount;
        ScoreBoard.text = "Score: " + hero.SCORE.ToString();
        highscore.text = "HighScore: " + doNotDestroy.HS.ToString();
        speedText.text = "speed: " + conveyor.speed;

    }

    public void startGame()
    {
        clicked = true;
        clicked = false;
        SceneManager.LoadScene("main");
    }


    public void maniac()
    {
        conveyor.init_speed = 1.75f * deviceMultiplier;
        conveyor.max_speed = 5f * deviceMultiplier;
        startGame();
    }

    public void hard()
    {
        conveyor.init_speed = 1f * deviceMultiplier;
        conveyor.max_speed = 3f * deviceMultiplier;
        startGame();
    }

    public void medium()
    {
        conveyor.init_speed = .75f * deviceMultiplier;
        conveyor.max_speed = 2 * deviceMultiplier;
        startGame();
    }

    public void easy()
    {
        conveyor.init_speed = .5f * deviceMultiplier;
        conveyor.max_speed = 1.5f * deviceMultiplier;
        startGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
