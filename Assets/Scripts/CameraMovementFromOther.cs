using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraMovementFromOther : MonoBehaviour
{
    public Transform target;
    void FixedUpdate()
    {
        transform.position = target.position;
    }

}
