using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    #region boilerplate
    private Rigidbody _rigidbody;
    private InputReader _inputReader;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _inputReader = GetComponent<InputReader>();
    }
    #endregion boilerplate

    Vector3 direction;
    Vector3 velocity;
    Vector3 velocityChange;

    [HideInInspector] public float speed;
    [HideInInspector] public float maxVelocityChange;
    [HideInInspector] public float sprintHorizontalInputReductionMult;

    private void Update()
    {
        direction = new Vector3(
            _inputReader.moveHorizontal * sprintHorizontalInputReductionMult, 
            0, 
            _inputReader.moveVertical);
        direction = transform.TransformDirection(direction);

        velocity = _rigidbody.velocity;
		velocityChange = (direction - velocity);


        direction *= speed;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
    }

    private void FixedUpdate()
    {
        // old grounded stuff incase new impl dont work
        /* if (_grounded.check)
        {
            // on ground has regular force response from input
            direction *= speed;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        }
        else
        {
            // in air has significantly reduced response from input
            direction *= airSpeed;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        } */

        // here is where force is added???
        _rigidbody.AddForce(direction);
    }
}
