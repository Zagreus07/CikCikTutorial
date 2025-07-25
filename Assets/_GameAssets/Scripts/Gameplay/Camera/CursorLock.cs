using UnityEngine;

public class FreeLookCamera : MonoBehaviour
{
void Start()
{
    Cursor.lockState = CursorLockMode.Locked;  // Mouse'u ekranın ortasına kilitle
    Cursor.visible = false;                    // Mouse imlecini gizle
}
}

