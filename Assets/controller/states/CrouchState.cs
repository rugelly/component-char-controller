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
    private Headroom _headroom;
    private PlayerStats _stats;
    #endregion

    #region components to disable
    
    #endregion

    #region crouching stuff
    private bool crouching; // false when crouch component not enabled
    #endregion

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
        #endregion

        /* #region crouch init try 2
        _crouch.toHeight = _stats.crouchHeight;
        _crouch.enabled = true;
        #endregion crouch init try 2 */

        #region change motor vals
        _motor.speed = _stats.crouchSpeed;
        _motor.maxVelocityChange = _stats.maxCrouchVelocityChange;
        #endregion
    }

    public override void OnStateExit()
    {
        _crouch.toHeight = _stats.standHeight;
        _crouch.enabled = true;
    }

    public override void Tick()
    {
        #region component active
        // true while component is active, false when not
        if (_crouch.enabled)
            crouching = true;
        else
            crouching = false;
        
        #endregion component active

        #region jump & air goto
        // not grounded
        // turn off ability to jump
        // change to air state
        if (!_grounded.check)
        {
            stateMachine.SetState(new AirState(stateMachine));
        }
        else if(!crouching && _headroom.check && _inputReader.jump)
        {
            _jump.enabled = true;
        }
        #endregion jump & air goto

        #region normal goto
        // not in the middle of performing a crouch (either up or down), and crouch is pressed
        if (!crouching && _headroom.check && _inputReader.crouch)
        {
            stateMachine.SetState(new NormalState(stateMachine));
        }
        #endregion normal goto

        #region sprint goto
        // forward input and pressed sprint and not currently transitioning
        if (!crouching && _headroom.check && _inputReader.moveVertical > 0 && _inputReader.sprint)
        {
            stateMachine.SetState(new SprintState(stateMachine));
        }
        #endregion sprint goto
    }
}
