using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
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
    }

    [HideInInspector] public float speed; // max speed
    [HideInInspector] public float accelRate; // how fast acceleration goes from 0-1 (0%-100%)
    [HideInInspector] public float sprintHorizontalInputReductionMult; // less acceleration when sprinting
    [HideInInspector] public bool disabledWorkAround;

    private Vector3 localMoveDirection;
    private Vector3 localVelocity;
    private Vector3 addVelocityFromStandingOnRigidbody;
    private Vector3 wantedSpeed;
    [HideInInspector] public Vector3 accelMult; // 0-1 multiplier
    private bool hitGround;
    private bool removeYVelocity;
    private float stepsSinceLastGrounded;
    private float slopeStickSpeed {get{return speed * 1.5f;}}

    private void Update()
    {
        if (!disabledWorkAround)
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
                hitGround = true; // reset this for next time it becomes grounded
        }
    }

    private void FixedUpdate()
    {
        localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
        
        if (_grounded.isGrounded)
        {
            Vector3 newLocalVelocity = localVelocity;
            // to make the multiplier positive or negative depending on input
            newLocalVelocity.x += localMoveDirection.x >= 0 ?
                (wantedSpeed.x *  accelMult.x) - newLocalVelocity.x:
                (wantedSpeed.x * -accelMult.x) - newLocalVelocity.x;

            newLocalVelocity.z += localMoveDirection.z >= 0 ?
                (wantedSpeed.z *  accelMult.z) - newLocalVelocity.z:
                (wantedSpeed.z * -accelMult.z) - newLocalVelocity.z;

            newLocalVelocity = Vector3.ClampMagnitude(newLocalVelocity, speed);

            Vector3 deltaLocalVelocity = newLocalVelocity - localVelocity;
            deltaLocalVelocity = transform.TransformDirection(deltaLocalVelocity);
            deltaLocalVelocity = Vector3.ProjectOnPlane(deltaLocalVelocity, _grounded.contactNormal);
            localVelocity = transform.TransformDirection(localVelocity);

            _rigidbody.velocity = localVelocity + deltaLocalVelocity + addVelocityFromStandingOnRigidbody;

            Debug.DrawRay(transform.position + Vector3.up * 2f, _rigidbody.velocity, Color.red);
        }
        else // in air control
        {
            if (_rigidbody.velocity.magnitude <= speed)
                _rigidbody.AddForce(transform.TransformDirection(localMoveDirection * speed), ForceMode.Acceleration);

            float slowdown = 0.2f * Time.deltaTime;
            _rigidbody.velocity -= new Vector3(_rigidbody.velocity.x * slowdown, 0, _rigidbody.velocity.z * slowdown);
        }
        // TODO: movement only works as wanted with input settings "Gravity" set to 99 or whatever
    }

    private void OnCollisionEnter(Collision col)
    {
        CombineVelocity(col);
    }

    private void OnCollisionStay(Collision col)
    {
        CombineVelocity(col);
    }

    private void CombineVelocity(Collision col)
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