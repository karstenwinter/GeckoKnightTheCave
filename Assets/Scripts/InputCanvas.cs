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
    public bool gameIsPaused;
    public GameObject bg;
    public GameObject mobileMenu;
    public GameObject mobileMenu2;
    public GameObject menu;
    public GameObject mainMenu;
    public GameObject inGameUi;
    public GameObject player;
    public Text titleText;
    public Text infoText;
    public float textWriteSpeed;
    public float textStayTime;
    string textToWrite = "Gecko Knight\nChapter 1: The Cave";
    public Button[] array;
    public AudioClip[] audioClips;
    string textBeforepausedText, pausedText = "";
    public bool startInMenu;
    public bool jumpFreely;

    int textIndex;
    float writeTimer;
    AudioSource audio;

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
        menu.active = false;
        mainMenu.active = startInMenu;
        bg.active = startInMenu;
        gameIsPaused = startInMenu;
    }

    void PauseLogic() {
        Time.timeScale = gameIsPaused ? 0f : 1f;
        AudioListener.pause = gameIsPaused;
        menu.active = !mainMenu.active && gameIsPaused;
        bg.active = mainMenu.active || menu.active;
        inGameUi.active = !mainMenu.active && !gameIsPaused;
    }

    void Update()
    {
        if(!mainMenu.active && Input2.GetButtonDown("Cancel")) {
            gameIsPaused = !gameIsPaused;
        }
        PauseLogic();
        
        if(gameIsPaused)
        {
            if(textBeforepausedText == null) {
                textBeforepausedText = infoText.text;
            }
            infoText.text = pausedText;
        } else if(textBeforepausedText != null) {
            infoText.text = textBeforepausedText;
            textBeforepausedText = null;
        }

        if (titleText == null || gameIsPaused)
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

    public void OnMobileInput()
    {
        mobileMenu.active = !mobileMenu.active;
    }

    public void OnMobileMenu()
    {
        mobileMenu2.active = !mobileMenu2.active;
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

    void MenuOff() {
        menu.active = false;
        mainMenu.active = false;
        bg.active = false;
        gameIsPaused = false;
    }

    public void OnContinue()
    {
        MenuOff();
    }

    public void OnSave()
    {
        MenuOff();
        SaveSystem.Save(player);
    }

    public void OnLoad()
    {
        MenuOff();
        SaveSystem.Load(player);
    }

    public void OnLoadGame()
    {
        MenuOff();
        SaveSystem.Load(player);
    }

    public void OnStartGame()
    {
        MenuOff();
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnMainMenu()
    {
        menu.active = false;
        mainMenu.active = true;
    }
}