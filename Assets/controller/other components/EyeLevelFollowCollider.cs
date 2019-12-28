using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLevelFollowCollider : MonoBehaviour
{
    private CapsuleCollider _capsule;

    private float eyeHeight = 0.25f; // distance from top of collider
    private Vector3 targetPos;
    private Vector3 velocity;

    private void Start()
    {
        _capsule = GetComponentInParent<CapsuleCollider>();
        
    }

    private void Update()
    {
        targetPos = new Vector3(0, _capsule.height - eyeHeight, 0.1f);
        //transform.localPosition = new Vector3( 0, _capsule.height - eyeHeight, 0);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, 3 * Time.deltaTime);
    }
}
