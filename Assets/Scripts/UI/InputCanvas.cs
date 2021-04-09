using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using Platformer.Mechanics;

public class InputCanvas : MonoBehaviour
{
    public static InputCanvas instance;
    public ParticleSystem menuParticleSystem;
    public bool gameIsPaused;
    public GameObject bgBlack, bgPause,
        mainMenu, pauseMenu,
        mainMenuButtons, optionsMenu,
        player, level,
        buttonStartMain, buttonContinuePause, buttonProfile,
        fontCredits;
        
    public GameObject[] inGameUi, mobileControls;

    public Text titleText, infoText, loadStateInfo, languageInfo, difficultyInfo, profileInfo, inputInfo;

    public float textWriteSpeed, textStayTime;
    string textToWrite = "Gecko Knight\nChapter 1: The Cave";
    public Button[] mobileButtonArray;
    public AudioClip[] audioClips;
    string textBeforepausedText, pausedText = "";
    public bool startInMenu;
    public bool jumpFreely;
    public float timeScale = 1f;
    public GameObject[] hearts = new GameObject[0];

    int textIndex;
    float writeTimer;
    AudioSource audio;
    string profile = "First";
    string difficulty = "Normal";
    string language = "English";
    string input = "Keyboard or Gamepad";

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
        if(audioClips == null)
            return;

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
        pauseMenu.active = false;
        bgPause.active = false;
        mainMenu.active = startInMenu;
        MainMenuActive();
        bgBlack.active = startInMenu;
        gameIsPaused = startInMenu;
        
        var settings = SaveSystem.LoadSettings(false);
        if(settings != null) {
            profile = settings.profile;
        } else {
            saveSettings();
        }
        RefreshLoadInfo();
    }

    void saveSettings() {
        SaveSystem.SaveSettings(new GlobalSettings { profile = profile });
    }

    void RefreshLoadInfo() {
        var state = SaveSystem.LoadState(profile, false);
        loadStateInfo.text = state == null ? "" : state.ToString();
    }

    void MainMenuActive() {
        if(mainMenu.active) {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(buttonStartMain);
        }
    }

    void PauseLogic() {
        Time.timeScale = gameIsPaused ? 0 : timeScale;
        AudioListener.pause = gameIsPaused;
        var pauseMenuActive = !mainMenu.active && gameIsPaused;

        if(pauseMenuActive && !pauseMenu.active) {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(buttonContinuePause);
        }
        pauseMenu.active = pauseMenuActive;

        bgPause.active = mainMenu.active || pauseMenu.active;
        Array.ForEach(inGameUi, x => x.active = !mainMenu.active && !gameIsPaused);
        bgPause.active = gameIsPaused;

        bgBlack.active = mainMenu.active;
        menuParticleSystem.gameObject.active = bgBlack.active;
        level.active = !mainMenu.active;
        player.active = !mainMenu.active;
        player.GetComponent<PlayerController>().controlEnabled = !gameIsPaused;
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

    public void OnMobileMenu()
    {
        //mobileMenu.active = !mobileMenu.active;
        gameIsPaused = !gameIsPaused;
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
        foreach (var b in mobileButtonArray)
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
        //player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        //player.transform.position = player.GetComponent<PlayerMovement>().startPosition;

        //jumpFreely = !jumpFreely;
        //SetText("Reset :) jumpFreely: " + jumpFreely);
    }

    public void UpdateValues(int currentHealth, float cooldown, int shells)
    {
        //infoText.text = "Health: " + (currentHealth <= 0 ? "" : new String('O', (int)currentHealth))
        //    + "\nCooldown: " + (cooldown <= 0 ? "" : new String('|', (int)(cooldown * 10)))
        //    + "\nShells: " + shells;
        infoText.text = shells.ToString();
        for(int i=0;i<hearts.Length;i++) {
            hearts[i].active = i < currentHealth;
        }
    }

    string area;
    public void SetArea(string v)
    {
        SetText("Area: " + v);
        area = v;
    }

    void MenuOff() {
        pauseMenu.active = false;
        mainMenu.active = false;
        bgPause.active = false;
        bgBlack.active = false;
        gameIsPaused = false;
    }

    public void OnContinue()
    {
        MenuOff();
    }

    public void OnSave()
    {
        MenuOff();
        SaveSystem.Save(profile, player);
    }

    public void OnLoad()
    {
        MenuOff();
        SaveSystem.Load(profile, player);
    }

    public void OnLoadGame()
    {
        MenuOff();
        SaveSystem.Load(profile, player);
    }

    public void OnStartGame()
    {
        MenuOff();
    }

    public void OnLanguage()
    {
        var oldVal = language;
        language = language == "English" ? "Deutsch" : "English";
        switchBold(languageInfo, oldVal, language);
    }

    public void OnMobileInput()
    {
        var oldVal = input;
        input = input == "Keyboard or Gamepad" ? "Touchpad" : "Keyboard or Gamepad";
        switchBold(inputInfo, oldVal, input);

        Array.ForEach(mobileControls, x => x.active = input == "Touchpad");
    }

    public void OnDifficulty()
    {
        var oldVal = difficulty;
        difficulty = difficulty == "Normal" ? "Easy" : "Normal";
        switchBold(difficultyInfo, oldVal, difficulty);
    }

    public void OnProfile()
    {
        var oldVal = profile;
        profile = profile == "First" ? "Second" : "First";
        switchBold(profileInfo, oldVal, profile);
        saveSettings();
    }

    void switchBold(Text text, string oldVal, string newVal) {
        text.text = text.text
                .Replace("<b>"+oldVal+"</b>", oldVal)
                .Replace(newVal, "<b>"+newVal+"</b>");
    }

    public void OnOptionsBack()
    {
        mainMenuButtons.active = !mainMenuButtons.active;
        optionsMenu.active = !optionsMenu.active;
        fontCredits.active = optionsMenu.active;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttonStartMain);
    }

    public void OnOptions()
    {
        mainMenuButtons.active = !mainMenuButtons.active;
        optionsMenu.active = !optionsMenu.active;
        fontCredits.active = optionsMenu.active;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttonProfile);
    }

    public void OnCommunity()
    {
        Application.OpenURL("https://discord.gg/PHXRWVf");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnMainMenu()
    {
        RefreshLoadInfo();
        pauseMenu.active = false;
        mainMenu.active = true;
        MainMenuActive();
    }
}