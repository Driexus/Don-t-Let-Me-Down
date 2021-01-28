using UnityEngine;
public class Movement : MonoBehaviour
{
    public Player player;

    public enum Direction { Forward, Backward, Right, Left };
    public Direction direction;


    public void MoveToDirection()
    {
        Vector3Int playerDirection = new Vector3Int() ;

        if (direction == Direction.Forward)
            playerDirection = Vector3Int.up;
        else if (direction == Direction.Backward)
            playerDirection = Vector3Int.down;
        else if (direction == Direction.Right)
            playerDirection = Vector3Int.right;
        else if (direction == Direction.Left)
            playerDirection = Vector3Int.left;

        player.MoveToDirection(playerDirection);
    }
}
