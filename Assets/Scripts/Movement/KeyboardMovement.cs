using UnityEngine;

[RequireComponent(typeof(Movement))]
public class KeyboardMovement : MonoBehaviour
{
    private Movement movement;
    GameManager GM;

    private void Awake()
    {
        movement = gameObject.GetComponent<Movement>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
        else
        {
            movement.StayIdle();
        }

        if (Input.GetKey("space"))
            GM.SkipMemorizationPhase();
    }
}
