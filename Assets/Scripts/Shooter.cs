using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public TankData firingTankData;
    public AudioClip tankFire;
    public float shotCooldown = 2;
    public float bulletSpeed = 8;
    public float defaultSoundEffectsVolume = 0.5f;

    private float lastShot;
    private readonly string soundEffectsVolumeKey = "SoundEffectsVolume";

    // Use this for initialization
    void Start ()
    {
        
        lastShot = Time.time - shotCooldown;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!PlayerPrefs.HasKey(soundEffectsVolumeKey))
        {
            PlayerPrefs.SetFloat(soundEffectsVolumeKey, defaultSoundEffectsVolume);
        }
    }

    public void Fire()
    {
        
        if (Time.time - lastShot > shotCooldown)
        {
            
            AudioSource.PlayClipAtPoint(tankFire, bulletSpawn.position, PlayerPrefs.GetFloat(soundEffectsVolumeKey));

            
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

            
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

            
            bullet.GetComponent<BulletController>().firingTankData = firingTankData;

            
            Destroy(bullet, 10.0f);

            
            lastShot = Time.time;
        }
    }
}