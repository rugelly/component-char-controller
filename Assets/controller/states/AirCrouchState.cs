using UnityEngine;
using StateMachineSimple;

public class AirCrouchState : State
{
    private InputReader _input;
    private Crouch _crouch;
    private Grounded _grounded;
    private EdgeDetect _edgeDetect;

    public AirCrouchState(StateMachine stateMachine) : base(stateMachine)
    {}

    public override void OnStateEnter()
    {
        _input = stateMachine.GetComponent<InputReader>();
        _crouch = stateMachine.GetComponent<Crouch>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _edgeDetect = stateMachine.GetComponent<EdgeDetect>();

        _edgeDetect.enabled = false;
    }

    public override void OnStateExit()
    {

    }

    public override void Tick()
    {
        if (_grounded.isGrounded)
        {
            stateMachine.SetState(new CrouchState(stateMachine));
        }

        if (_crouch.hasHeadroom)
        {
            if (_input.jump || _input.crouch || !Physics.Raycast(stateMachine.transform.position, Vector3.down, 0.2f))
            {
                _crouch.crouching = false;
                _edgeDetect.enabled = true;
                stateMachine.SetState(new AirState(stateMachine));
            }
        }
    }
}
