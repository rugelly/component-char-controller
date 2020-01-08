using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSimple;

public class SprintState : State
{
    
    public SprintState(StateMachine stateMachine) : base(stateMachine)
    {}

    #region components to enable
    private Motor _motor;
    private PlayerStats _stats;
    private Grounded _grounded;
    private Jump _jump;
    private Crouch _crouch;
    private InputReader _inputReader;
    private FallDamage _falldmg;
    #endregion

    #region components to disable

    #endregion

    public override void OnStateEnter()
    {
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _motor = stateMachine.GetComponent<Motor>();
        _inputReader = stateMachine.GetComponent<InputReader>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _jump = stateMachine.GetComponent<Jump>();
        _crouch = stateMachine.GetComponent<Crouch>();
        _falldmg = stateMachine.GetComponent<FallDamage>();

        #region change motor vals
        _motor.speed = _stats.sprintSpeed;
        _motor.accelRate = _stats.sprintAccelRate;
        _motor.sprintHorizontalInputReductionMult = _stats.sprintHorizontalInputReduction;
        #endregion
    }

    public override void OnStateExit()
    {
        
    }

    public override void Tick()
    {
        if (_falldmg.landingFirm || _falldmg.landingHard || _falldmg.landingSplat)
            stateMachine.SetState(new HardLandingState(stateMachine));
        
        // not grounded
        // turn off ability to jump
        // change to air state
        if (!_grounded.isGrounded)
        {
            _inputReader.wasSprinting = true;
            stateMachine.SetState(new AirState(stateMachine));
        }

        // not holding down forward
        if (_inputReader.moveVertical <= 0)
        {
            stateMachine.SetState(new NormalState(stateMachine));
        }

        // pressed crouch while sprinting
        if (_inputReader.crouch)
        {
            _crouch.crouching = true;
            stateMachine.SetState(new SlideState(stateMachine));
        }
    }
}
