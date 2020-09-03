using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hero : MonoBehaviour
{

    public Camera cam;
    public float rollSpeed = 10;
    public float height = 10;
    public bool jumping = false;
    public bool atDestination = false;
    public float changeLaneSpeed = 5;
    public Quaternion camRotation;
    public bool keyboard;
    public float tolerance = .3f;
    public float verticalTolerance = .75f;


    //touch vars
    private Touch touch;
    private Vector2 beginTouchPos, endTouchPos;
    private float horizontal, vertical, min, max;

    public GameObject waterEffect;

    static public int SCORE;

    //global placement of our marble.
    protected int counter = 2;

    //possible marble placements
    //Vector3[] places = { new Vector3(-7f, .5f,0), new Vector3(-3.75f, .5f, 0), new Vector3(0, .5f, 0), new Vector3(3.75f, .5f, 0), new Vector3(7f,.5f,0) };
    float[] places = { -7f, -3.75f, 0, 3.75f, 7f};

    

    public int camRotationAngle = 5;

    protected Rigidbody rb;
    protected SphereCollider col;

    protected Vector3 destination;

    static public bool game_over = false;

    public AudioSource source;
    public AudioClip[] clips;
    GameObject water;

    // Start is called before the first frame update
    void Start()
    {
        water = null;

        source = GetComponent<AudioSource>();

        SCORE = 0;
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<SphereCollider>();

        destination = gameObject.transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //check the jump son
        if (gameObject.transform.position.y > verticalTolerance)
            jumping = true;
        else
            jumping = false;

        //output our hero pos to the shader
        Shader.SetGlobalVector("_ObjectPos", gameObject.transform.position);

        //have the parical effect follow the hero
        if (water)
            water.transform.position = new Vector3(gameObject.transform.position.x, .3f, gameObject.transform.position.z);

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, destination, Time.deltaTime * changeLaneSpeed);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camRotation, Time.deltaTime * changeLaneSpeed);
        cam.transform.position = new Vector3(gameObject.transform.position.x, 1.5f + gameObject.transform.position.y, cam.transform.position.z);

        //this is our ground truth. If hero is near destination, camera goes back to normal
        if (Vector3.Distance(gameObject.transform.position, destination) < tolerance)
        {
            //bool of these flags allow us to jump and or move again
            atDestination = true;
        }

        //do the things that resest state to normal
        if (atDestination)
        {
            camRotation = Quaternion.Euler(0,0,0);
        }


        Roll();



        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            switch(touch.phase)
            {
                case TouchPhase.Began:
                {
                    beginTouchPos = touch.position;
                    break;
                }

                case TouchPhase.Ended:
                {
                    endTouchPos = touch.position;


                    if (atDestination && !jumping)
                    {
                        //calculate the delta to differentiate
                        min = Mathf.Min(beginTouchPos.x, endTouchPos.x);
                        max = Mathf.Max(beginTouchPos.x, endTouchPos.x);

                        //x-axis delta
                        horizontal = max - min;

                        min = Mathf.Min(beginTouchPos.y, endTouchPos.y);
                        max = Mathf.Max(beginTouchPos.y, endTouchPos.y);

                        //y-axis delta
                        vertical = max - min;

                        if (vertical > horizontal)
                            Jump();

                        else
                        {
                            if (beginTouchPos.x >= endTouchPos.x)
                                moveLeft();
                            else
                                moveRight();
                        }
                    }
                    break;
                }
            }
        }

        if(keyboard)
        {
            //DEBGUG testing input on keyboard
            if (Input.GetKeyDown("space") && !jumping && atDestination)
                Jump();

            if (Input.GetKeyDown("d") && !jumping && atDestination)
                moveRight();

            if (Input.GetKeyDown("a") && !jumping && atDestination)
                moveLeft();
        }
        
    }



    void Roll()
    {
        gameObject.transform.Rotate(gameObject.transform.position.x + (1 * rollSpeed), 0, 0, Space.Self);
    }

    void Jump()
    {
        rb.velocity = Vector3.up * height;
        source.PlayOneShot(clips[0]);
    }

    void moveRight()
    {
        atDestination = false;
        counter++;
        destination = new Vector3(places[counter], gameObject.transform.position.y, 0);
        camRotation = Quaternion.Euler(0, 0, -camRotationAngle);
        source.PlayOneShot(clips[1]);
    }

    void moveLeft()
    {
        atDestination = false;
        counter--;
        destination = new Vector3(places[counter], gameObject.transform.position.y, 0);
        camRotation = Quaternion.Euler(0, 0, camRotationAngle);
        source.PlayOneShot(clips[1]);
    }


    private void OnCollisionStay(Collision col)
    {

    }

    private void OnCollisionExit(Collision col)
    {
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Bad") == true)
        {
            Debug.Log("ending");
            end();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(!col.CompareTag("Water"))
            SCORE++;
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("Water") && !water)
            water = Instantiate(waterEffect);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Water"))
        {
            Destroy(water);
            water = null;
        }

    }

    void end()
    {
        game_over = true;
    }
}
