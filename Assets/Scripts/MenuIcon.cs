﻿using UnityEngine;
using System.Collections;

public class MenuIcon : MonoBehaviour {
    private const string DEBUG_KEY = "debug";
    private const string HINTS_KEY = "hints";
    private const string SOUNDS_KEY = "sounds";
    private const string MUSIC_KEY = "music";
    [SerializeField] GameObject debug;
    [SerializeField] GameObject hints;
    [SerializeField] GameObject sound;
    [SerializeField] GameObject music;
    SpriteRenderer debugSR;
    SpriteRenderer soundsSR;
    SpriteRenderer hintsSR;
    SpriteRenderer musicSR;
    public Color gray;
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource sfxPlayer;
    Scripts scripts;
    void Start() {
        scripts = FindObjectOfType<Scripts>();
        debugSR = debug.GetComponent<SpriteRenderer>();
        hintsSR = hints.GetComponent<SpriteRenderer>();
        soundsSR = sound.GetComponent<SpriteRenderer>();
        musicSR = music.GetComponent<SpriteRenderer>();
        ColorUtility.TryParseHtmlString("#404040", out gray);
        // get the necessary components and colors
        // assign var gray to the html string parsed
        PlayerPrefSetter(DEBUG_KEY, debugSR, false);
        PlayerPrefSetter(HINTS_KEY, hintsSR, false);
        PlayerPrefSetter(SOUNDS_KEY, soundsSR, false);
        PlayerPrefSetter(MUSIC_KEY, musicSR, false);
        // set the default preferences
        StartCoroutine(FindMusicLate());
    }

    private IEnumerator FindMusicLate() { 
        yield return scripts.delays[0.25f];
        musicPlayer = FindObjectOfType<Music>().GetComponent<AudioSource>();
    }

    void Update() {
        if (Input.GetKeyDown("d")) { PlayerPrefSetter(DEBUG_KEY, debugSR); }
        else if (Input.GetKeyDown("h")) { PlayerPrefSetter(HINTS_KEY, hintsSR); }
        else if (Input.GetKeyDown("s")) { PlayerPrefSetter(SOUNDS_KEY, soundsSR); }
        else if (Input.GetKeyDown("m")) { PlayerPrefSetter(MUSIC_KEY, musicSR); }
    }
    // toggle player preferences based on the keys pressed

    /// <summary>
    /// Set the player preference (setting) for the given key.
    /// </summary>
    public void PlayerPrefSetter(string key, SpriteRenderer spriteRenderer, bool isSwap = true) {
        if (PlayerPrefs.GetString(key) == "on") {
            // on
            if (isSwap) {
                PlayerPrefs.SetString(key, "off");
                TurnOff(key, spriteRenderer);
                // so turn off
            }
            else { TurnOn(key, spriteRenderer); }
            // set the correct sprite
        }
        else {
            // off
            if (isSwap) {
                PlayerPrefs.SetString(key, "on");
                TurnOn(key, spriteRenderer);
                // so turn on
            }
            else { TurnOff(key, spriteRenderer); }
            // set the correct sprite
        }
    }

    /// <summary>
    /// Turn on the player preference with the associated key.
    /// </summary>
    private void TurnOn(string key, SpriteRenderer spriteRenderer) {
        spriteRenderer.color = Color.white;
        // make the icon white
        switch (key) {
            case DEBUG_KEY:
                break;
            case HINTS_KEY:
                break;
            case SOUNDS_KEY:
                sfxPlayer.volume = 1f;
                break;
            case MUSIC_KEY:
                musicPlayer.volume = 0.5f;
                break;
            // do the correct action
        }
    }

    /// <summary>
    /// Turn on the player preference with the associated key.
    /// </summary>
    private void TurnOff(string key, SpriteRenderer spriteRenderer) {
        spriteRenderer.color = gray;
        // make the icon gray
        switch (key) {
            case DEBUG_KEY:
                break;
            case HINTS_KEY:
                break;
            case SOUNDS_KEY:
                sfxPlayer.volume = 0f;
                break;
            case MUSIC_KEY:
                musicPlayer.volume = 0f;
                break;
            // do the correct action
        }
    }
}
