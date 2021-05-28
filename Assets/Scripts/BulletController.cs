using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
  
    public GameObject bullet;
    public TankData firingTankData;

    private void OnTriggerEnter(Collider other)
    {
       
        Destroy(bullet);
    }
}