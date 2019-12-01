using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSimple;

public class CrouchState : State
{
    
    public CrouchState(StateMachine stateMachine) : base(stateMachine)
    {}

    #region components to enable
    private Motor _motor;
    private Grounded _grounded;
    private Jump _jump;
    private InputReader _inputReader;
    private Crouch _crouch;
    private PlayerStats _stats;
    #endregion

    #region components to disable
    
    #endregion

    #region crouching stuff
    private bool crouching; // only turns false when not in the act of performing any kind of crouch transition
    #endregion

    public override void OnStateEnter()
    {
        #region get comps
        _motor = stateMachine.GetComponent<Motor>();
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _inputReader = stateMachine.GetComponent<InputReader>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _jump = stateMachine.GetComponent<Jump>();
        #endregion

        /* #region turn on crouch
        _crouch = stateMachine.GetComponent<Crouch>();
        _crouch.isCrouching = true; // we want to go from standing, to crouch
        _crouch.transitionCurve = _stats.crouchTransitionCurve;
        _crouch.enabled = true;

        crouching = true; // true until the component has finished its work and disabled itself
        #endregion */

        #region crouch init try 2
        _crouch = stateMachine.GetComponent<Crouch>();
        _crouch.toHeight = _stats.crouchHeight;
        _crouch.transitionCurve = _stats.crouchTransitionCurve;
        _crouch.enabled = true;
        #endregion crouch init try 2

        #region change motor vals
        _motor.speed = _stats.crouchSpeed;
        _motor.maxVelocityChange = _stats.maxCrouchVeloctiyChange;
        #endregion
    }

    public override void OnStateExit()
    {
        // un crouch is hackily put in for transition to sprint and normal states
        // TODO: can "un crouching be handled in a better way?"
    }

    public override void Tick()
    {
        #region jump & air goto
        // not grounded
        // turn off ability to jump
        // change to air state
        if (!_grounded.check)
        {
            stateMachine.SetState(new AirState(stateMachine));
        }
        else if(!crouching && _inputReader.jump)
        {
            _jump.enabled = true;
            _crouch.toHeight = _stats.standHeight;
            _crouch.transitionCurve = _stats.crouchTransitionCurve;
            _crouch.enabled = true;
        }
        #endregion jump & air goto

        #region actual crouch business

        // true while component is active
        // false when its done and gone turned itself off
        if (_crouch.enabled)
        {
            crouching = true;
        }
        else
        {
            crouching = false;
        }
        #endregion actual crouch business

        #region normal goto
        // not in the middle of performing a crouch (either up or down), and crouch is pressed
        if (!crouching && _inputReader.crouch)
        {
            _crouch.toHeight = _stats.standHeight;
            _crouch.transitionCurve = _stats.crouchTransitionCurve;
            _crouch.enabled = true;
            stateMachine.SetState(new NormalState(stateMachine));
        }
        #endregion normal goto

        #region sprint goto
        // forward input and pressed sprint and not currently transitioning
        if (!crouching && _inputReader.moveVertical > 0 && _inputReader.sprint)
        {
            _crouch.toHeight = _stats.standHeight;
            _crouch.transitionCurve = _stats.crouchTransitionCurve;
            _crouch.enabled = true;
            stateMachine.SetState(new SprintState(stateMachine));
        }
        #endregion sprint goto
    }
}
