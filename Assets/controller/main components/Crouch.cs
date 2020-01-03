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
    public bool hasHeadroom {get{return _headroom;}}
    private bool _crouching, _crouched, _standing, _headroom;
    private float height, center, percent;

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

        if (!standing)
            _headroom = CheckHeadroom();
    }

    private bool CheckHeadroom()
    {
        Debug.DrawRay(
            transform.position + new Vector3(0, _collider.height - 0.4f, 0),
            Vector3.up * (_stats.standHeight - _collider.height + 0.4f),
            Color.red);

        // THIS SOMETIMES DIDNT WORK AND YOU COULD UN CROUCH INSIDE GEO
        // TODO: MORE ROBUST SOLUTION
        /* return !Physics.SphereCast(
            // cast right from the top of collider (but shift it down a bit just in case)
            transform.position + new Vector3(0, _collider.height - 0.4f, 0),
            // cast same size as player
            _collider.radius,
            Vector3.up,
            out RaycastHit hit,
            // length of ray dynamically scales (also shifted to match origin)
            _stats.standHeight - _collider.height + 0.4f); */
        return !Physics.Raycast(transform.position + (Vector3.up * (_collider.height - 0.4f)), Vector3.up, _stats.standHeight - _collider.height + 0.4f);
    }
}