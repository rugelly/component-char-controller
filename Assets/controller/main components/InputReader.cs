using UnityEngine;
using System.IO;
using Luminosity.IO;

public class InputReader : MonoBehaviour
{
    private int mh, mv, lh, lv, imh, imv, ich, icv;
    private ChangeInputAxes whichAxisToRead;
    private void Awake()
    {
        GameObject gui = GameObject.FindGameObjectWithTag("GUI");
        whichAxisToRead = gui.GetComponent<ChangeInputAxes>();
    }
    private PlayerStats _stats;
    private void OnEnable()
    {
        _stats = GetComponent<StatHolder>().held;
    }
    private void Update()
    {
        mh = whichAxisToRead.movhor;
        mv = whichAxisToRead.movver;
        lh = whichAxisToRead.lookhor;
        lv = whichAxisToRead.lookver;
        imh = whichAxisToRead._ilkbh == true ? -1 : 1;
        imv = whichAxisToRead._ilkbv == true ? -1 : 1;
        ich = whichAxisToRead._ilch == true ?  -1 : 1;
        icv = whichAxisToRead._ilcv == true ?  -1 : 1;
    }

    public float moveHorizontal
    {get{
        float kb = Input.GetAxisRaw("Horizontal");
        if (kb != 0)
            return kb;
        else if (mh == 0)
            return Input.GetAxisRaw("LeftStickHorizontal");
        else if (mh == 1)
            return Input.GetAxisRaw("RightStickHorizontal");
        else return 0;        
    }}

    public float moveVertical
    {get{
        float kb = Input.GetAxisRaw("Vertical");
        if (kb != 0)
            return kb;
        else if (mh == 0)
            return Input.GetAxisRaw("LeftStickVertical");
        else if (mh == 1)
            return Input.GetAxisRaw("RightStickVertical");
        else return 0;        
    }}

    public float lookHorizontal
    {get{
        float kb = Input.GetAxisRaw("LookHorizontal") * _stats.horizSensitivity * imh;
        if (kb != 0)
            return kb;
        else if (mh == 0)
            return Input.GetAxisRaw("RightStickHorizontal") * _stats.horizSensitivity * ich;
        else if (mh == 1)
            return Input.GetAxisRaw("LeftStickHorizontal") * _stats.horizSensitivity * ich;
        else return 0;        
    }}

    public float lookVertical
    {get{
        float kb = Input.GetAxisRaw("LookVertical") * _stats.vertSensitivity * imv;
        if (kb != 0)
            return kb;
        else if (mh == 0)
            return Input.GetAxisRaw("RightStickVertical") * _stats.vertSensitivity * icv;
        else if (mh == 1)
            return Input.GetAxisRaw("LeftStickVertical") * _stats.vertSensitivity * icv;
        else return 0;
    }}

    public bool jump
    {get{return InputManager.GetButtonDown("Jump");}}
    // {get{return _jump;}}
    // private bool _jump;

    public bool sprint
    {get{return InputManager.GetButtonDown("Sprint");}}
    // {get{return _sprint;}}
    // private bool _sprint;

    public bool crouch
    {get{return InputManager.GetButtonDown("Crouch");}}
    // {get{return _crouch;}}
    // private bool _crouch;

    public bool hasMoveInput
    {get
        {
            if (moveHorizontal > 0 || moveHorizontal < 0)
                return true;
            else if (moveVertical > 0 || moveVertical < 0)
                return true;
            else
                return false;
        }
    }

    [HideInInspector] public bool wasSprinting;

    // new input system stuff i stopped using cause its broke###OR IM DUMB AND SALTY###
    // private PlayerInput _playerInput;

    // private void OnMove(InputValue value)
    // {
    //     _moveHorizontal = value.Get<Vector2>().x;
    //     _moveVertical = value.Get<Vector2>().y;
    //     _hasMoveInput = value.Get<Vector2>() != Vector2.zero ? true : false;
    // }

    // private void OnLook(InputValue value)
    // {
    //     _lookHorizontal = value.Get<Vector2>().x;
    //     _lookVertical = value.Get<Vector2>().y;
    // }

    // private void OnEnable()
    // {
    //     _playerInput = GetComponent<PlayerInput>();
    // }

    // private void Update()
    // {
    //     _jump = _playerInput.actions["Jump"].triggered;
    //     _crouch = _playerInput.actions["Crouch"].triggered;
    //     _sprint = _playerInput.actions["Sprint"].triggered;
    // }
}
