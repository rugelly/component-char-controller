using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private Health _health;

    public bool landingFirm {get{return impact >= 5 && impact < 10;}} // reset accel mult to 0 regardless of input direction
    public bool landingHard {get{return impact >= 10 && impact < 15;}} // reset accel mult to 0, slow motor for a short period
    public bool landingSplat {get{return impact >= 15;}} // reset accel mult to 0, slow motor, take fall dmg
    private float impact;

    private void OnEnable()
    {
        _health = GetComponent<Health>();
    }

    private void OnCollisionEnter(Collision col)
    {
        float falldmg = impact = col.impulse.magnitude;
        if (landingSplat)
        {
            falldmg -= impact;
            falldmg *= falldmg;
            _health.Hurt(falldmg);
        }
    }
}
