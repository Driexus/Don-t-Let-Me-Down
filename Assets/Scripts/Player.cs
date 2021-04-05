using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{ 
    public GameManager GM;
    public Animator playerAnimator;

    // TODO: Switch these to private (?)
    [HideInInspector]
    public Grid grid;

    [HideInInspector]
    public Vector3 playerWorldOffset;

    public bool IsMoving
    {
        get { return isMoving; }
    }
    private bool isMoving = false;
    private float moveDuration;
    private float timeSinceStartedMoving;

    private void Awake()
    {
        playerWorldOffset = transform.position;
        GM.lm.OnLevelFailed += Fall;
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
        if (isMoving)
        {
            transform.position += transform.localRotation * Vector3.forward * Time.deltaTime / moveDuration;
            timeSinceStartedMoving += Time.deltaTime;

            if (timeSinceStartedMoving >= moveDuration)
                StopMoving();
        }
    }

    public void MoveForSeconds(float seconds)
    {
        moveDuration = seconds;
        timeSinceStartedMoving = 0f;
        isMoving = true;
    }

    private void StopMoving()
    {
        AlignWithGrid();
        GM.CheckState();
        isMoving = false;
    }

    // Idle animation facing the same direction as before (default implementation)
    public void Idle()
    {
        playerAnimator.SetTrigger("Idle");
    }

    // Idle animation facing a specific direction
    public void Idle(Vector3Int direction)
    {
        ApplyRotation(direction);
        playerAnimator.SetTrigger("Idle");
    }

    public void Walk(Vector3Int direction)
    {
        ApplyRotation(direction);
        playerAnimator.SetTrigger("Walk");
        playerAnimator.ResetTrigger("Idle");
    }

    public void Jump(Vector3Int direction)
    {
        ApplyRotation(direction);
        playerAnimator.SetTrigger("Jump");
        playerAnimator.ResetTrigger("Idle");
    }

    public void Fall()
    {
        playerAnimator.SetTrigger("Fall");
        playerAnimator.ResetTrigger("Idle");
    }

    // Realigns/Fixes the player position according to the grid -- Fixes small errors of movement
    private void AlignWithGrid()
    {
        Vector3Int cellPos = grid.WorldToCell(transform.position);
        Vector3 fixedPos = grid.CellToWorld(cellPos);

        transform.position = fixedPos + playerWorldOffset;
    }

    // Changes the rotation of the player to face a given direction
    private void ApplyRotation(Vector3Int direction)
    {
        if (direction == Vector3Int.up)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        else if (direction == Vector3Int.down)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else if (direction == Vector3Int.right)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        else if (direction == Vector3Int.left)
            transform.rotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
    }

    // Used for first jump
    public IEnumerator JumpAndWait(Vector3Int direction)
    {
        Jump(direction);
        while (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            yield return null;
        
        Idle(direction);
        while (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            yield return null;
    }    
}