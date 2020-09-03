using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class conveyor : MonoBehaviour
{
    public GameObject starter;
    public GameObject plyer;
    GameObject front, second, third;
    public GameObject[] shapes;
    int score_copy;
    int randPool;


    static public float speed = 0;
    static public float init_speed = 0;
    static public float max_speed;

    public Queue<GameObject> platform = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
        //conform all devices
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;


        logic_UI.clicked = false;

        speed = init_speed;
        

        randPool = Random.Range(0, shapes.Length - 1);
        front = Instantiate(shapes[randPool]);
        front.transform.position = new Vector3(0, 0, 45);

        randPool = Random.Range(0, shapes.Length - 1);
        second = Instantiate(shapes[randPool]);
        second.transform.position = new Vector3(0, 0, 75);


        front.AddComponent<movement>();
        second.AddComponent<movement>();

        // shapes = new GameObject[shapes.Length];
        platform.Enqueue(front);
        platform.Enqueue(second);


    }

    // Update is called once per frame
    void Update()
    {

        if(score_copy < hero.SCORE)
            score_copy = hero.SCORE;

        if (score_copy % 5 == 0 && score_copy > 0)
        {
            speed += .1f;
            score_copy++;

            if (speed > max_speed)
                speed = init_speed;
        }

        //end the game?
        if (hero.game_over == true)
        {
            //Time.timeScale = 0;
            hero.game_over = false;
            SceneManager.LoadScene("menu");
        }


        //if the front is running out, add the the queue, delete the front
        if (front.transform.position.z <= -20f)
        {
            float offset = 0;
            int randPool = Random.Range(0, shapes.Length-1);
            GameObject newOBJ = Instantiate(shapes[randPool]);
            newOBJ.AddComponent<movement>();
            if (newOBJ.name.Contains("cube"))
                offset = -.5f;

            if (randPool == 2)
                offset = -.25f;
            //the 59 builds in some seam tolerances.  30fps creates seaming complications when adding to the queue
            newOBJ.transform.position = new Vector3(0, 0+offset, platform.Peek().transform.position.z + 80);
            platform.Enqueue(newOBJ);
            platform.Dequeue();
            Destroy(front);

            //get the new front
            front = platform.Peek();
        }
    }
}
