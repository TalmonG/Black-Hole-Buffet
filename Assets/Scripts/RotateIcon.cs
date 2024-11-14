using UnityEngine;

public class RotateIcon : MonoBehaviour
{

    //private GameObject consumeHistoryIcon;
    private float rotationSpeed = 30f; // Degrees per second

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GameObject consumeHistoryIcon = GameObject.FindGameObjectWithTag("ConsumeHistoryIcon");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
