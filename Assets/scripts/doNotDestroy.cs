using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class doNotDestroy : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource source;
    static public int HS = 0;


    private void Awake()
    {
        HS = PlayerPrefs.GetInt("highscore");
        Object.DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HS < hero.SCORE)
        {
            HS = hero.SCORE;
            PlayerPrefs.SetInt("highscore", HS);
        }


        if (hero.game_over)
            source.PlayOneShot(clips[0]);

        if(logic_UI.clicked)
            source.PlayOneShot(clips[1]);



    }
}
