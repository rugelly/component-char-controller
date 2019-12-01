using UnityEngine;

public class Slide : MonoBehaviour
{
    Rigidbody _rigidbody;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * 5f, ForceMode.VelocityChange);
        
        enabled = false;
    }
}
