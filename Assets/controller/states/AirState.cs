using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSimple;

public class AirState : State
{
    private InputReader _input;
    private PlayerStats _stats;
    private Motor _motor;
    private Grounded _grounded;
    private EdgeDetect _edgeDetect;
    private Climb _climb;
    private Crouch _crouch;
    private FallDamage _falldmg;

    private bool toggle;

    public AirState(StateMachine stateMachine) : base(stateMachine)
    {}

    public override void OnStateEnter()
    {
        _input = stateMachine.GetComponent<InputReader>();
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _motor = stateMachine.GetComponent<Motor>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _edgeDetect = stateMachine.GetComponent<EdgeDetect>();
        _edgeDetect.enabled = true;
        _climb = stateMachine.GetComponent<Climb>();
        toggle = false;
        _crouch = stateMachine.GetComponent<Crouch>();
        _falldmg = stateMachine.GetComponent<FallDamage>();

        #region change motor vals
        _motor.speed = _stats.airSpeed;
        _motor.accelRate = _stats.airAccelRate;
        _motor.sprintHorizontalInputReductionMult = 1f;
        #endregion
    }

    public override void OnStateExit()
    {
        _edgeDetect.enabled = false;
    }

    public override void Tick()
    {
        if (_crouch.crouching || _crouch.crouched)
            stateMachine.SetState(new AirCrouchState(stateMachine));

        if (_grounded.isGrounded)
        {
            if (_input.wasSprinting)
                stateMachine.SetState(new SprintState(stateMachine));
            else
                stateMachine.SetState(new NormalState(stateMachine));
        }

        if (_input.moveVertical > 0)
        {
            if (_edgeDetect.canClimbAndStand && !toggle)
            {
                toggle = true;
                _climb.enabled = true;
            }
        }

        /* // uncrouch if you jump in air and theres room
        if (_crouch.hasHeadroom)
        {
            _edgeDetect.enabled = true;
            if (_input.jump)
                _crouch.crouching = false;
        }
        else
            _edgeDetect.enabled = false;

        if (_grounded.isGrounded)
        {
            if (_crouch.crouched)
            {
                stateMachine.SetState(new CrouchState(stateMachine));
            }
            
            if (_crouch.hasHeadroom)
            {
                if (_input.wasSprinting)
                {
                    _crouch.crouching = false;
                    stateMachine.SetState(new SprintState(stateMachine));
                }
                else if (_crouch.standing || !_crouch.crouching)
                {
                    _crouch.crouching = false;
                    stateMachine.SetState(new NormalState(stateMachine));
                }
            }
        }

        if (_input.moveVertical > 0)
        {
            if (_edgeDetect.canClimbAndStand && !toggle)
            {
                toggle = true;
                _climb.enabled = true;
            }
        } */
    }
}
