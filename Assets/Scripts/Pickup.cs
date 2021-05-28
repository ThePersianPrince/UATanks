using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Powerup powerup;
    public AudioClip feedback;
    public float defaultSoundEffectsVolume = 0.5f;

    private Transform tf;
    private readonly string soundEffectsVolumeKey = "SoundEffectsVolume";

    // Use this for initialization
    void Start ()
    {
        tf = gameObject.GetComponent<Transform>();
	}

    public void OnTriggerEnter(Collider other)
    {
        PowerupController powCon = other.gameObject.GetComponent<PowerupController>();

 
        if (powCon != null)
        {
            powCon.Add(powerup);

            if (feedback != null)
            {
                if (!PlayerPrefs.HasKey(soundEffectsVolumeKey))
                {
                    PlayerPrefs.SetFloat(soundEffectsVolumeKey, defaultSoundEffectsVolume);
                }

                float volume = Mathf.Clamp(PlayerPrefs.GetFloat(soundEffectsVolumeKey), 0.0f, 1.0f);
                AudioSource.PlayClipAtPoint(feedback, tf.position, volume);
            }

            Destroy(gameObject);
        }
    }
}
