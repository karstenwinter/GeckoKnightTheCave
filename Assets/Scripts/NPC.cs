using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Vector3 startPos;
    //public float speed = 2f;
    //public float patrolWidth = 0f;
    //bool headingRight;
    //float dx;
    public GameObject mark;

    void Start()
    {
        startPos = transform.position;
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Mark")
            {
                mark = child.gameObject;
                break;
            }
        }
       // Debug.Log("Mark is " + mark);
    }

    void Update()
    {
        /*if(patrolWidth != 0) { 
            dx += (headingRight ? speed : -speed) * Time.deltaTime;
            transform.position = startPos + new Vector3(dx, 0, 0);

            if (headingRight && dx > patrolWidth)
            {
                headingRight = false;
            }

            if (!headingRight && dx < -patrolWidth)
            {
                headingRight = true;
            }
        }*/
    }
}
