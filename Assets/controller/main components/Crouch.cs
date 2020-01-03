using UnityEngine;

public class Crouch : MonoBehaviour
{
    private InputReader _inputReader;
    private CapsuleCollider _collider;
    private PlayerStats _stats;

    private void OnEnable()
    {
        _inputReader = GetComponent<InputReader>();
        _collider = GetComponent<CapsuleCollider>();
        _stats = GetComponent<StatHolder>().held;

        height = 1;
        center = 1;
    }

    public bool crouching {get{return _crouching;} set{_crouching = value;}}
    public bool crouched {get{return _crouched;}}
    public bool standing {get{return _standing;}}
    private bool _crouching;
    private bool _crouched, _standing;
    private float height;
    private float center;
    private float percent;

    private void Update()
    {
        _crouched = _collider.height == _stats.crouchHeight;
        _standing = _collider.height == _stats.standHeight;

        // multiply the height by a multiplier that goes from percentage (crouchheight/standheight)-> 1(fully standing)
        // center is the same
        height += _crouching ? -_stats.crouchTime * Time.deltaTime : _stats.crouchTime * Time.deltaTime;
        height = Mathf.Clamp(height, (_stats.crouchHeight / _stats.standHeight), 1);
        center += _crouching ? -_stats.crouchTime * Time.deltaTime : _stats.crouchTime * Time.deltaTime;
        center = Mathf.Clamp(center, (_stats.crouchHeight / _stats.standHeight), 1);

        _collider.height = Mathf.Clamp(_stats.standHeight * height, _stats.crouchHeight, _stats.standHeight);
        _collider.center = new Vector3(0, Mathf.Clamp(1 * center, (_stats.crouchHeight / _stats.standHeight), 1), 0);
    }
}

/* #region possibly better imp
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

        // base centers height off crouch height TODO: UN-HARDCODE THIS VALUE?
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
#endregion */