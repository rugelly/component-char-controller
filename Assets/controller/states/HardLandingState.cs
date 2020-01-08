using UnityEngine;
using StateMachineSimple;

public class HardLandingState : State
{
    private Motor _motor;
    private Grounded _grounded;
    private PlayerStats _stats;
    private FallDamage _falldmg;
    private Jump _jump;
    private Crouch _crouch;
    private float timeToExit;
    private float timer;

    public HardLandingState(StateMachine stateMachine) : base(stateMachine)
    {}

    public override void OnStateEnter()
    {
        _motor = stateMachine.GetComponent<Motor>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _falldmg = stateMachine.GetComponent<FallDamage>();
        _jump = stateMachine.GetComponent<Jump>();
        _crouch = stateMachine.GetComponent<Crouch>();

        _crouch.crouching = true;

        if (_falldmg.landingFirm)
        {
            _motor.accelMult = Vector3.zero;
            timeToExit = 0.01f;

            _falldmg.landingFirm = false;
        }
        else if (_falldmg.landingHard)
        {
            _motor.accelMult = Vector3.zero;
            _motor.speed = _stats.runSpeed * 0.6f;
            _motor.accelRate = _stats.runAccelRate * 0.6f;
            timeToExit = 0.9f;

            _jump.disableOverride = true;

            _falldmg.landingHard = false;
        }
        else if (_falldmg.landingSplat)
        {
            _motor.accelMult = Vector3.zero;
            _motor.speed = _stats.runSpeed * 0.2f;
            _motor.accelRate = _stats.runAccelRate * 0.2f;
            timeToExit = 1.5f;

            _jump.disableOverride = true;

            _falldmg.landingSplat = false;
        }

        timer = 0;
    }

    public override void OnStateExit()
    {
        _jump.disableOverride = false;
    }

    public override void Tick()
    {
        timer += 1 * Time.deltaTime;
        timer = Mathf.Clamp(timer, 0, timeToExit);

        if (timer > (timeToExit * 0.75f))
            _crouch.crouching = false;

        if (timer == timeToExit)
            stateMachine.SetState(new NormalState(stateMachine));
    }
}
