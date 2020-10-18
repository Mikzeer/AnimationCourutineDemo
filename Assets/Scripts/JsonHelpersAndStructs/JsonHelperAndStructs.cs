using PositionerDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonHelperAndStructs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public struct TotalCardsPerDeck
{
    public int limitOfCardsPerDeck;
}

[System.Serializable]
public struct AmontPerCardsPerLevelPerDeck
{
    public Dictionary<CardRarity, int> AmountPerCardsPerLevelPerDeck;
}

[System.Serializable]
public struct AmountPerCardPerDeck
{
    public Dictionary<int, int> amountPerCardsPerDeck;
}
