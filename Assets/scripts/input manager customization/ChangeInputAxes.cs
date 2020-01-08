using UnityEngine;
using System.IO;

public class ChangeInputAxes : MonoBehaviour
{
    private void Awake()
    {
        ReadEmAndWeep();
        UpdateValues();
    }

    private string mh = "0", mv = "0", lh = "0", lv = "0", ilkbh = "false", ilkbv = "true", ilch = "false", ilcv = "false";
    public int movhor, movver, lookhor, lookver;
    public bool _ilkbh = false, _ilkbv = true, _ilch = false, _ilcv = false;
    // for movement:
    // 0 == left stick
    // 1 == right stick

    // for looking
    // 0 == right stick
    // 1 == left stick

    public void ChangeMoveHorizontal(int dropdown)
    {
        bool tmp = dropdown == 0 ? true : false;
        mh = tmp == true ? "0" : "1";
        WriteToFileIHope();
        ReadEmAndWeep();
        UpdateValues();
    }

    public void ChangeMoveVertical(int dropdown)
    {
        bool tmp = dropdown == 0 ? true : false;
        mv = tmp == true ? "0" : "1";
        WriteToFileIHope();
        ReadEmAndWeep();
        UpdateValues();
    }

    public void ChangeLookHorizontal(int dropdown)
    {
        bool tmp = dropdown == 0 ? true : false;
        lh = tmp == true ? "0" : "1";
        WriteToFileIHope();
        ReadEmAndWeep();
        UpdateValues();
    }

    public void ChangeLookVertical(int dropdown)
    {
        bool tmp = dropdown == 0 ? true : false;
        lv = tmp == true ? "0" : "1";
        WriteToFileIHope();
        ReadEmAndWeep();
        UpdateValues();
    }

    public void InvertMouseHor()
    {
        _ilkbh = !_ilkbh;
        WriteToFileIHope();
        ReadEmAndWeep();
        UpdateValues();
    }
    public void InvertMouseVer()
    {
        _ilkbv = !_ilkbv;
        WriteToFileIHope();
        ReadEmAndWeep();
        UpdateValues();
    }
    public void InvertConHor()
    {
        _ilch = !_ilch;
        WriteToFileIHope();
        ReadEmAndWeep();
        UpdateValues();
    }
    public void InvertConVer()
    {
        _ilcv = !_ilcv;
        WriteToFileIHope();
        ReadEmAndWeep();
        UpdateValues();
    }

    private void WriteToFileIHope()
    {
        //Debug.Log("file should be saving???");
        using (StreamWriter sw = new StreamWriter("Assets/axis_config.txt", false))
        {
            sw.WriteLine(mh);
            sw.WriteLine(mv);
            sw.WriteLine(lh);
            sw.WriteLine(lv);
            sw.WriteLine(_ilkbh.ToString());
            sw.WriteLine(_ilkbv.ToString());
            sw.WriteLine(_ilch.ToString());
            sw.WriteLine(_ilcv.ToString());
            sw.Close();
        }
    }

    private void ReadEmAndWeep()
    {
        //Debug.Log("file reading now");
        using (StreamReader sr = new StreamReader("Assets/axis_config.txt"))
        {
            mh = sr.ReadLine();
            mv = sr.ReadLine();
            lh = sr.ReadLine();
            lv = sr.ReadLine();
            ilkbh = sr.ReadLine();
            ilkbv = sr.ReadLine();
            ilch = sr.ReadLine();
            ilcv = sr.ReadLine();
            sr.Close();
        }
    }

    private void UpdateValues()
    {
        movhor = int.Parse(mh);
        movver = int.Parse(mv);
        lookhor = int.Parse(lh);
        lookver = int.Parse(lv);
        _ilkbh = bool.Parse(ilkbh);
        _ilkbv = bool.Parse(ilkbv);
        _ilch = bool.Parse(ilch);
        _ilcv = bool.Parse(ilcv);
    }
}
