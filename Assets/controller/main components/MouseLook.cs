using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private PlayerStats _stats;
    private InputReader _input;
    public GameObject camRootNode;

    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;
        _input = GetComponent<InputReader>();
    }

    private Vector2 rotation;

    private void Update()
    {
        rotation.y += _input.lookHorizontal;
        rotation.x += _input.lookVertical;
        rotation.x = Mathf.Clamp(rotation.x, _stats.vertClampMin, _stats.vertClampMax);
        transform.eulerAngles = new Vector2(0, rotation.y); // rotate the base level with horizontal, on horizontal
        camRootNode.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0); // rotate child level found cam with vert, on vert
    }
}
