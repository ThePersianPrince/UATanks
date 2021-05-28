using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    public GameObject start;
    public GameObject game;
    public GameObject options;
    public GameObject gameManager;
    public AudioSource menuMusic;
    public AudioSource buttonClick;
    public float defaultMusicVolume = 0.5f;
    public float defaultSoundEffectsVolume = 0.5f;

    private readonly string musicVolumeKey = "MusicVolume";
    private readonly string soundEffectsVolumeKey = "SoundEffectsVolume";

    public void Start_1Player()
    {
        gameManager.GetComponent<Manager>().newGame = true;

        PlayerPrefs.SetInt("NumPlayers", 1);
        PlayerPrefs.Save();

        start.SetActive(false);
        game.SetActive(true);
        options.SetActive(false);
    }

    public void Start_2Player()
    {
        gameManager.GetComponent<Manager>().newGame = true;

        PlayerPrefs.SetInt("NumPlayers", 2);
        PlayerPrefs.Save();

        start.SetActive(false);
        game.SetActive(true);
        options.SetActive(false);
    }

    public void DisplayOptions()
    {
        start.SetActive(false);
        game.SetActive(false);
        options.SetActive(true);

        GameObject optionsManager = GameObject.FindGameObjectWithTag("OptionsManager");
        optionsManager.GetComponent<OptionsManager>().whoCalledMe = "Start";
    }

    public void Update()
    {
        if (!PlayerPrefs.HasKey(musicVolumeKey))
        {
            PlayerPrefs.SetFloat(musicVolumeKey, defaultMusicVolume);
        }
        menuMusic.volume = PlayerPrefs.GetFloat(musicVolumeKey);

        if (!PlayerPrefs.HasKey(soundEffectsVolumeKey))
        {
            PlayerPrefs.SetFloat(soundEffectsVolumeKey, defaultSoundEffectsVolume);
        }
        buttonClick.volume = PlayerPrefs.GetFloat(soundEffectsVolumeKey);
    }
}
