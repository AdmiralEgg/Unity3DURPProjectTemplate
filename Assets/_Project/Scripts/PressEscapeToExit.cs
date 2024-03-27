using UnityEngine;

public class PressEscapeToExit : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Key pressed! Quitting!");
            Application.Quit();
        }
    }
}
