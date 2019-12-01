using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private PlayerStats _stats;
    private CapsuleCollider _collider;

    [HideInInspector] public AnimationCurve transitionCurve; // changed from scripts that enable this behaviour
    [HideInInspector] public float toHeight;

    private float tmpHeight; // just a temp holder for readability
    private float timer;
    private float startHeight;

    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;
        _collider = GetComponent<CapsuleCollider>();
        timer = 0f;
        startHeight = _collider.height;
    }

    private void Update()
    {
        timer += 1f * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        tmpHeight = Mathf.Lerp(startHeight, toHeight, timer);
        _collider.height = Mathf.Clamp(tmpHeight, _stats.crouchHeight, _stats.standHeight);

        if (_collider.height == toHeight)
        {
            enabled = false;
        }
    }
}
