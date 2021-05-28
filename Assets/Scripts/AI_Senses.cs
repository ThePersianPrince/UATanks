using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Senses : MonoBehaviour
{
    public float fieldOfView = 45;
    public float viewDistance = 100;
    public float hearDistance = 50;

    private Transform tf;

    public void Awake()
    {
       
        tf = gameObject.GetComponent<Transform>();
    }

    public bool CanSee(GameObject target)
    {
        if (target)
        {
           
            Vector3 vectorToTarget = target.transform.position - tf.position;
            float angleToTarget = Vector3.Angle(tf.forward, vectorToTarget);

            if (angleToTarget < fieldOfView)
            {

                Ray myRay = new Ray();
                _ = new RaycastHit();
                myRay.origin = tf.position;
                myRay.direction = vectorToTarget;
                if (Physics.Raycast(myRay, out RaycastHit hitInfo, viewDistance))
                {
                    if (hitInfo.collider.gameObject == target)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
              
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool CanHear(GameObject target)
    {
        if (target)
        {
            if (Vector3.Distance(target.transform.position, tf.position) < hearDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
