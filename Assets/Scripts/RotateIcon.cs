using UnityEngine;

public class RotateIcon : MonoBehaviour
{

    private float rotationSpeed = 30f; 

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
