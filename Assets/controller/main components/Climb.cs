using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    private Vector3 castPoint;
    private PlayerStats _stats;
    private Rigidbody _rigidbody;

    public Vector3 toPosition {set{_endPoint = value;}}
    private Vector3 _endPoint;
    private float count;
    private float scaledTime; // start close to endpoint? get there faster
    private float scaledForce; // not as much force for the shorter climbs

    private Vector3 cp1, ch1, cp2, ch2; // climb point 1, handle 1, etc

    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;
        _rigidbody = GetComponent<Rigidbody>();

        cp1 = transform.position;
        ch1 = transform.TransformDirection(new Vector3(0, 0, -0.2f) + new Vector3(0, 0.5f, 0));
        ch1 += transform.position;
        cp2 = _endPoint;
        ch2 = transform.TransformDirection(new Vector3(0, 0, -0.3f) + new Vector3(0, 0.1f, 0));
        ch2 += _endPoint;

        // these values are annoyingly fine tuned
        // in combination with: 
        // climbtime = 0.7
        // climbforce = 2
        scaledTime = Mathf.Clamp(cp2.y - cp1.y, 0.8f, 1) * _stats.climbTime;
        scaledForce = Mathf.Clamp(cp1.y - cp2.y, -1, -2) * -_stats.climbForce;

        count = 0;
        _rigidbody.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        float pos = count / scaledTime;
        Vector3 pointAt = transform.position - CalculateBezierPoint(pos, cp1, ch1, ch2, cp2);
        count += 1 * Time.deltaTime;
        count = Mathf.Clamp(count, 0, scaledTime);
        if (count < scaledTime)
            _rigidbody.AddForce(-pointAt * scaledForce, ForceMode.VelocityChange);
        else
            enabled = false;
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