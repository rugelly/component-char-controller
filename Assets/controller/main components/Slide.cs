using System.Collections;
using UnityEngine;

public class Slide : MonoBehaviour
{
    Rigidbody _rigidbody;

    [HideInInspector] public float duration;
    [HideInInspector] public float strength;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(SlideCoroutine());
    }

    private IEnumerator SlideCoroutine()
    {
        _rigidbody.AddForce(transform.forward * strength, ForceMode.VelocityChange);

        yield return new WaitForSeconds(duration);

        enabled = false;
    }
}
