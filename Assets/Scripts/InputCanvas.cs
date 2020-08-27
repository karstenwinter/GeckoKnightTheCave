using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class InputCanvas : MonoBehaviour
{
    public static InputCanvas instance;
    public GameObject menu;
    public GameObject player;
    public Text titleText;
    public float textWriteSpeed;
    public float textStayTime;
    string textToWrite = "Gecko Knight - The Cave";
    int textIndex;
    float writeTimer;
    public Button[] array;

    public bool jumpFreely;

    public void SetText(string t)
    {
        titleText.text = "";
        textIndex = 0;
        textToWrite = t;
        var c = titleText.color;
        c.a = 1;
        titleText.color = c;
    }

    // Start is called before the first frame update
    public InputCanvas()
    {
        instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (titleText.text != textToWrite)
        {
            Debug.Log("writeTimer" + writeTimer + ", index" + textIndex);
            writeTimer -= Time.fixedDeltaTime;

            if (writeTimer < 0 && textIndex < textToWrite.Length)
            {
                textIndex++;
                titleText.text = textToWrite.Substring(0, textIndex);

                if (titleText.text == textToWrite)
                {
                    writeTimer += textStayTime;
                    textIndex = 0;
                }
                else
                {
                    writeTimer += textWriteSpeed;
                }
            }
        }
        else
        {
            if (writeTimer > 0 && textToWrite != "")
            {
                Debug.Log("writeTimer" + writeTimer + ", alpha " + titleText.color.a);
                writeTimer -= Time.fixedDeltaTime;
                if (writeTimer < 0)
                {
                    var c = titleText.color;
                    c.a -= 0.05f;
                    titleText.color = c;
                    writeTimer += textStayTime;
                    if (c.a <= 0)
                    {
                        textToWrite = "";
                        titleText.text = "";
                        c.a = 1;
                        titleText.color = c;
                    }
                }
            }
        }
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
        menu.SetActive(!menu.activeSelf);
    }
    public void OnScaleUp()
    {
        scale(+10);
    }
    public void OnScaleDown()
    {
        scale(-10);
    }
    void scale(int delta)
    {
        foreach(var b in array)
        {
            var t = ((RectTransform)b.transform);
            var v = t.sizeDelta;
            v.x += delta;
            v.y += delta;
            t.sizeDelta = v;
        }
    }

    public void OnReset()
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.transform.position = player.GetComponent<PlayerMovement>().startPosition;

        jumpFreely = !jumpFreely;
        SetText("Reset :) jumpFreely: " + jumpFreely);

    }
}
