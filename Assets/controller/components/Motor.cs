using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    #region boilerplate
    private Rigidbody _rigidbody;
    private InputReader _inputReader;
    private Grounded _grounded;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _inputReader = GetComponent<InputReader>();
        _grounded = GetComponent<Grounded>();
    }
    #endregion boilerplate

    Vector3 move;
    Vector3 velocity;
    Vector3 velocityChange;

    [HideInInspector] public float speed;
    [HideInInspector] public float maxVelocityChange;
    [HideInInspector] public float sprintHorizontalInputReductionMult;

    bool horizInput;
    bool vertInput;

    private void Update()
    {
        horizInput = _inputReader.moveHorizontal != 0f ? true : false;
        vertInput = _inputReader.moveVertical != 0f ? true : false;
    }

    private void FixedUpdate()
    {
        move.x = _inputReader.moveHorizontal * sprintHorizontalInputReductionMult;
        move.z = _inputReader.moveVertical;
        move.y = 0f;
        move = Vector3.ClampMagnitude(move, 1f); // no going faster on diagonals
        move = transform.TransformDirection(move); // convert to local coords
        move *= speed; // add length

        // get velocity and determine the change from the move
        velocity = _rigidbody.velocity; 
        velocityChange = move - velocity; 
        // dont exceed constraints
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0f;
        //velocityChange = Vector3.ClampMagnitude(velocityChange, maxVelocityChange);

        // regular control on the ground
        if (_grounded.check)
        {
            //_rigidbody.AddForce(velocityChange, ForceMode.VelocityChange); // weird airborne behaviour on this
            _rigidbody.AddForce(new Vector3(velocityChange.x, 0, 0), ForceMode.VelocityChange);
            _rigidbody.AddForce(new Vector3(0, 0, velocityChange.z), ForceMode.VelocityChange);
            // extra stopping power possibly??
            // double apply force to reach ~0 velocity when input stops
            // TODO: is that what this is actually doing??
            if (transform.InverseTransformDirection(_rigidbody.velocity).x > 0.01f)
            {
                if (!horizInput)
                    _rigidbody.AddForce(new Vector3(velocityChange.x, 0, 0), ForceMode.VelocityChange);
            }
            if (transform.InverseTransformDirection(_rigidbody.velocity).z > 0.01f)
            {
                if (!vertInput)
                    _rigidbody.AddForce(new Vector3(0, 0, velocityChange.z), ForceMode.VelocityChange);
            }
        }
        // in air much looser control
        else
        {
            _rigidbody.AddForce(move, ForceMode.Acceleration);
        }
    }
}
