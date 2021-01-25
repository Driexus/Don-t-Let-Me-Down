using UnityEngine;
using UnityEngine.UI;
public class Movement : MonoBehaviour
{
    public Player player;

    public enum Direction { Forward, Backward, Right, Left };
    public Direction direction;


    public void MoveToDirection()
    {
        Vector3Int targetTile = player.GridPosition;

        if (direction == Direction.Forward)
            targetTile += new Vector3Int(0, 1, 0);
        else if (direction == Direction.Backward)
            targetTile += new Vector3Int(0, -1, 0);
        else if (direction == Direction.Right)
            targetTile += new Vector3Int(1, 0, 0);
        else if (direction == Direction.Left)
            targetTile += new Vector3Int(-1, 0, 0);

        player.MoveToTile(targetTile);
    }
}
