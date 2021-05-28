using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    public enum TankType
    {
        Undefined,
        NPC,
        Player_1,
        Player_2
    }

    public int bulletDmg = 25;
    public int pointValue = 10;
    public float moveSpeed = 5;
    public float turnSpeed = 180; 
    public float health = 100;
    public float maxHealth = 100;
    public float fireRate = 5;
    public float healValue = 10;
    public float healCooldown = 5;
    public TankType type = TankType.NPC;
    public TankType whoShotMeLast = TankType.Undefined;
}