using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats/Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] 
    private float _horizSensitivity; // mouse look sensitivity
    public float horizSensitivity 
    {get => _horizSensitivity;}

    [SerializeField] 
    private float _vertSensitivity; // mouse look sensitivity
    public float vertSensitivity 
    {get => _vertSensitivity;}

    [SerializeField] 
    private float _vertClampMax; // mouse look vertical clamp range max
    public float vertClampMax 
    {get => _vertClampMax;}

    [SerializeField] 
    private float _vertClampMin; // mouse look vertical clamp range min
    public float vertClampMin 
    {get => _vertClampMin;}

    [SerializeField] 
    private float _standHeight; // how tall when standing straight
    public float standHeight 
    {get => _standHeight;}

    [SerializeField] 
    private float _crouchHeight; // how tall when fully crouched
    public float crouchHeight 
    {get => _crouchHeight;}

    [SerializeField] 
    private float _crouchTime; // how long to complete a crouch
    public float crouchTime 
    {get => _crouchTime;}

    [SerializeField] 
    private float _airSpeed;
    public float airSpeed
    {get => _airSpeed;}

    [SerializeField]
    private float _airAccelRate;
    public float airAccelRate
    {get => _airAccelRate;}
    
    [SerializeField] 
    private float _runSpeed;
    public float runSpeed 
    {get => _runSpeed;}

    [SerializeField] 
    private float _runAccelRate;
    public float runAccelRate 
    {get => _runAccelRate;}

    [SerializeField] 
    private float _sprintSpeed;
    public float sprintSpeed 
    {get => _sprintSpeed;}

    [SerializeField] 
    private float _sprintAccelRate;
    public float sprintAccelRate 
    {get => _sprintAccelRate;}

    [SerializeField] 
    private float _sprintHorizontalInputReduction;
    public float sprintHorizontalInputReduction 
    {get => _sprintHorizontalInputReduction;}

    [SerializeField] 
    private float _crouchSpeed;
    public float crouchSpeed 
    {get => _crouchSpeed;}

    [SerializeField] 
    private float _crouchAccelRate;
    public float crouchAccelRate 
    {get => _crouchAccelRate;}

    [SerializeField] 
    private Vector3 _jumpDirection;
    public Vector3 jumpDirection 
    {get => _jumpDirection;}

    [SerializeField] 
    private float _jumpStrength;
    public float jumpStrength 
    {get => _jumpStrength;}

    [SerializeField] 
    private Vector2 _ledgeReach;
    public Vector2 ledgeReach 
    {get => _ledgeReach;}

    [SerializeField] 
    private float _slideLength; // how long slide component stays active, preventing state transitions
    public float slideLength 
    {get => _slideLength;}

    [SerializeField] 
    private float _slideStrength; // amount of force slide applies
    public float slideStrength 
    {get => _slideStrength;}

    public float slopeSnapSpeed
    {get {return _sprintSpeed * 1.5f;}}

    [SerializeField]
    private float _climbTime;
    public float climbTime
    {get => _climbTime;}

    [SerializeField]
    private float _climbForce;
    public float climbForce
    {get => _climbForce;}
}
