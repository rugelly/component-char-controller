using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private PlayerStats _stats;
    private CapsuleCollider _collider;

    [HideInInspector] public AnimationCurve transitionCurve; // changed from scripts that enable this behaviour
    [HideInInspector] public bool isCrouching; // true = want to crouch, false = want to stand
    private bool running; // so the coroutine can only activate once
    private bool canActivate; // if true the coroutine is actually able to be ran

    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;
        _collider = GetComponent<CapsuleCollider>();

        // check if collider height already matches the height it should be transitioning to
        // if it matches, just exit without doing anything

        canActivate = false; // cannot crouch unless otherwise proven true
        running = false; // coroutine is not curently active

        // want to crouch, but already crouching
        if (isCrouching && _collider.height == _stats.crouchHeight)
        {
            // perform nothing and leave
            enabled = false;
        }
        // want to stand but already standing
        else if (!isCrouching && _collider.height == _stats.standHeight)
        {
            // do nothing and leave
            enabled = false;
        }
        else
        {
            canActivate = true;
        }
        
    }

    private void FixedUpdate()
    {
        if (!running && canActivate)
        {
            if (isCrouching)
            {
                StartCoroutine(CrouchRoutine(_stats.standHeight, _stats.crouchHeight, 
                transitionCurve, _stats.crouchTime));
            }
            else if (!isCrouching)
            {
                StartCoroutine(CrouchRoutine(_stats.crouchHeight, _stats.standHeight, 
                transitionCurve, _stats.crouchTime));
            }
        }
    }

    private IEnumerator CrouchRoutine(float start, float end, AnimationCurve ac, float length)
    {
        running = true; // prevent duplicate coroutines
        float timer = 0f;
        while (timer <= length)
        {
            _collider.height = Mathf.Lerp(start, end, ac.Evaluate(timer/length));
            timer += Time.deltaTime;
            yield return null;
        }
        //Debug.Log("IS THIS LINE EVEN HIT? I DOUBT IT CHECK CHECK MIC CHECK");
        //enabled = false;
    }

    // some kind of NaN error when calculating transform matrix for collider center
    /* private IEnumerator CrouchRoutine()
    {
        float crouchParam = 0f;
        float smoothCrouchParam = 0f;

        float crouchSpeed = 1f / _stats.crouchTime;
        float currentHeight = _collider.height;
        Vector3 currentCenter = _collider.center;

        float desiredHeight = isCrouching ? initPlayerHeight : _stats.crouchHeight;
        // center is half the difference between playerheight and crouchheight
        Vector3 desiredCenter = isCrouching ? initCenter : new Vector3(0, ((initPlayerHeight - _stats.crouchHeight) / 2), 0);

        //Vector3 camPos = playerCamera.localPosition;
        //float camCurrentHeight = camPos.y;
        //float camDesiredHeight = isCrouching ? initCamHeight : crouchCamHeight;

        while (crouchParam < 1f)
        {
            crouchParam += Time.deltaTime * crouchSpeed;
            smoothCrouchParam = transitionCurve.Evaluate(crouchParam);

            _collider.height = Mathf.Lerp(currentHeight, desiredHeight, smoothCrouchParam);
            _collider.center = Vector3.Lerp(currentCenter, desiredCenter, smoothCrouchParam);

            //camPos.y = Mathf.Lerp(camCurrentHeight, camDesiredHeight, smoothCrouchParam);
            //playerCamera.localPosition = camPos;

            yield return null;
        }

        enabled = false; // turn itself off when done
    } */
}
