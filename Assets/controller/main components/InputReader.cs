using UnityEngine;

public class InputReader : MonoBehaviour
{
    #region boilerplate
    private PlayerStats _stats;

    private void OnEnable()
    {
        StatHolder temp = GetComponent<StatHolder>();
        _stats = temp.held;
    }
    #endregion
    
    public float moveHorizontal
    {get{return Input.GetAxis("Horizontal");}}

    public float moveVertical
    {get{return Input.GetAxis("Vertical");}}

    public float lookHorizontal
    {get{return Input.GetAxis("Mouse X") * _stats.horizSensitivity;}}

    public float lookVertical
    {get{return Input.GetAxis("Mouse Y") * _stats.vertSensitivity;}}

    public bool jump
    {get{return Input.GetButtonDown("Jump");}}

    public bool sprint
    {get{return Input.GetButtonDown("Sprint");}}

    public bool crouch
    {get{return Input.GetButtonDown("Crouch");}}

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

    // TODO: can slide+crouch use this to continue a sprint after sliding?
    // do i want them to?
    public bool wasSprinting;
}
