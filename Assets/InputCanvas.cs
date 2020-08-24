using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnLeft()
    {
        Debug.Log("OnLeft");
        Input2.hor = -1;
    }
    public void OnLeftUp()
    {
        Debug.Log("OnLeftUp");
        Input2.hor = Single.NaN;
    }
    public void OnRight()
    {
        Debug.Log("OnRight");
        Input2.hor = 1;
    }
    public void OnRightUp()
    {
        Debug.Log("OnRightUp");
        Input2.hor = Single.NaN;
    }
    public void OnJump()
    {
        Debug.Log("OnJump");
        Input2.jump = true;
    }
    public void OnJumpUp()
    {
        Debug.Log("OnJumpUp");
        Input2.jump = null;
    }
}
