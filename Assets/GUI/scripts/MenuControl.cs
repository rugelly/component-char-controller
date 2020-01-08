using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    private bool off;
    private bool offf;
    private bool controls;
    private bool config;
    public GameObject pauseMenu, ingameGUI, controlsMenu, keyboardControls, gamepadControls, configMenu;
    public Toggle keyboard, gamepad;
    public Dropdown mh, mv, lh, lv;
    private ChangeInputAxes axisconfig;

    private void Awake()
    {
        axisconfig = GetComponent<ChangeInputAxes>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;

        off = false;
        offf = true;

        MatchDropdownsToConfig();
    }

    public void ExitMenu()
    {
        off = true;
        offf = false;
        controls = false;
        config = false;

        Time.timeScale = 1;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EnterMenu()
    {
        off = false;
        offf = true;
        controls = false;
        config = false;

        Time.timeScale = 0;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ControlsMenu()
    {
        controls = true;
        config = false;
    }

    public void ConfigMenu()
    {
        config = true;
        controls = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (off)
                EnterMenu();
            else
                ExitMenu();
        }

        ingameGUI.SetActive(off);
        pauseMenu.SetActive(offf);
        controlsMenu.SetActive(controls);
        configMenu.SetActive(config);

        keyboardControls.SetActive(keyboard.isOn);
        gamepadControls.SetActive(gamepad.isOn);

        MatchDropdownsToConfig();
    }

    private void MatchDropdownsToConfig()
    {
        mh.value = axisconfig.movhor;
        mv.value = axisconfig.movver;
        lh.value = axisconfig.lookhor;
        lv.value = axisconfig.lookver;

        // GameObject player = GameObject.FindGameObjectWithTag("Player");
        // InputReader inputreader = player.GetComponent<InputReader>();
        // inputreader.mh = mh.value;
        // inputreader.mv = mv.value;
        // inputreader.lh = lh.value;
        // inputreader.lv = lv.value;
    }
}
