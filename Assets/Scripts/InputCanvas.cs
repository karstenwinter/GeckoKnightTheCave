using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputCanvas : MonoBehaviour
{
    //public GameObject menu;
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
           Input2.hor = -1;
    }
    public void OnLeftUp()
    {
       Input2.hor = Single.NaN;
    }
    public void OnRight()
    {  
         Input2.hor = 1;
    }
    public void OnRightUp()
    {
        Input2.hor = Single.NaN;
    }
    public void OnJump()
    {
        Input2.jump = true;
    }
    public void OnJumpUp()
    {
        Input2.jump = null;
    }
    public void OnCrouch()
    {
        Input2.crouch = true;
    }
    public void OnCrouchUp()
    {
        Input2.crouch = null;
    }
    public void OnPause()
    {
       // menu.SetEnabled(true);
    }
    public void OnReset()
    {

    }
}
