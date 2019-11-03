using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    private Vector3 castPoint;
    private PlayerStats _stats;
    private RaycastHit hit;
    // TODO: get these from stats or something please!
    private float rayLength = 2f;
    private float standHeight = 1f;
    private float crouchHeight = 0.6f;

    private Vector3 cp1, ch1, cp2, ch2; // climb point 1, handle 1, etc

    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;

        #region duplicate raycast shit from EdgeDetect
        // get dynamic points slightly infront and above the controller
        castPoint = (transform.forward * _stats.ledgeReach.x) + new Vector3(0, _stats.ledgeReach.y, 0);

        // cast down for flat surface
        Debug.DrawRay(castPoint, Vector3.down * rayLength, Color.magenta);
        if (Physics.Raycast(castPoint, Vector3.down * rayLength, out hit))
        {
            // the ray should definitely be hitting something so uh idk???
        }
        #endregion

        cp1 = transform.position;
        ch1 = (transform.forward * -0.5f) + new Vector3(0, 0.5f, 0);
        cp2 = hit.point;
        ch2 = (hit.transform.forward * -0.8f) + new Vector3(0, 0.3f, 0);
    }

    private void Update()
    {
        for (float i = 0; i < 1000; i++)
        {
            Debug.Log(CalculateBezierPoint(i * 0.001f, cp1, ch1, ch2, cp2));
            if (i == 1000)
                enabled = false;
        }
    }

    // p0 is the start POINT of the line, p1 is its HANDLE
    // p2 is the HANDLE of the second point, p3 is the second POINT
    // pass in t for time and return your position on the line at that time
    // for now assume t is 0-1 (0% - 100%)
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; //first term
        p += 3 * uu * t * p1; //second term
        p += 3 * u * tt * p2; //third term
        p += ttt * p3; //fourth term

        return p;
    }
}