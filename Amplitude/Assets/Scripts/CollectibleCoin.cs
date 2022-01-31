using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCoin : MonoBehaviour
{
    public CoinScore score;


    // Start is called before the first frame update
    void Start()
    {
        score = FindObjectOfType<CoinScore>();
        score.registerCoin();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            score.collectCoin();
            Destroy(this.gameObject);
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
