using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats/Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private float _horizSensitivity; // mouse look sensitivity
    public float horizSensitivity {get => _horizSensitivity;}

    [SerializeField] private float _vertSensitivity; // mouse look sensitivity
    public float vertSensitivity {get => _vertSensitivity;}

    [SerializeField] private float _vertClampMax; // mouse look vertical clamp range max
    public float vertClampMax {get => _vertClampMax;}

    [SerializeField] private float _vertClampMin; // mouse look vertical clamp range min
    public float vertClampMin {get => _vertClampMin;}

    [SerializeField] private float _standHeight; // how tall when standing straight
    public float standHeight {get => _standHeight;}

    [SerializeField] private float _crouchHeight; // how tall when fully crouched
    public float crouchHeight {get => _crouchHeight;}

    [SerializeField] private float _crouchTime; // how long to complete a crouch
    public float crouchTime {get => _crouchTime;}

    [SerializeField] private AnimationCurve _crouchTransitionCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
    public AnimationCurve crouchTransitionCurve {get => _crouchTransitionCurve;}

    [SerializeField] private float _airSpeed;
    public float airSpeed {get => _airSpeed;}

    [SerializeField] private float _maxAirVelocityChange;
    public float maxAirVelocityChange {get => _maxAirVelocityChange;}
    
    [SerializeField] private float _runSpeed;
    public float runSpeed {get => _runSpeed;}

    [SerializeField] private float _maxRunVelocityChange;
    public float maxRunVelocityChange {get => _maxRunVelocityChange;}

    [SerializeField] private float _sprintSpeed;
    public float sprintSpeed {get => _sprintSpeed;}

    [SerializeField] private float _maxSprintVelocityChange;
    public float maxSprintVelocityChange {get => _maxSprintVelocityChange;}

    [SerializeField] private float _sprintHorizontalInputReduction;
    public float sprintHorizontalInputReduction {get => _sprintHorizontalInputReduction;}

    [SerializeField] private AnimationCurve _sprintTransitionCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
    public AnimationCurve sprintTransitionCurve {get => _sprintTransitionCurve;}

    [SerializeField] private float _crouchSpeed;
    public float crouchSpeed {get => _crouchSpeed;}

    [SerializeField] private float _maxCrouchVelocityChange;
    public float maxCrouchVelocityChange {get => _maxCrouchVelocityChange;}

    [SerializeField] private Vector3 _jumpDirection; // basic standard human "jump"
    public Vector3 jumpDirection {get => _jumpDirection;}

    [SerializeField] private float _jumpStrength;
    public float jumpStrength {get => _jumpStrength;}

    [SerializeField] private Vector2 _ledgeReach;
    public Vector2 ledgeReach {get => _ledgeReach;}

    [SerializeField] private float _slideLength; // how long slide component stays active, preventing state transitions
    public float slideLength {get => _slideLength;}

    [SerializeField] private float _slideStrength; // amount of force slide applies
    public float slideStrength {get => _slideStrength;}
}
