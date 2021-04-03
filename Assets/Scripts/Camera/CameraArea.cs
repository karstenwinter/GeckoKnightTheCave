using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraArea : MonoBehaviour
{
    public CinemachineConfiner confiner;
    Collider2D old;
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other: "+other);
        if(other.tag == "Player") {
            old = confiner.m_BoundingShape2D;
            confiner.m_BoundingShape2D = GetComponent<PolygonCollider2D>();
            Debug.Log("Enter " + this.gameObject.name);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player" && old != null) {
             Debug.Log("Exit " + this.gameObject.name);
            //confiner.m_BoundingShape2D = old;
            //old = null;
        }
    }

}
