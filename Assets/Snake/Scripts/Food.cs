using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name=="SnakeHead")
        {
            //collision.GetComponent<SnakeController>().Grow();
            collision.GetComponentInParent<SnakeController>().Grow();
            Destroy(gameObject);
        }
    }
}
