using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headroom : MonoBehaviour
{
    // CAN THIS CONTROLLER UNCROUCH? Stay tuned to find out.
    private CapsuleCollider _collider;
    private PlayerStats _stats;

    [SerializeField]
    private bool _check;
    // INVERT THIS AKA: 
    // IS THERE HEADROOM (ray hits nothing)? THEN ITS TRUE
    public bool check {get => !_check;} 

    private RaycastHit hit;

    private void OnEnable()
    {
        _collider = GetComponent<CapsuleCollider>();
        _stats = GetComponent<StatHolder>().held;
    }

    private void Update()
    {
        if (_collider.height != _stats.standHeight)
        {
            _check = Physics.SphereCast
                (
                // cast right from the top of collider (but shift it down a bit just in case)
                transform.position + new Vector3(0, _collider.height - 0.4f, 0),
                // cast same size as player
                _collider.radius,
                Vector3.up,
                out hit,
                // length of ray dynamically scales (also shifted to match origin)
                _stats.standHeight - _collider.height + 0.4f
                );

            Debug.DrawRay(
                transform.position + new Vector3(0, _collider.height - 0.4f, 0), 
                Vector3.up * (_stats.standHeight - _collider.height + 0.4f), 
                Color.red);
        }
    }
}
