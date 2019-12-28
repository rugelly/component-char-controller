using System;
using System.Linq;
using UnityEngine;

public class OldGrounded : MonoBehaviour
{
    [SerializeField]
    private bool _check; // serialized for debug
    [SerializeField]
    private bool _slope; // serialized for debug
    [SerializeField]
    private bool _tooSteep; // serialized for debug */

    public bool check {get => _check;}
    public bool slope {get => _slope;}
    public bool tooSteep {get => _tooSteep;}
    public Vector3 slopeNormals {get => contactVector;}

    [SerializeField]
    private float maxSlopeAngle = 44;
    private Vector3[] rays = new Vector3[5];
    private bool[] rayStatus = new bool[5];
    private Vector3 contactVector; // combine normals the rays are touching
    private Vector3[] normalStorage = new Vector3[5]; // pretty dirty imp here but whatever

    private void Start()
    {
        float shift = 0.2f;
        rays[0] = Vector3.zero;
        rays[1] = Vector3.right * shift;
        rays[2] = Vector3.left * shift;
        rays[3] = Vector3.forward * shift;
        rays[4] = Vector3.back * shift;
    }

    private void FixedUpdate()
    {
        //_check = CheckGrounded();

        for (int i = 0; i < rays.Length; i++)
            rayStatus[i] = CheckNormalAngle(i)[0];
        _slope = rayStatus.Contains(true);

        for (int i = 0; i < rays.Length; i++)
            rayStatus[i] = CheckNormalAngle(i)[1];
        _tooSteep = rayStatus.Contains(true);

        contactVector = AggregateNormals(); 

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
    }

    private bool[] CheckNormalAngle(int index)
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
    }

    private Vector3 AggregateNormals()
    {
        return normalStorage[0] + normalStorage[1] + normalStorage[2] + normalStorage[3] + normalStorage[4];
    }
}
