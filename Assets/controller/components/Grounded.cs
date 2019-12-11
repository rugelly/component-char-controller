using UnityEngine;

public class Grounded : MonoBehaviour
{
    [SerializeField]
    private bool _check;
    public bool check {get => _check;}

    private void OnTriggerStay(Collider other)
    {
        _check = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _check = false;
    }
}
