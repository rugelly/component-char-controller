using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorHalfway : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private InputReader _inputReader;
    private Grounded _grounded;
    private CapsuleCollider _collider;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _inputReader = GetComponent<InputReader>();
        _grounded = GetComponent<Grounded>();
        _collider = GetComponent<CapsuleCollider>();

        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad); //***************//
    }

    [HideInInspector] public float speed; // max speed
    [HideInInspector] public float accelRate; // how fast acceleration goes from 0-1 (0%-100%)
    [HideInInspector] public float sprintHorizontalInputReductionMult; // less acceleration when sprinting

    private Vector3 localMoveDirection;
    private Vector3 localVelocity;
    private Vector3 addVelocityFromStandingOnRigidbody;
    private Vector3 wantedSpeed;
    private Vector3 accelMult; // 0-1 multiplier
    private Vector3 prevMoveDir;
    private Vector3 slopeHit;
    private bool hitGround;
    private bool stopOnSlope;

    Vector3 contactNormal; //***************//
    float maxGroundAngle = 35; //***************//
    float minGroundDotProduct; //***************//

    private void Update()
    {
        localMoveDirection.x = _inputReader.moveHorizontal;
        localMoveDirection.z = _inputReader.moveVertical;
        localMoveDirection = Vector3.ClampMagnitude(localMoveDirection, 1f);

        if (_grounded.isGrounded)
        {
            // only triggers once on landing from air
            if (hitGround) 
            {
                // yes/no instant moving at full speed on landing
                Vector3 localVelocityWithoutY = new Vector3(localVelocity.x, 0, localVelocity.z);
                accelMult = Vector3.Angle(localMoveDirection, localVelocityWithoutY) < 90f ? accelMult : Vector3.zero;

                hitGround = false;
            }

            accelMult.x = localMoveDirection.x != 0 ? accelMult.x += accelRate * Time.deltaTime : 0;
            accelMult.x *= sprintHorizontalInputReductionMult;
            accelMult.x = Mathf.Clamp(accelMult.x, 0, 1);
            accelMult.z = localMoveDirection.z != 0 ? accelMult.z += accelRate * Time.deltaTime : 0;
            accelMult.z = Mathf.Clamp(accelMult.z, 0, 1);

            wantedSpeed = accelMult * speed;
        }
        else
            hitGround = true; // reset this for next time it becomes grounded}
    }

    private void FixedUpdate()
    {
        localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);

        if (_grounded.isGrounded)
        {
            // to make the multiplier positive or negative depending on input
            localVelocity.x += localMoveDirection.x >= 0 ?
                (wantedSpeed.x *  accelMult.x) - localVelocity.x:
                (wantedSpeed.x * -accelMult.x) - localVelocity.x;
            localVelocity.z += localMoveDirection.z >= 0 ?
                (wantedSpeed.z *  accelMult.z) - localVelocity.z:
                (wantedSpeed.z * -accelMult.z) - localVelocity.z;

            /* if (_grounded.slope && !_grounded.tooSteep)
            {
                Quaternion lookAtNextStep = Quaternion.Euler(new Vector3(0, Vector3.Angle(transform.position, StairCheckNextStep()), 0));
                localVelocity = lookAtNextStep * localVelocity;
            } */

            localVelocity = transform.TransformDirection(localVelocity) + addVelocityFromStandingOnRigidbody;
            _rigidbody.velocity = localVelocity;

            Debug.DrawRay(transform.position + Vector3.up * 2f, _rigidbody.velocity, Color.red);
        }
        else
        {
            if (_rigidbody.velocity.magnitude <= speed)
                _rigidbody.AddForce(transform.TransformDirection(localMoveDirection * speed), ForceMode.Acceleration);
        }

        // TODO: movement only works as wanted with input settings "Gravity" set to 99 or whatever
    }
    
    /* public float footHeight = 0.08f; // base cast height
    public float maxStepHeight = 0.3f; // try to clear a ray over an imaginary step smaller than this
    public float forwardCheckLength = 0.1f; // max length to cast in front
    public float downEdgeShift = 0.02f; // push the ray forward over the step a bit to be safe

    private Vector3 StairCheckNextStep()
    {
        //                         ########
        //   ########     3        #      #     // check the height of the step, return hit
        //          #     |        #      #
        //          #     V        #      #
        //          ########       #      #
        //                 #X------#--2   #     // if this ray hit nothing, OK to cast 3
        //                 #       #      #
        //                 #<------#--1 ###     // flat surface in front of movement? cast 2

        RaycastHit frontStepHit;
        RaycastHit downStepHit;
        Vector3 castAheadStart = transform.position + (Vector3.up * footHeight);
        Vector3 stepTopCheck = transform.position + (Vector3.up * maxStepHeight);

        if (Physics.Raycast(castAheadStart, transform.forward, out frontStepHit, forwardCheckLength))
        {
            Debug.DrawRay(castAheadStart, transform.forward * forwardCheckLength, Color.cyan);
            if (!Physics.Raycast(stepTopCheck, transform.forward, forwardCheckLength))
            {
                Debug.DrawRay(stepTopCheck, transform.forward * forwardCheckLength, Color.yellow);
                Vector3 castDownStart = frontStepHit.point + (transform.forward * downEdgeShift) + (Vector3.up * 0.3f);
                if (Physics.Raycast(castDownStart, Vector3.down, out downStepHit, maxStepHeight))
                {
                    Debug.DrawRay(castDownStart, Vector3.down * maxStepHeight, Color.magenta);
                    return downStepHit.point;
                }
                else return Vector3.zero;
            }
            else return Vector3.zero;
        }
        else return Vector3.zero;
    } */

    private void OnCollisionEnter(Collision col)
    {
        EvaluateCollision(col);
    }

    private void OnCollisionStay(Collision col)
    {
        EvaluateCollision(col);
    }

    private void EvaluateCollision(Collision col)
    {
        float minimumHeight = _collider.bounds.min.y + _collider.radius;
        foreach (ContactPoint c in col.contacts)
        {
            if (c.point.y < minimumHeight)
            {
                if (col.rigidbody)
                    addVelocityFromStandingOnRigidbody = col.rigidbody.velocity;
                else
                    addVelocityFromStandingOnRigidbody = Vector3.zero;
            }
        }
    }
}