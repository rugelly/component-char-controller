using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats/Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] 
    private float _horizSensitivity; // mouse look sensitivity
    public float horizSensitivity 
    {get{return _horizSensitivity;}}

    [SerializeField] 
    private float _vertSensitivity; // mouse look sensitivity
    public float vertSensitivity 
    {get{return _vertSensitivity;}}

    [SerializeField] 
    private float _vertClampMax; // mouse look vertical clamp range max
    public float vertClampMax 
    {get{return _vertClampMax;}}

    [SerializeField] 
    private float _vertClampMin; // mouse look vertical clamp range min
    public float vertClampMin 
    {get{return _vertClampMin;}}

    [SerializeField] 
    private float _standHeight; // how tall when standing straight
    public float standHeight 
    {get{return _standHeight;}}

    [SerializeField] 
    private float _crouchHeight; // how tall when fully crouched
    public float crouchHeight 
    {get{return _crouchHeight;}}

    [SerializeField] 
    private float _crouchTime; // how long to complete a crouch
    public float crouchTime 
    {get{return _crouchTime;}}

    [SerializeField] 
    private float _airSpeed;
    public float airSpeed
    {get{return _airSpeed;}}

    [SerializeField]
    private float _airAccelRate;
    public float airAccelRate
    {get{return _airAccelRate;}}
    
    [SerializeField] 
    private float _runSpeed;
    public float runSpeed 
    {get{return _runSpeed;}}

    [SerializeField] 
    private float _runAccelRate;
    public float runAccelRate 
    {get{return _runAccelRate;}}

    [SerializeField] 
    private float _sprintSpeed;
    public float sprintSpeed 
    {get{return _sprintSpeed;}}

    [SerializeField] 
    private float _sprintAccelRate;
    public float sprintAccelRate 
    {get{return _sprintAccelRate;}}

    [SerializeField] 
    private float _sprintHorizontalInputReduction;
    public float sprintHorizontalInputReduction 
    {get{return _sprintHorizontalInputReduction;}}

    [SerializeField] 
    private float _crouchSpeed;
    public float crouchSpeed 
    {get{return _crouchSpeed;}}

    [SerializeField] 
    private float _crouchAccelRate;
    public float crouchAccelRate 
    {get{return _crouchAccelRate;}}

    [SerializeField] 
    private Vector3 _jumpDirection;
    public Vector3 jumpDirection 
    {get{return _jumpDirection;}}

    [SerializeField] 
    private float _jumpStrength;
    public float jumpStrength 
    {get{return _jumpStrength;}}

    [SerializeField] 
    private Vector2 _ledgeReach;
    public Vector2 ledgeReach 
    {get{return _ledgeReach;}}

    [SerializeField] 
    private float _slideLength; // how long slide component stays active, preventing state transitions
    public float slideLength 
    {get{return _slideLength;}}

    [SerializeField] 
    private float _slideStrength; // amount of force slide applies
    public float slideStrength 
    {get{return _slideStrength;}}

    public float slopeSnapSpeed
    {get{return _sprintSpeed * 1.5f;}}

    [SerializeField]
    private float _climbTime;
    public float climbTime
    {get{return _climbTime;}}

    [SerializeField]
    private float _climbForce;
    public float climbForce
    {get{return _climbForce;}}

    [Header("AIR_JUMP_GLOBAL")]
    [SerializeField]private float _maxFuel; public float maxFuel{get{return _maxFuel;}}

    [Header(">_HOVER")]
    [SerializeField]private float _hoverStrength; public float hoverStrength{get{return _hoverStrength;}}
    [SerializeField]private float _hoverInstantCost; public float hoverInstantCost{get{return _hoverInstantCost;}}
    [SerializeField]private float _hoverConstantCost; public float hoverConstantCost{get{return _hoverConstantCost;}}

    [Header(">_ROCKET")]
    [SerializeField]private float _rocketStrength; public float rocketStrength{get{return _rocketStrength;}}
    [SerializeField]private float _rocketInstantCost; public float rocketInstantCost{get{return _rocketInstantCost;}}
    [SerializeField]private float _rocketConstantCost; public float rocketConstantCost{get{return _rocketConstantCost;}}

    [Header(">_BOOST")]
    [SerializeField]private float _boostStrength; public float boostStrength{get{return _boostStrength;}}
    [SerializeField]private float _boostInstantCost; public float boostInstantCost{get{return _boostInstantCost;}}
    // [SerializeField]private float _boostConstantCost; public float boostConstantCost{get{return _boostConstantCost;}}

}
