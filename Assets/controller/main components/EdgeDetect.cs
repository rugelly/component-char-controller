using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetect : MonoBehaviour
{
    //private Vector3 castPoint;
    private PlayerStats _stats;
    private Climb _climb;
    private CapsuleCollider _collider;
    private RaycastHit hit;
    private float rayLength = 1.1f; // tuned to not constantly trigger on slopes with stats.ledgereach x=0.65 y=1.6

    [HideInInspector] public bool canClimbAndStand;
    //[HideInInspector] public bool canClimbAndCrouch;

    float minClimbDotProduct;
    private void Start()
    {
        float maxClimbAngle = 30f;
        minClimbDotProduct = Mathf.Cos(maxClimbAngle * Mathf.Deg2Rad);
    }

    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;
        _climb = GetComponent<Climb>();
    }

    private void Update()
    {
        // get dynamic points slightly infront and above the controller
        Vector3 castPoint = (transform.forward * _stats.ledgeReach.x) + new Vector3(0, _stats.ledgeReach.y, 0);
        castPoint = transform.TransformDirection(new Vector3(0, _stats.ledgeReach.y, _stats.ledgeReach.x));
        castPoint += transform.position;
        
        if (Physics.Raycast(castPoint, Vector3.down, out hit, rayLength))
        {
            if (hit.normal.y > minClimbDotProduct)
                if (!Physics.SphereCast(hit.point, 0.43f, Vector3.up, out RaycastHit hat, _stats.standHeight - 0.43f))
                {
                    DebugDrawSphereCastKinda(hit.point, 0.43f, Vector3.up, _stats.standHeight, Color.yellow);
                    _climb.toPosition = hit.point;
                    canClimbAndStand = true;
                    //canClimbAndCrouch = false;
                }
                else
                {
                    DebugDrawSphereCastKinda(hit.point, 0.43f, Vector3.up, rayLength, Color.red);
                    //canClimbAndCrouch = false;
                    canClimbAndStand = false;
                }
        }
        else
        {
            Debug.DrawRay(castPoint, Vector3.down * rayLength, Color.magenta);
            //canClimbAndCrouch = false;
            canClimbAndStand = false;
        }
    }

    private void DebugDrawSphereCastKinda(Vector3 origin, float radius, Vector3 direction, float length, Color colour)
    {
        Debug.DrawRay(origin,                               direction * length, colour);
        Debug.DrawRay(origin + Vector3.forward  * radius,   direction * length, colour);
        Debug.DrawRay(origin + Vector3.back     * radius,   direction * length, colour);
        Debug.DrawRay(origin + Vector3.left     * radius,   direction * length, colour);
        Debug.DrawRay(origin + Vector3.right    * radius,   direction * length, colour);
    }
}
