using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    private TextMeshProUGUI textBox;

    private void Awake()
    {
        textBox = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeValue(LevelData levelData)
    {
        textBox.text = levelData.levelNumber.ToString();
    }
}
