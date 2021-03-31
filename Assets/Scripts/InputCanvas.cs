using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class InputCanvas : MonoBehaviour
{
    public static InputCanvas instance;
    public GameObject menu;
    public GameObject player;
    public Text titleText;
    public Text infoText;
    public float textWriteSpeed;
    public float textStayTime;
    string textToWrite = "Gecko Knight - The Cave";
    int textIndex;
    float writeTimer;
    public Button[] array;
    public AudioClip[] audioClips;
    AudioSource audio;
    string textBeforepausedText, pausedText = "PAUSED";

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

    public void SetTextDebug(string t)
    {
        titleText.text = t;
        textIndex = 0;
        textToWrite = t;
        var c = titleText.color;
        c.a = 1;
        titleText.color = c;
    }

    public void PlaySound(string t)
    {
        foreach (var clip in audioClips)
        {
            //Debug.Log(clip.name +"=="+ t);
            if (clip.name == t)
            {
                audio.clip = clip;
                audio.Play();
                return;
            }
        }
    }

    public InputCanvas()
    {
        instance = this;
    }

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {

        if(Platformer.Mechanics.PlayerController.gameIsPaused)
        {
            if(textBeforepausedText == null) {
                textBeforepausedText = infoText.text;
            }
            infoText.text = pausedText;
        } else if(textBeforepausedText != null) {
            infoText.text = textBeforepausedText;
            textBeforepausedText = null;
        }

        if (titleText == null || Platformer.Mechanics.PlayerController.gameIsPaused)
            return;

        if (titleText.text != textToWrite)
        {
            //Debug.Log("writeTimer" + writeTimer + ", index" + textIndex);
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
                //Debug.Log("writeTimer" + writeTimer + ", alpha " + titleText.color.a);
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
        foreach (var b in array)
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

    public void SetHealth(float currentHealth, float cooldown)
    {
        infoText.text = "HEALTH: " + (currentHealth <= 0 ? "" : new String('|', (int)currentHealth))
            + "\nCooldown: " + (cooldown <= 0 ? "" : new String('|', (int)(cooldown * 10)));
    }

    string area;
    public void SetArea(string v)
    {
        SetText("Area: " + v);
        area = v;
    }

    public void OnSave()
    {
        SaveSystem.Save(player);
        
    }

    public void OnLoad()
    {
        SaveSystem.Load(player);
    }

    public void OnExit()
    {
        Environment.Exit(0);
    }
}