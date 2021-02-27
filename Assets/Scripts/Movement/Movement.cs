using System.Collections;
using UnityEngine;
public class Movement : MonoBehaviour
{
    public Player player;
    public GameManager GM;
    private bool movedRecently;

    public enum Direction { Forward, Backward, Right, Left };
    public Direction direction;

    // Set movedRecently to true for x number of frames after a movement has been registered to avoid multiple commands
    private IEnumerator JustMoved(int frames)
    {
        movedRecently = true;
        for (int i = 0; i < frames; i++)
            yield return new WaitForEndOfFrame();
        movedRecently = false;
    }

    public void MoveToDirection()
    {
        // If player is moving or has moved recently do not enter new commands
        if (player.IsMoving || movedRecently)
            return;
        
        // Sets movedRecently to true to avoid multiple commands
        StartCoroutine(JustMoved(20));

        Vector3Int playerDirection = new Vector3Int() ;

        if (direction == Direction.Forward)
            playerDirection = Vector3Int.up;
        else if (direction == Direction.Backward)
            playerDirection = Vector3Int.down;
        else if (direction == Direction.Right)
            playerDirection = Vector3Int.right;
        else if (direction == Direction.Left)
            playerDirection = Vector3Int.left;


        Vector3Int targetCoords = player.GridPosition(GM.map.ActiveTilemap) + playerDirection;
        if (GM.map.ActiveTilemap.HasTile(targetCoords))
        {
            player.Walk(playerDirection);
        }
        else
        {
            GM.ResetTimer();
            player.Jump(playerDirection);
            GM.map.NextTilemap();
        }
    }
}
