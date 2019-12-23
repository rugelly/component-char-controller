using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private InputReader _input;

    [HideInInspector] public float strength;
    [HideInInspector] public Vector3 direction;

    /* private float maxGroundAngle = 40f; // specified in degrees
    private float minGroundDotProduct;
    Vector3 contactNormal; */

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();

        /* // angle is given in degrees so needs to be converted to rads
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad); */
    }

    private void FixedUpdate()
    {
        // equation:
        // velocity(y) = sqrRoot of -2(negative because gravity is too) * gravity(g) * height(h)
        float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * strength);

        _rigidbody.AddRelativeForce(direction * jumpSpeed, ForceMode.VelocityChange);

        /* // accounting for normal direction, check for speed aligned with contact normal
        // find this by projecting the velocity on the contact normal and calculating their dot
        float alignedSpeed = Vector3.Dot(_rigidbody.velocity, contactNormal);
        // dont let the value be smaller than 0
        if (alignedSpeed > 0f)
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        
        // jump in direction accounting for normal of whats being stood on
        _rigidbody.AddForce(contactNormal * jumpSpeed, ForceMode.VelocityChange);

        // reset contact normal for combining normals
        contactNormal = Vector3.zero; */

        // turn yourself off
        enabled = false;
    }

    /* private void OnCollisionEnter(Collision other)
    {
        EvaluateCollision(other);
    }

    private void OnCollisionStay(Collision other)
    {
        EvaluateCollision(other);
    }

    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            // for altering jump direction to account for slopes
            if (normal.y >= minGroundDotProduct)
            {
                contactNormal = normal;
            }
            // incase not grounded, just go default direction
            else
            {
                contactNormal = direction;
            }
        }
    } */
}
