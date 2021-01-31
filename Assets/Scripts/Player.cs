using UnityEngine;

public class Player : MonoBehaviour
{
    public Map map;
    public Vector3 Offset;
    public Animator playerAnimator;
    public GameManager GM;

    private Vector3 playerWorldOffset;

    private bool mustMove = false;
    private float moveDuration;
    private float timeSinceStartedMoving;

    private void Awake()
    {
        playerWorldOffset = transform.parent.position;
    }

    public bool HasTileUnderneath
    {
        get
        {
            Vector3Int posToCheck = map.ActiveTilemap.WorldToCell(transform.position);
            return map.ActiveTilemap.HasTile(posToCheck);
        }
    }

    public Vector3Int GridPosition
    {
        get
        {
            return map.ActiveTilemap.WorldToCell(transform.position);
        }
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

    public void MoveToDirection(Vector3Int direction)
    {        
        Vector3Int targetCoords = GridPosition + direction;
        if (map.ActiveTilemap.HasTile(targetCoords))
        {
            Walk(direction);
        }
        else
        {
            GM.ResetTimer();
            Jump(direction);
            map.NextTilemap();
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
        Vector3Int cellPos = map.ActiveTilemap.WorldToCell(transform.position);
        Vector3 fixedPos = map.ActiveTilemap.CellToWorld(cellPos);

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
