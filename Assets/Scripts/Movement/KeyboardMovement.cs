using UnityEngine;

[RequireComponent(typeof(Movement))]
public class KeyboardMovement : MonoBehaviour
{
    private Movement movement;

    private void Awake()
    {
        movement = gameObject.GetComponent<Movement>();
    }

    void Update()
    {
        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            movement.direction = Movement.Direction.Forward;
            movement.MoveToDirection();
        }
        else if (Input.GetKey("s") || Input.GetKey("down"))
        {
            movement.direction = Movement.Direction.Backward;
            movement.MoveToDirection();
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            movement.direction = Movement.Direction.Left;
            movement.MoveToDirection();

        }
        else if (Input.GetKey("d") || Input.GetKey("right"))
        {
            movement.direction = Movement.Direction.Right;
            movement.MoveToDirection();
        }
    }
}
