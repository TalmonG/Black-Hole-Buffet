using UnityEngine;

public class DeleteArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {

        // Check tag
        switch (collision.gameObject.tag)
        {
            case "Ant":
                Destroy(collision.gameObject);
                break;
            case "LadyBug":
                Destroy(collision.gameObject);
                break;

            default:
                break;
        }
    }
}
