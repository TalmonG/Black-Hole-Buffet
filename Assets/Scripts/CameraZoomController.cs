using UnityEngine;

public class CameraZoomController : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject CineCam = GameObject.FindGameObjectWithTag("CineCam");
        GameObject Player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (Player.playerSizeCounter > 30)
        {
            Debug.Log("Zoom Out");
        }
    }
}
