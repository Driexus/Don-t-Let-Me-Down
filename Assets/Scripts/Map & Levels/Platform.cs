using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class Platform : MonoBehaviour
{
    public enum eColor { red, green, blue, purple, pink}
    public eColor color;
    public Tilemap tilemap;

    public static int S = 46, V = 87, H_red = 4, H_green = 100, H_blue = 186, H_purple = 248, H_pink = 292;
    private int H;

    private void Awake()
    {
        if (color == eColor.red)
            H = H_red;
        else if (color == eColor.green)
            H = H_green;
        else if (color == eColor.blue)
            H = H_blue;
        else if (color == eColor.purple)
            H = H_purple;
        else if (color == eColor.pink)
            H = H_pink;

    }

    private void Start()
    {
        tilemap.color = Color.HSVToRGB((float) H/360, (float) S/100, (float) V/100);
    }
}
