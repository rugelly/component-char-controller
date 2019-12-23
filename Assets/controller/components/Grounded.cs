using System;
using System.Linq;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    #region first and mostly working imp
    /* [SerializeField]
    private bool _check; // serialized for debug
    [SerializeField]
    private bool _slope; // serialized for debug
    [SerializeField]
    private bool _tooSteep; // serialized for debug */

    /* public bool check {get => _check;}
    public bool slope {get => _slope;}
    public bool tooSteep {get => _tooSteep;}
    public Vector3 slopeNormals {get => contactVector;} */

    /* private Vector3[] rays = new Vector3[5];
    private bool[] rayStatus = new bool[5];
    private Vector3 contactVector; // combine normals the rays are touching
    private Vector3[] normalStorage = new Vector3[5]; // pretty dirty imp here but whatever
    */
    #endregion first and mostly working imp

    [SerializeField]
    private float maxSlopeAngle = 44;
    private CapsuleCollider _collider;
    float minGroundDotProduct;
    int groundContactCount, steepContactCount;
    int stepsSinceLastGrounded;
    [SerializeField] private bool grounded; // DEBUG
    [SerializeField] private bool steep; // DEBUG
    public bool isGrounded {get => groundContactCount > 0;}
    public bool isSteep {get => steepContactCount > 0;}
    public Vector3 contactNormal {get => _contactNormal;} private Vector3 _contactNormal;
    Vector3 steepNormal;

    private void Start()
    {
        #region first and mostly working imp
        /* float shift = 0.2f;
        rays[0] = Vector3.zero;
        rays[1] = Vector3.right * shift;
        rays[2] = Vector3.left * shift;
        rays[3] = Vector3.forward * shift;
        rays[4] = Vector3.back * shift; */
        #endregion first and mostly working imp

        minGroundDotProduct = Mathf.Cos(maxSlopeAngle * Mathf.Deg2Rad);
    }

    private void OnEnable()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    #region first and mostly working imp
    /*private void FixedUpdate()
    {
        #region first and mostly working imp
        //_check = CheckGrounded();

        for (int i = 0; i < rays.Length; i++)
            rayStatus[i] = CheckNormalAngle(i)[0];
        _slope = rayStatus.Contains(true);

        for (int i = 0; i < rays.Length; i++)
            rayStatus[i] = CheckNormalAngle(i)[1];
        _tooSteep = rayStatus.Contains(true);

        contactVector = AggregateNormals(); 
        #endregion first and mostly working imp

    }
    private bool CheckGrounded()
    {
        int mask = 1 << 8; // only layer 8
        mask = ~mask; // invert the bitmask to get every layer EXCEPT 8
        Collider[] underneath = Physics.OverlapSphere(transform.position + (Vector3.up * 0.36f), 0.43f, mask);
        return underneath == null || underneath.Length == 0 ? false : true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position + (Vector3.up * 0.36f), 0.43f);
    } */
    #endregion first and mostly working imp
    #region first and mostly working imp
    /* private bool[] CheckNormalAngle(int index)
    {
        RaycastHit hit;
        float length = rays[index] == rays[0] ? 0.13f : 0.081f; // length to match OverlapSphere
        float groundSlopeAngle;
        Vector3 rayToWorld = transform.TransformDirection(rays[index]);

        if (Physics.Raycast(transform.position + rayToWorld + (Vector3.up * 0.06f), Vector3.down, out hit, length))
            Debug.DrawRay(transform.position + rayToWorld + (Vector3.up * 0.06f), Vector3.down * length, Color.yellow);

        groundSlopeAngle = Vector3.Angle(hit.normal, Vector3.up);

        normalStorage[index] = groundSlopeAngle == 0 || groundSlopeAngle > maxSlopeAngle ? Vector3.zero : hit.normal;

        bool[] status = new bool [2];
        status[0] = groundSlopeAngle == 0 ? false : true;
        status[1] = groundSlopeAngle > maxSlopeAngle ? true : false;
        return status;
    } */
    #endregion first and mostly working imp
    #region first and mostly working imp
    /* private Vector3 AggregateNormals()
    {
        return normalStorage[0] + normalStorage[1] + normalStorage[2] + normalStorage[3] + normalStorage[4];
    } */
    #endregion first and mostly working imp

    private void FixedUpdate()
    {
        grounded = isGrounded; // DEBUG
        steep = isSteep; // DEBUG

        stepsSinceLastGrounded += 1;
        if (isGrounded || isSteep)
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
        float minimumHeight = _collider.bounds.min.y + _collider.radius;
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
