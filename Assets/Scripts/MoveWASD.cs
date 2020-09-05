using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWASD : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        var dx = Input.GetAxis("Horizontal");
        var dy = Input.GetAxis("Vertical");
        transform.position += new Vector3(dx, dy, 0);
      
        if(dx != 0 || dy != 0)
        {
            InputCanvas.instance.PlaySound("move");
        }
    }

   

}
