using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wiggle : MonoBehaviour
{
    public float speed;
    public bool tiltR = false, tiltL = true;
    RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {

        speed = .5f;
        rect = gameObject.GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        if (tiltL)
        {
            gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0, 0, -20), Time.deltaTime * speed);

            if (rect.eulerAngles.z > 50 && rect.eulerAngles.z < 358) 
            {
                tiltR = true;
                tiltL = false;
            }
        }

        if(tiltR)
        {
            gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0, 0, 20), Time.deltaTime * speed);

            if (rect.eulerAngles.z < 300 && rect.eulerAngles.z > 2)
            {
                tiltR = false;
                tiltL = true;
            }
        }
    }
}
