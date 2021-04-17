using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    [Range(0, 1)]
    public float timescale;

    private void OnEnable()
    {
        Time.timeScale = timescale;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
