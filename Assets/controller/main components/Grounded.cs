using System;
using System.Linq;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    [SerializeField]
    private float maxSlopeAngle = 44;
    private CapsuleCollider _collider;
    private Rigidbody _rigidbody;
    private PlayerStats _stats;
    private Jump _jump;
    private InputReader _inputReader;
    private Health _health;
    float minGroundDotProduct;
    int groundContactCount, steepContactCount;
    int stepsSinceLastGrounded;
    public bool isGrounded {get => groundContactCount > 0;}
    public bool isSteep {get => steepContactCount > 0;}
    public Vector3 contactNormal {get => _contactNormal;} private Vector3 _contactNormal;
    Vector3 steepNormal;

    private void Start()
    {
        minGroundDotProduct = Mathf.Cos(maxSlopeAngle * Mathf.Deg2Rad);
    }

    private void OnEnable()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _stats = GetComponent<StatHolder>().held;
        _jump = GetComponent<Jump>();
        _inputReader = GetComponent<InputReader>();
        _health = GetComponent<Health>();
    }

    private void FixedUpdate()
    {
        stepsSinceLastGrounded += 1;
        if (isGrounded || isSteep || SnapToGround())
        {
            stepsSinceLastGrounded = 0;
            if (groundContactCount > 1)
                _contactNormal.Normalize();
        }
        else
        {
            _contactNormal = Vector3.up;
        }
        // reset some things at the end of the update
        groundContactCount = steepContactCount = 0;
        _contactNormal = steepNormal = Vector3.zero;
    }

    private bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 2)
            return false;
        float speed = _rigidbody.velocity.magnitude;
        if (speed > _stats.slopeSnapSpeed)
            return false;
        if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.25f)) // 0.2 is a good length
            return false;
        if (hit.normal.y < minGroundDotProduct) // TODO: THIS PART SEEMS TO CAUSE UNWANTED BEHAVIOUR SOMETIMES??
            return false;
        if (_inputReader.jump)
            return false;

        groundContactCount = 1;
        _contactNormal = hit.normal;
        float verticalPositionDelta = (hit.point.y - transform.position.y) * 4;
        _rigidbody.velocity += new Vector3(0, verticalPositionDelta, 0);
        return true;
    }

    private void OnCollisionEnter(Collision col)
    {
        EvaluateCollisions(col);
    }

    private void OnCollisionStay(Collision col)
    {
        EvaluateCollisions(col);
    }

    private void EvaluateCollisions(Collision col)
    {
        for (int i = 0; i < col.contactCount; i++)
        {
            Vector3 normal = col.GetContact(i).normal;
            if (normal.y >= minGroundDotProduct)
            {
                groundContactCount += 1;
                _contactNormal += normal;
            }
            else if (normal.y > -0.01f)
            {
                steepContactCount += 1;
                steepNormal += normal;
            }
        }
    }

    private bool CheckSteepContact()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            if (steepNormal.y >= minGroundDotProduct)
            {
                steepContactCount = 0;
                groundContactCount = 1;
                _contactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }
}
