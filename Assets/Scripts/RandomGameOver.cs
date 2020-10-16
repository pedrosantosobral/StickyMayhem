using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomGameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public List<string> strings;

    private string textToBeSet;

    public void GetNewText()
    {
        textToBeSet = strings[Random.Range(0, strings.Count)];
        text.SetText(textToBeSet);
    }

}
