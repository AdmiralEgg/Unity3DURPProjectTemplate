using UnityEngine;

public class SpinCube : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.Rotate(
            4 * Time.deltaTime,
            5 * Time.deltaTime,
            6 * Time.deltaTime
        );
    }
}
