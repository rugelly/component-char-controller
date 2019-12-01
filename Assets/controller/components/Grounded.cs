using UnityEngine;

public class Grounded : MonoBehaviour
{
    [SerializeField]
    private bool _check;
    public bool check {get => _check;}

    //private Vector3 gravity = Vector3.down * 9.81f;

    // TODO: some kind of function to keep "grounded == true" 
    // for a very small delay after not actually being grounded anymore
    // like the mario jumping off edge forgiveness thingy

    /// new imp does it work right?
    // seems to work like:
    // true if collision stay with face normal pointing up? ish?
    /* void OnCollisionStay(Collision other)
    {
        _check = Vector3.Angle(other.contacts[0].normal, gravity) > 100f;
    } */

    RaycastHit hit;
    Vector3 posMod = new Vector3(0, 0.5f, 0);

    void FixedUpdate()
    {
        _check = Physics.SphereCast(transform.position + posMod, 0.3f, -transform.up, out hit, 0.51f);
    }
}
