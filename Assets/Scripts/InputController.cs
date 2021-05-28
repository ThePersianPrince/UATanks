using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public enum InputScheme { WASD, arrowKeys };
    public InputScheme input = InputScheme.WASD;
    public TankMotor motor;
    public TankData data;
    public Shooter shooter;
	
	// Update is called once per frame
	void Update ()
    {
		switch (input)
        {
            case InputScheme.arrowKeys:
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    
                    motor.Move(data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    
                    motor.Move(-data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                   
                    motor.Rotate(data.turnSpeed);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    
                    motor.Rotate(-data.turnSpeed);
                }
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                   
                    shooter.Fire();
                }
                break;
            
            case InputScheme.WASD:
                if (Input.GetKey(KeyCode.W))
                {
                   
                    motor.Move(data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    
                    motor.Move(-data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    
                    motor.Rotate(data.turnSpeed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                   
                    motor.Rotate(-data.turnSpeed);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                   
                    shooter.Fire();
                }
                break;
            default:
                break;
        }
	}
}