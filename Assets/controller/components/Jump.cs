using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [HideInInspector] public float strength;
    [HideInInspector] public Vector3 direction;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * strength);
        jumpSpeed = Mathf.Max(jumpSpeed - _rigidbody.velocity.y, 0);
        _rigidbody.AddForce(direction * jumpSpeed, ForceMode.VelocityChange);

        enabled = false;
    }
}
