using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SerializeStats : MonoBehaviour
{
    public Object statobj;
    public PlayerStats actualStatRef;
    public Dropdown jumptypeDropdown;
    public Slider vertSensi, horizSensi;
    private string statsToString;
    private string stringToStats;

    private void Awake()
    {
        Write();
        ReadAndOverwrite();

        MatchDropdownValue();
        MatchHorizSensiValue();
        MatchVertSensiValue();
    }

    private string fileLocation = "Assets/player_stats.json";
    private void ReadAndOverwrite()
    {
        using (StreamReader sr = new StreamReader(fileLocation))
        {
            JsonUtility.FromJsonOverwrite(sr.ReadToEnd(), statobj);
        }
    }

    private void Write()
    {
        statsToString = JsonUtility.ToJson(statobj, true);
        using (StreamWriter sw = new StreamWriter(fileLocation))
        {
             sw.Write(statsToString);
        }
    }

    private void OnValueChange()
    {
        Write();
        ReadAndOverwrite();
    }

    public void ChangeSensitivityHorizontal()
    {
        actualStatRef.horizSensitivity = horizSensi.value;
        OnValueChange();
    }

    public void ChangeSensitivityVertical()
    {
        actualStatRef.vertSensitivity = vertSensi.value;
        OnValueChange();
    }

    public void ChangeJumpType()
    {
        int value = jumptypeDropdown.value;
        if (value == 0)
            actualStatRef.jumpType = PlayerStats.JumpType.hover;
        else if (value == 1)
            actualStatRef.jumpType = PlayerStats.JumpType.rocket;
        else
            actualStatRef.jumpType = PlayerStats.JumpType.boost;

        OnValueChange();
    }

    private void MatchDropdownValue()
    {
        if (actualStatRef.jumpType == PlayerStats.JumpType.hover)
            jumptypeDropdown.value = 0;
        else if (actualStatRef.jumpType == PlayerStats.JumpType.rocket)
            jumptypeDropdown.value = 1;
        else
            jumptypeDropdown.value = 2;
    }

    private void MatchVertSensiValue()
    {
        vertSensi.value = actualStatRef.vertSensitivity;
    }
    private void MatchHorizSensiValue()
    {
        horizSensi.value = actualStatRef.horizSensitivity;
    }
}
