using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSimple;

public class AirState : State
{
    private InputReader _inputReader;
    private PlayerStats _stats;
    private Motor _motor;
    private Grounded _grounded;
    private EdgeDetect _edgeDetect;
    private Climb _climb;
    private Crouch _crouch;

    private bool toggle;

    public AirState(StateMachine stateMachine) : base(stateMachine)
    {}

    public override void OnStateEnter()
    {
        _inputReader = stateMachine.GetComponent<InputReader>();
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _motor = stateMachine.GetComponent<Motor>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _edgeDetect = stateMachine.GetComponent<EdgeDetect>();
        _edgeDetect.enabled = true;
        _climb = stateMachine.GetComponent<Climb>();
        _crouch = stateMachine.GetComponent<Crouch>();

        #region change motor vals
        _motor.speed = _stats.airSpeed;
        _motor.accelRate = _stats.airAccelRate;
        _motor.sprintHorizontalInputReductionMult = 1f;
        #endregion

        toggle = false;
    }

    public override void OnStateExit()
    {
        _edgeDetect.enabled = false;
    }

    public override void Tick()
    {
        // have hit the ground? get outta here
        if (_grounded.isGrounded)
        {
            // were we sprinting prior? back to sprint,
            // otherwise back to normal
            if (_inputReader.wasSprinting)
                stateMachine.SetState(new SprintState(stateMachine));
            else
                stateMachine.SetState(new NormalState(stateMachine));
        }

        if (_inputReader.moveVertical > 0)
        {
            if (_edgeDetect.canClimbAndStand && !toggle)
            {
                toggle = true;
                _climb.enabled = true;
            }
            /* else if (_edgeDetect.canClimbAndCrouch && !toggle)
            {
                toggle = true;
                _crouch.speedOverride = 1f;
                _crouch.toHeight = _stats.crouchHeight;
                _crouch.enabled = true;
                _climb.enabled = true;
            } */
        }
    }
}
