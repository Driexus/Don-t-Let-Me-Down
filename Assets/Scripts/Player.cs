using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public Animator playerAnimator;
    public Grid grid;
    public GameManager GM;

    private Vector3 playerWorldOffset;

    private bool mustMove = false;
    private float moveDuration;
    private float timeSinceStartedMoving;

    private void Awake()
    {
        playerWorldOffset = transform.parent.position;
    }

    public bool HasTileUnderneath(Tilemap tilemap)
    {
        Vector3Int posToCheck = tilemap.WorldToCell(transform.position);
        return tilemap.HasTile(posToCheck);
    }

    public Vector3Int GridPosition(Tilemap tilemap)
    {
        return tilemap.WorldToCell(transform.position);
    }

    private void Update()
    {
        if (mustMove)
        {
            transform.parent.position += (transform.parent.localRotation * Vector3.forward * Time.deltaTime) /moveDuration;
            timeSinceStartedMoving += Time.deltaTime;

            if (timeSinceStartedMoving >= moveDuration)
                StopMoving();
        }

    }

    public void MoveForSeconds(float seconds)
    {
        moveDuration = seconds;
        timeSinceStartedMoving = 0f;
        mustMove = true;
    }

    private void StopMoving()
    {
        mustMove = false;
        AlignWithGrid();
        GM.CheckState();
    }

    public void Walk(Vector3Int direction)
    {
        ApplyRotation(direction);
        playerAnimator.SetTrigger("Walk");
    }

    public void Jump(Vector3Int direction)
    {
        ApplyRotation(direction);
        playerAnimator.SetTrigger("Jump");
    }

    // Realigns/Fixes the player position according to the grid -- Fixes small errors of movement
    private void AlignWithGrid()
    {
        Vector3Int cellPos = grid.WorldToCell(transform.position);
        Vector3 fixedPos = grid.CellToWorld(cellPos);

        transform.parent.position = fixedPos + playerWorldOffset;
    }

    // Changes the rotation of the player to face a given direction
    private void ApplyRotation(Vector3Int direction)
    {
        if (direction == Vector3Int.up)
            transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        else if (direction == Vector3Int.down)
            transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else if (direction == Vector3Int.right)
            transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        else if (direction == Vector3Int.left)
            transform.parent.rotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
    }
}
