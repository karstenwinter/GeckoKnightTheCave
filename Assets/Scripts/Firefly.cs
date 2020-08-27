using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : MonoBehaviour
{
    Vector3 startPos;
    float scale = 0.1f;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        var rnd = new Vector3(Random.value, Random.value, 0) * scale;
        transform.position = startPos + rnd;
    }
}
