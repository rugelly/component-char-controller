using UnityEngine;
using Luminosity.IO;

public class SaveOnButtonPress : MonoBehaviour
{
    private void Awake()
    {
        InputManager.Load("Assets/input_config.xml");
    }

    public void Save()
    {
        InputManager.Save("Assets/input_config.xml");
    }
}
