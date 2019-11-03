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
        StatHolder temp = stateMachine.GetComponent<StatHolder>();
        _stats = temp.held;
        _inputReader = stateMachine.GetComponent<InputReader>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _jump = stateMachine.GetComponent<Jump>();
        #endregion

        #region turn on crouch
        _crouch = stateMachine.GetComponent<Crouch>();
        _crouch.isCrouching = true; // we want to go from standing, to crouch
        _crouch.transitionCurve = _stats.crouchTransitionCurve;
        _crouch.enabled = true;

        crouching = true; // true until the component has finished its work and disabled itself
        #endregion

        #region change motor vals
        _motor.speed = _stats.crouchSpeed;
        _motor.maxVelocityChange = _stats.maxCrouchVeloctiyChange;
        #endregion
    }

    public override void OnStateExit()
    {
        #region cleanup
        _crouch.enabled = true; // turn on crouch as we leave to get back to normal standing position
        _crouch.isCrouching = false; // tell component to stand us back up
        #endregion
    }

    public override void Tick()
    {
        #region jump & air goto
        // not grounded
        // turn off ability to jump
        // change to air state
        if (!_grounded.check)
        {
            _jump.enabled = false;

            stateMachine.SetState(new AirState(stateMachine));
        }
        else // ability to jump is enabled (note this SHOULD grounded = false and exit state)
        {
            _jump.enabled = true;
        }
        #endregion jump & air goto

        #region actual crouch business
        // so crouch state was entered
        // component enabled to start off
        // standing -> crouch, crouching == true
        // component disabled, crouching == false
        // IN CROUCH STATE, PHYSICALLY CROUCHED
        // exiting the state enables the component to set player back to standing
        // crouch -> standing

        // true while component is active and, presumably, running
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
            stateMachine.SetState(new NormalState(stateMachine));
        }
        #endregion normal goto

        #region sprint goto
        // forward input and pressed sprint and not currently transitioning
        if (!crouching && _inputReader.moveVertical > 0 && _inputReader.sprint)
        {
            stateMachine.SetState(new SprintState(stateMachine));
        }
        #endregion sprint goto
    }
}
