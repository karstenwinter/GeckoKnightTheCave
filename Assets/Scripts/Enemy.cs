using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Vector3 startPos;
    public float speed = 2f;
    public float patrolWidth = 8f;
    bool headingRight;
    float dx;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
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
    }
}
