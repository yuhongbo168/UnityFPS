using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCanvas : MonoBehaviour
{
    public Character character;
    public TextMeshProUGUI number;
    // Start is called before the first frame update
    void Start()
    {

        if (character != null)
        {
            number.text = character.GetStorePice.ToString();
        }
    }

    public void ChangeCoinCountUI(CoinCanvas canvasUI)
    {

        if (character != null)
        {
            number.text = character.GetStorePice.ToString();
        }

    }
}
