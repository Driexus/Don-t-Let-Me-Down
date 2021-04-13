using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    CubeLights lights;

    // Update is called once per frame
    void Update()
    {
        GameObject aCube = Instantiate(cubePrefab, transform);
        lights = aCube.transform.GetChild(0).GetComponent<CubeLights>();

        int r = Random.Range(0, 4);
        lights.SetPlatformColor((DLMD.PlatformColors.PlatformColor)r);
        lights.ResetColor();
    }
}
