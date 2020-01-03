using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private Health _health;

    [HideInInspector] public bool landingFirm; // reset accel mult to 0 regardless of input direction
    [HideInInspector] public bool landingHard; // reset accel mult to 0, slow motor for a short period
    [HideInInspector] public bool landingSplat; // reset accel mult to 0, slow motor, take fall dmg

    private float impact;
    private float low = 13;
    private float mid = 15;
    private float high = 18;

    private void OnEnable()
    {
        _health = GetComponent<Health>();
    }

    private void OnCollisionEnter(Collision col)
    {
        float falldmg = col.impulse.magnitude;

        if (falldmg >= low && falldmg < mid)
        {
            landingFirm = true;
            impact = low;
        }
        else if (falldmg >= mid && falldmg < high)
        {
            landingHard = true;
            impact = mid;
        }
        else if (falldmg >= high)
        {
            landingSplat = true;
            impact = high;

            falldmg -= impact;
            falldmg = falldmg * falldmg * falldmg;
            _health.Hurt(falldmg);
        }
    }
}
