using UnityEngine;

public class DeleteArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

        // Check the tag of the colliding object
        switch (collision.gameObject.tag)
        {
            case "Ant":
                Destroy(collision.gameObject);
                Debug.Log($"{collision.gameObject.tag} was destroyed.");
                break;
            case "LadyBug":
                Destroy(collision.gameObject);
                Debug.Log($"{collision.gameObject.tag} was destroyed.");
                break;

            default:
                Debug.Log($"{collision.gameObject.tag} entered the DeleteArea but was not destroyed.");
                break;
        }
    }
}
