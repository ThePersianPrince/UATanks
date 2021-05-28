using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]

public class TankMotor : MonoBehaviour
{
    
    private CharacterController characterController;
    private Transform tf;
    private Shooter shooter;

    public void Awake()
    {
        
        tf = gameObject.GetComponent<Transform>();
    }

    // Use this for initialization
    void Start ()
    {
        
        characterController = gameObject.GetComponent<CharacterController>();
        shooter = gameObject.GetComponent<Shooter>();
    }

    
    public void Move(float speed)
    {
     
        characterController.SimpleMove(transform.forward * speed);
    }

    
    public void Rotate(float speed)
    {
        
        tf.Rotate(speed * Time.deltaTime * Vector3.up, Space.Self);
    }

    
    public bool RotateTowards(Vector3 target, float speed)
    {
        Vector3 vectorToTarget;

       
        vectorToTarget = target - tf.position;

        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);
        if (targetRotation == tf.rotation)
        {
            return false;
        }

      
        tf.rotation = Quaternion.RotateTowards(tf.rotation, targetRotation, speed * Time.deltaTime);

       
        return true;
    }

    public void Shoot()
    {
        shooter.Fire();
    }
}