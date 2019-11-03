using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetect : MonoBehaviour
{
    private Vector3 castPoint;
    private PlayerStats _stats;
    private RaycastHit hit;
    // TODO: get these from stats or something please!
    private float rayLength = 2f;
    private float standHeight = 1f;
    private float crouchHeight = 0.6f;

    [HideInInspector] public bool canClimbAndStand;
    [HideInInspector] public bool canClimbAndCrouch;

    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;
    }

    private void Update()
    {
        // get dynamic points slightly infront and above the controller
        castPoint = (transform.forward * _stats.ledgeReach.x) + new Vector3(0, _stats.ledgeReach.y, 0);
        
        // cast down for flat surface
        Debug.DrawRay(castPoint, Vector3.down * rayLength, Color.magenta);
        if (Physics.Raycast(castPoint, Vector3.down * rayLength, out hit))
        {
            // surface detected
            // check if collider fits at full height
            // does not fit
            if (CustomCapsuleCast(hit.point, standHeight))
            {
                canClimbAndStand = false;
                // check if player fits crouching
                // does not fit
                if (CustomCapsuleCast(hit.point, crouchHeight))
                {
                    Debug.DrawRay(hit.point, Vector3.up * crouchHeight, Color.red);
                    canClimbAndCrouch = false;
                }
                // does fit
                else
                {
                    Debug.DrawRay(castPoint, Vector3.up * crouchHeight, Color.green);
                    // this space can be climbed up onto WHILE CROUCHED
                    canClimbAndCrouch = true;
                }
            }
            // does fit
            else
            {
                Debug.DrawRay(castPoint, Vector3.up * standHeight, Color.green);
                // this space can be climbed up onto WHILE STANDING
                canClimbAndStand = true;
            }
        }
        else // no surface hit
        {
            canClimbAndCrouch = false;
            canClimbAndStand = false;
        }
        
    }

    // TODO: SHOULD THIS JUST BE A CYLINDER CAST??

    // function to make reading above much cleaner
    // tries to use values to make the capsule the same size as the player collider
    // note: the "start" and "end" points need to be thought of as the "center" of a sphere
    // which is why they are not at the exact start and end of the height
    private bool CustomCapsuleCast(Vector3 hitPoint, float height)
    {
        if (Physics.CapsuleCast(
            hitPoint + new Vector3(0, height * 0.2f, 0), 
            hitPoint + new Vector3(0, height * 0.8f, 0),
            0.5f, Vector3.up, height))
            {
                return true;
            }
            else
            {
                return false;
            }
    }
}
