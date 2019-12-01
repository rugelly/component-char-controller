﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // rotates the base controller on the horizontal, and whatever camera it finds in children
    // on the vertical
    private InputReader _input;
    private PlayerStats _stats;
    private Camera cam;

    private Quaternion startRotation; // set to facing whenever this component enabled
    private float rot; // temp for the rotate function sorry 

    void OnEnable()
    {
        _input = GetComponentInParent<InputReader>();
        _stats = GetComponentInParent<StatHolder>().held;

        startRotation = transform.localRotation;

        cam = GetComponentInChildren<Camera>();
    }


    void Update()
    {

        transform.localRotation = RotateThis(_input.lookHorizontal, -360, 360, Vector3.up);

        cam.transform.localRotation = RotateThis(_input.lookVertical, _stats.vertClampMin, _stats.vertClampMax, -Vector3.right);

        /* xRotation += _input.lookHorizontal;
        yRotation += _input.lookVertical;

        xRotation = ClampAngle(xRotation, -360, 360);
        yRotation = ClampAngle(yRotation, _stats.vertClampMin, _stats.vertClampMax);

        Quaternion xQuat = Quaternion.AngleAxis(xRotation, Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(yRotation, -Vector3.right);

        transform.localRotation = startRotation * xQuat * yQuat; */
    }

    // vague attempt at cleanly reclying the above commented out block
    private Quaternion RotateThis(float input, float clampMin, float clampMax, Vector3 rotateAround)
    {
        rot += input;
        rot = ClampAngle(rot, clampMin, clampMax);
        Quaternion quat = Quaternion.AngleAxis(rot, rotateAround);
        return startRotation * quat;
    }

    public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
