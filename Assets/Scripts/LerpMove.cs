using UnityEngine;

public class LerpMove : MonoBehaviour
{
    // The total time taken to move
    public float LerpTime = 5f;

    public bool IsMoving { get { return isLerping; } }
    private bool isLerping = false;
    private float TimeStartedLerping = 0f;

    private Vector3 StartLerpPosition;
    private Vector3 EndLerpPosition;


    public void MoveTo(Vector3 targetPos)
    {
        StartLerpPosition = transform.position;
        EndLerpPosition = targetPos;

        TimeStartedLerping = Time.time;
        isLerping = true;
    }

    private void Update()
    {
        // If we are lerping apply the new lerp position in each frame
        if (isLerping)
        {

            float timeSinceStarted = Time.time - TimeStartedLerping;
            float t = timeSinceStarted / LerpTime;

            // The ease function we are using
            // https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/
            // https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            transform.position = Vector3.Lerp(StartLerpPosition, EndLerpPosition, t);

            if (timeSinceStarted >= LerpTime)
            {
                isLerping = false;
                // Small error fixing
                transform.position = EndLerpPosition;
            }
        }
    }
}
