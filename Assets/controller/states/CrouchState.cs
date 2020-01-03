using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSimple;

public class CrouchState : State
{
    
    public CrouchState(StateMachine stateMachine) : base(stateMachine)
    {}

    private Motor _motor;
    private Grounded _grounded;
    private Jump _jump;
    private InputReader _input;
    private Crouch _crouch;
    private CapsuleCollider _collider;
    private PlayerStats _stats;

    public override void OnStateEnter()
    {
        #region get comps
        _motor = stateMachine.GetComponent<Motor>();
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _input = stateMachine.GetComponent<InputReader>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _jump = stateMachine.GetComponent<Jump>();
        _crouch = stateMachine.GetComponent<Crouch>();
        _collider = stateMachine.GetComponent<CapsuleCollider>();
        #endregion

        #region change motor vals
        _motor.speed = _stats.crouchSpeed;
        _motor.accelRate = _stats.crouchAccelRate;
        #endregion
    }

    public override void OnStateExit()
    {
        
    }

    public override void Tick()
    {
        if (!_grounded.isGrounded)
            stateMachine.SetState(new AirCrouchState(stateMachine));

        if (_crouch.hasHeadroom)
        {
            if (_input.moveVertical > 0 && _input.sprint)
            {
                _crouch.crouching = false;
                stateMachine.SetState(new SprintState(stateMachine));
            }   
            else if (_input.crouch)
            {
                _crouch.crouching = false;
                stateMachine.SetState(new NormalState(stateMachine));
            }
        }
    }
}
