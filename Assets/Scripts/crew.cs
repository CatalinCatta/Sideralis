using UnityEngine;

public class crew : MonoBehaviour
{
    public float speed = 1; // adjust this in the inspector to control the crew's movement speed
    private bool isMovingRight = true; // set the initial direction of the crew

    private void Update()
    {
        // Move the crew horizontally
        if (isMovingRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(-Vector2.right * speed * Time.deltaTime);

        // Check if the crew has hit a wall
        var hit = Physics2D.Raycast(transform.position, isMovingRight ? Vector2.right : -Vector2.right, 0.2f);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Wall"))
        {
            // If a wall is hit, change direction
            Debug.Log("VAR");
            isMovingRight = !isMovingRight;
        }
    }
}