using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class StatShower : MonoBehaviour
{ 
    TMP_Text statText;
    public GameData.DataType dataType;
    public string staticText;
    public string noValueText;

    private void Start()
    {
        statText = GetComponent<TMP_Text>();
        int value = Saver.LoadData().GetDataValue(dataType);

        if (value < 0)
            statText.text = noValueText;
        else
            statText.text = staticText + value.ToString();
    }
}
