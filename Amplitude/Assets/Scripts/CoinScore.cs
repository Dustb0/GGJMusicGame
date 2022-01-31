using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinScore : MonoBehaviour
{

    public TextMeshProUGUI tmp;

    public int coinsAvailable { get; private set; } = 0;
    public int coinsCollected { get; private set; } = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void collectCoin()
    {
        coinsAvailable += 1;
        UpdateText();
    }

    public void registerCoin()
    {
        coinsCollected += 1;
        UpdateText();
    }

    private void UpdateText()
    {
        tmp.SetText(coinsAvailable + " / " + coinsCollected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
