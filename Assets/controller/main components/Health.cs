using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float hp {get{return _hp;}}
    private float _hp;
    public float hpDelta {get{return _hpDelta;}}
    private float _hpDelta;
    public float shield {get{return _shield;}}
    private float _shield;
    public float shieldDelta {get{return _shieldDelta;}}
    private float _shieldDelta;
    [HideInInspector] public bool dead {get{return hp == 0;}}
    private PlayerStats _stats;

    private void Awake()
    {
        _stats = GetComponent<StatHolder>().held;
        
        _hp = _stats.maxHealth;
        _shield = _stats.maxShield;
    }

    public void Hurt(float amount)
    {
        if (_shield > 0)
        {
            _shield -= amount;
            _shieldDelta = Mathf.Clamp(_shield - amount, 0, _stats.maxShield);
            // any negative overflow? subtract it (by adding) from the hp next
            if (_shield < 0)
                _hp += _shield;
        }
        else
        {
            _hp -= amount;
            _hp = Mathf.Clamp(_hp, 0, _stats.maxHealth);
            _hpDelta = Mathf.Clamp(_hp - amount, 0, _stats.maxHealth);
        }
        
        regenTimer = 0;
    }

    public float regenTimer;
    private float regenDelayTime = 2f;
    float shieldRegenAmount = 20;
    private void RegenTimer()
    {
        if (_shield == _stats.maxShield)
            regenTimer = 0;
        else
            regenTimer += 1 * Time.deltaTime;

        regenTimer = Mathf.Clamp(regenTimer, 0, regenDelayTime);

        if (regenTimer == regenDelayTime)
            _shield += shieldRegenAmount * Time.deltaTime;
    }

    public GameObject shieldref; // TODO: REMOVE THIS
    public GameObject hpref; // TODO: REMOVE THIS

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            Hurt(30);

        shieldref.GetComponent<RectTransform>().localScale = new Vector3(shield / 100, 1, 1);
        hpref.GetComponent<RectTransform>().localScale = new Vector3(hp / 100, 1, 1);

        RegenTimer();
        _shield = Mathf.Clamp(_shield, 0, _stats.maxShield);
    }
}
