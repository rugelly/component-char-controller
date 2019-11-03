using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private InputReader _input;

    [HideInInspector] public float strength;
    [HideInInspector] public Vector3 direction;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<InputReader>();
    }

    private void FixedUpdate()
    {
        if (_input.jump)
        {
            _rigidbody.AddForce((direction * strength), ForceMode.Impulse);
        }
    }
}
