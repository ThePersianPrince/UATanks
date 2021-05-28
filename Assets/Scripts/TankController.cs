using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
  
    public TankData data;
    public GameObject tank;
    public AudioClip tankDeath;
    public AudioClip bulletHit;
    public float defaultSoundEffectsVolume = 0.5f;
    
    private Manager manager;
    private readonly string soundEffectsVolumeKey = "SoundEffectsVolume";
    private Transform tf;
    public float minY;

    // Use this for initialization
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();

        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();

       
        if (data == null)
        {
            data = gameObject.GetComponent<TankData>();
        }

        minY = tf.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (tf.position.y < minY)
        {
            tf.position = new Vector3(tf.position.x, minY, tf.position.z);
        }

        if (!PlayerPrefs.HasKey(soundEffectsVolumeKey))
        {
            PlayerPrefs.SetFloat(soundEffectsVolumeKey, defaultSoundEffectsVolume);
        }

        if (data.health <= 0)
        {
            if (data.type == TankData.TankType.NPC)
            {
                switch (data.whoShotMeLast)
                {
                    case TankData.TankType.Player_1:
                        
                        manager.IncrementPlayerOneScore(data.pointValue);
                        break;
                    case TankData.TankType.Player_2:
                        
                        manager.IncrementPlayerTwoScore(data.pointValue);
                        break;
                }

                float volume = Mathf.Clamp(PlayerPrefs.GetFloat(soundEffectsVolumeKey), 0.0f, 1.0f);
                AudioSource.PlayClipAtPoint(tankDeath, tf.position, volume);
            }
            else if (data.type == TankData.TankType.Player_1)
            {
                if (!manager.CanPlayerRespawn(1))
                {
                    manager.playerOneGameOverImage.SetActive(true);
                }
            }
            else if (data.type == TankData.TankType.Player_2)
            {
                if (!manager.CanPlayerRespawn(2))
                {
                    manager.playerTwoGameOverImage.SetActive(true);
                }
            }

           
            Destroy(tank);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (data.type != TankData.TankType.NPC)
        {
            
            if (other.gameObject.CompareTag("EnemyBullet"))
            {
                float volume = Mathf.Clamp(PlayerPrefs.GetFloat(soundEffectsVolumeKey), 0.0f, 1.0f);
                AudioSource.PlayClipAtPoint(bulletHit, tf.position, volume);

                data.whoShotMeLast = other.gameObject.GetComponent<BulletController>().firingTankData.type;

                data.health -= data.bulletDmg;
            }
        }
        else
        {
            
            if (other.gameObject.CompareTag("PlayerBullet"))
            {
                float volume = Mathf.Clamp(PlayerPrefs.GetFloat(soundEffectsVolumeKey), 0.0f, 1.0f);
                AudioSource.PlayClipAtPoint(bulletHit, tf.position, volume);

                data.whoShotMeLast = other.gameObject.GetComponent<BulletController>().firingTankData.type;

                data.health -= data.bulletDmg;
            }
        }
    }
}