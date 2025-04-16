using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Paytable", menuName = "Slot Machine/Paytable")]
public class Paytable : ScriptableObject
{
    public GameObject[] symbolsChecker;
    public List<PaytableEntry> entries = new List<PaytableEntry>();

    public List<PaytablePattern> patterns = new List<PaytablePattern>();


    private void OnEnable()
    {
        if (patterns == null || patterns.Count == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                PaytablePattern p = new PaytablePattern();
                p.pattern.rows = Constants.MAX_ROW;
                p.pattern.columns = Constants.MAX_COLUMN;
                p.pattern.Initialize();
                p.reward = 100 * (i + 1);
                patterns.Add(p);
            }
        }
    }

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
