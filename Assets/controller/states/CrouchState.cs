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
    private InputReader _inputReader;
    private Crouch _crouch;
    private Headroom _headroom;
    private CapsuleCollider _collider;
    private PlayerStats _stats;

    public override void OnStateEnter()
    {
        #region get comps
        _motor = stateMachine.GetComponent<Motor>();
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _inputReader = stateMachine.GetComponent<InputReader>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _jump = stateMachine.GetComponent<Jump>();
        _crouch = stateMachine.GetComponent<Crouch>();
        _headroom = stateMachine.GetComponent<Headroom>();
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
        {
            stateMachine.SetState(new AirState(stateMachine));
        }

        if (_headroom.check)
        {
            if (_inputReader.moveVertical > 0 && _inputReader.sprint)
            {
                _crouch.crouching = false;
                stateMachine.SetState(new SprintState(stateMachine));
            }   
            else if (_inputReader.crouch)
            {
                _crouch.crouching = false;
                stateMachine.SetState(new NormalState(stateMachine));
            }
        }
    }
}
