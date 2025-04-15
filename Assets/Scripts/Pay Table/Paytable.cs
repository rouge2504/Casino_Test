using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Paytable", menuName = "Slot Machine/Paytable")]
public class Paytable : ScriptableObject
{
    public GameObject[] symbolsChecker;
    public List<PaytableEntry> entries = new List<PaytableEntry>();

    public int GetReward(int symbolId, int matchCount)
    {
        foreach (var entry in entries)
        {
            if (entry.symbolId == symbolId && entry.matchCount == matchCount)
                return entry.reward;
        }
        return 0;
    }
}
