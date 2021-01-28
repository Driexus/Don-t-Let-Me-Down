using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Map map;
    public Vector3 Offset;
    public Animator playerAnimator;
    public GameManager GM;

    private bool mustMove = false;

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

    public void MoveToDirection(Vector3Int direction)
    {
        Vector3Int targetCoords = GridPosition + direction;

        if (direction == Vector3Int.up)
            transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        else if (direction == Vector3Int.down)
            transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else if (direction == Vector3Int.right)
            transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        else if (direction == Vector3Int.left)
            transform.parent.rotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));


        if (map.ActiveTilemap.HasTile(targetCoords))
        {
            StartCoroutine(MoveForSeconds(1f));
            Walk();
        }
        else
        {
            GM.ResetTimer();
            StartCoroutine(MoveForSeconds(1f));
            Jump();
            map.NextTilemap();
        }
    }

    private void Update()
    {
        if (mustMove)
            transform.parent.position += transform.parent.rotation * Vector3.forward * Time.deltaTime;
    }

    private IEnumerator MoveForSeconds(float seconds)
    {
        mustMove = true;
        yield return new WaitForSeconds(seconds);
        mustMove = false;
        AlignWithGrid();
    }

    private void Walk()
    {
        playerAnimator.SetTrigger("Walk");
    }

    private void Jump()
    {
        playerAnimator.SetTrigger("Jump");
    }

    // Realigns/Fixes the player position according to the grid
    private void AlignWithGrid()
    {

    }    
}
