using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private PlayerStats _stats;
    private InputReader _input;
    private Rigidbody _rigidbody;
    public GameObject camRootNode;

    private void Awake()
    {
        // avoid character controller snapping to 0,0,0 rotation on start
        rotation.y = transform.position.y;
    }

    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;
        _input = GetComponent<InputReader>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private Vector2 rotation;

    private void Update()
    {
        rotation.y = _input.lookHorizontal;
        rotation.x += _input.lookVertical;
        rotation.x = Mathf.Clamp(rotation.x, _stats.vertClampMin, _stats.vertClampMax);
        camRootNode.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
    }

    private void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, rotation.y, 0));
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }
}
