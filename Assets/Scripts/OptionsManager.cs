using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public GameObject start;
    public GameObject game;
    public GameObject options;
    public Text soundEffectsVolumeText;
    public Text mapTypeText;
    public Text musicVolumeText;
    public AudioSource menuMusic;
    public AudioSource buttonClick;
    public float musicVolume = 0.5f;
    public float musicVolumeIncrement = 0.1f;
    public float musicVolumeMin = 0.0f;
    public float musicVolumeMax = 1.0f;
    public float soundEffectsVolume = 0.5f;
    public float soundEffectsVolumeIncrement = 0.1f;
    public float soundEffectsVolumeMin = 0.0f;
    public float soundEffectsVolumeMax = 1.0f;
    public string whoCalledMe;

    private string mapType;
    private readonly string musicVolumeKey = "MusicVolume";
    private readonly string soundEffectsVolumeKey = "SoundEffectsVolume";
    private readonly string mapTypeKey = "MapType";

    public void Start()
    {
        if (PlayerPrefs.HasKey(musicVolumeKey))
        {
            musicVolume = PlayerPrefs.GetFloat(musicVolumeKey);
        }
        else
        {
            PlayerPrefs.SetFloat(musicVolumeKey, musicVolume);
        }

        if (PlayerPrefs.HasKey(soundEffectsVolumeKey))
        {
            soundEffectsVolume = PlayerPrefs.GetFloat(soundEffectsVolumeKey);
        }
        else
        {
            PlayerPrefs.SetFloat(soundEffectsVolumeKey, soundEffectsVolume);
        }

        if (PlayerPrefs.HasKey(mapTypeKey))
        {
            mapType = PlayerPrefs.GetString(mapTypeKey);
        }
        else
        {
            mapType = "Random Map";
            PlayerPrefs.SetString(mapTypeKey, mapType);
        }
    }
    public void Return()
    {
        options.SetActive(false);

        if (whoCalledMe == "Start")
        {
            start.SetActive(true);
            game.SetActive(false);
        }
        else if (whoCalledMe == "Game")
        {
            start.SetActive(false);
            game.SetActive(true);
        }
    }

    public void TurnMusicVolumeUp()
    {
        if (PlayerPrefs.HasKey(musicVolumeKey))
        {
            musicVolume = PlayerPrefs.GetFloat(musicVolumeKey);
            musicVolume = Mathf.Clamp(musicVolume + musicVolumeIncrement, musicVolumeMin, musicVolumeMax);
            PlayerPrefs.SetFloat(musicVolumeKey, musicVolume);
        }
        else
        {
            PlayerPrefs.SetFloat(musicVolumeKey, musicVolume);
        }

        PlayerPrefs.Save();
    }

    public void TurnMusicVolumeDown()
    {
        if (PlayerPrefs.HasKey(musicVolumeKey))
        {
            musicVolume = PlayerPrefs.GetFloat(musicVolumeKey);
            musicVolume = Mathf.Clamp(musicVolume - musicVolumeIncrement, musicVolumeMin, musicVolumeMax);
            PlayerPrefs.SetFloat(musicVolumeKey, musicVolume);
        }
        else
        {
            PlayerPrefs.SetFloat(musicVolumeKey, musicVolume);
        }

        PlayerPrefs.Save();
    }

    public void TurnSoundEffectsVolumeUp()
    {
        if (PlayerPrefs.HasKey(soundEffectsVolumeKey))
        {
            soundEffectsVolume = PlayerPrefs.GetFloat(soundEffectsVolumeKey);
            soundEffectsVolume = Mathf.Clamp(soundEffectsVolume + soundEffectsVolumeIncrement, soundEffectsVolumeMin, soundEffectsVolumeMax);
            PlayerPrefs.SetFloat(soundEffectsVolumeKey, soundEffectsVolume);
        }
        else
        {
            PlayerPrefs.SetFloat(soundEffectsVolumeKey, soundEffectsVolume);
        }

        PlayerPrefs.Save();
    }

    public void TurnSoundEffectsVolumeDown()
    {
        if (PlayerPrefs.HasKey(soundEffectsVolumeKey))
        {
            soundEffectsVolume = PlayerPrefs.GetFloat(soundEffectsVolumeKey);
            soundEffectsVolume = Mathf.Clamp(soundEffectsVolume - soundEffectsVolumeIncrement, soundEffectsVolumeMin, soundEffectsVolumeMax);
            PlayerPrefs.SetFloat(soundEffectsVolumeKey, soundEffectsVolume);
        }
        else
        {
            PlayerPrefs.SetFloat(soundEffectsVolumeKey, soundEffectsVolume);
        }

        PlayerPrefs.Save();
    }

    public void MapTypePrevious()
    {
        if (mapType == "Map Of The Day")
        {
            mapType = "Random Map";
        }
        else if (mapType == "Random Map")
        {
            mapType = "Map Of The Day";
        }

        PlayerPrefs.SetString(mapTypeKey, mapType);
    }

    public void MapTypeNext()
    {
        if (mapType == "Map Of The Day")
        {
            mapType = "Random Map";
        }
        else if (mapType == "Random Map")
        {
            mapType = "Map Of The Day";
        }

        PlayerPrefs.SetString(mapTypeKey, mapType);
    }

    public void Update()
    {
        musicVolume = PlayerPrefs.GetFloat(musicVolumeKey);
        musicVolumeText.text = musicVolume.ToString();

        soundEffectsVolume = PlayerPrefs.GetFloat(soundEffectsVolumeKey);
        soundEffectsVolumeText.text = soundEffectsVolume.ToString();

        mapType = PlayerPrefs.GetString(mapTypeKey);
        mapTypeText.text = mapType;

        if (!PlayerPrefs.HasKey(musicVolumeKey))
        {
            PlayerPrefs.SetFloat(musicVolumeKey, musicVolume);
        }

        menuMusic.volume = PlayerPrefs.GetFloat(musicVolumeKey);

        if (!PlayerPrefs.HasKey(soundEffectsVolumeKey))
        {
            PlayerPrefs.SetFloat(soundEffectsVolumeKey, soundEffectsVolume);
        }

        buttonClick.volume = PlayerPrefs.GetFloat(soundEffectsVolumeKey);
    }
}
