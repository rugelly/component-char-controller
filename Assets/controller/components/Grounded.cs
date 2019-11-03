using UnityEngine;

public class Grounded : MonoBehaviour
{
    private bool _check;
    public bool check {get => _check;}

    private Vector3 gravity = Vector3.down * 9.81f;

    // TODO: some kind of function to keep "grounded == true" 
    // for a very small delay after not actually being grounded anymore
    // like the mario jumping off edge forgiveness thingy

    /// new imp does it work right?
    // seems to work like:
    // true if collision stay with face normal pointing up? ish?
    void OnCollisionStay(Collision other)
    {
        _check = Vector3.Angle (other.contacts[0].normal, gravity) > 100f;
    }

    /// temp and kind of shitty imp
    /* private void OnTriggerEnter()
    {
        _check = true;
    }
    private void OnTriggerExit()
    {
        _check = false;
    } */

    /// old and buggy ass imp
    /* {
        get
        {
            if (_check)
            {
                _check = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void OnCollisionStay()
    {
        _check = true;
    }
    */
}
