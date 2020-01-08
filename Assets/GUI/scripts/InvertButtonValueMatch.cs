using UnityEngine;
using UnityEngine.UI;

public class InvertButtonValueMatch : MonoBehaviour
{
    private ChangeInputAxes axisconfig;
    private Text text;
    public enum Axis {mouseHorizontal, mouseVertical, controllerHorizontal, controllerVertical}
    public Axis axis = Axis.mouseHorizontal;

    private void Awake()
    {
        GameObject gui = GameObject.FindGameObjectWithTag("GUI");
        axisconfig = gui.GetComponent<ChangeInputAxes>();

        text = GetComponentInChildren<Text>();
    }

    private string yesText = "On", noText = "Off";
    private void Update()
    {
        if (axis == Axis.mouseHorizontal)
            text.text = axisconfig._ilkbh ? yesText : noText;
        else if (axis == Axis.mouseVertical)
            text.text = axisconfig._ilkbv ? yesText : noText;
        else if (axis == Axis.controllerHorizontal)
            text.text = axisconfig._ilch ? yesText : noText;
        else if (axis == Axis.controllerVertical)
            text.text = axisconfig._ilcv ? yesText : noText;
        
    }
}
