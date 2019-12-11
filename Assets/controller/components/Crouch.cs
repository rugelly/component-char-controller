using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region old imp
/* public class Crouch : MonoBehaviour
{
    private PlayerStats _stats;
    private CapsuleCollider _collider;

    [HideInInspector] public AnimationCurve transitionCurve; // changed from scripts that enable this behaviour
    [HideInInspector] public float toHeight;
    [HideInInspector] public float toCenter;

    private float tmpHeight; // just a temp holder for readability
    private float tmpCenter; // temp holder
    private float timer;
    private float startHeight;
    private float startCenter;

    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;
        _collider = GetComponent<CapsuleCollider>();
        timer = 0f;
        startHeight = _collider.height;
        startCenter = _collider.center.y;

        if (toHeight == _stats.crouchHeight)
            toCenter = 0.7f;
        else
            toCenter = 1f;
    }

    private void Update()
    {
        timer += _stats.crouchTransSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        tmpHeight = Mathf.Lerp(startHeight, toHeight, timer);
        _collider.height = Mathf.Clamp(tmpHeight, _stats.crouchHeight, _stats.standHeight);

        // also shift the center so the transform position stays in the same spot
        tmpCenter = Mathf.Lerp(startCenter, toCenter, timer);
        _collider.center = new Vector3(0, tmpCenter, 0);

        if (_collider.height == toHeight)
        {
            enabled = false;
        }
    }
} */
#endregion

#region possibly better imp
public class Crouch : MonoBehaviour
{
    private PlayerStats _stats;
    private CapsuleCollider _collider;

    [HideInInspector] public float toHeight;
    [HideInInspector] public float speedOverride;
    private float toCenter;

    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;
        _collider = GetComponent<CapsuleCollider>();

        #region EARLY EXIT
        if (toHeight == _collider.height)
            enabled = false;
        #endregion

        // base centers height off crouch height (TODO: UN-HARDCODE THIS VALUE?)
        if (toHeight == _stats.crouchHeight)
            toCenter = 0.7f;
        else
            toCenter = 1f;

        // if has not been overriden, set to stats value
        if (speedOverride == 1f)
            speedOverride = _stats.crouchTime;

        StartCoroutine(CrouchCoroutine(toCenter, toHeight));
    }

    private IEnumerator CrouchCoroutine(float center, float height)
    {
        // halfway through the duration instantly shrink half the way between standing and crouching
        // 0.85 is halfway between 1 and 0.7
        yield return new WaitForSeconds(speedOverride / 2f);
        _collider.center = new Vector3(0, 0.85f, 0);
        _collider.height = (_stats.crouchHeight + _stats.standHeight) / 2f;

        // at the end of the duration, go to the requested end position
        yield return new WaitForSeconds(speedOverride);
        _collider.center = new Vector3(0, center, 0);
        _collider.height = height;

        // reset speed override
        speedOverride = 1f;

        // turn this component off
        enabled = false;
    }
}
#endregion