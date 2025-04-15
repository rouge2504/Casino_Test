using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SlotColumn : MonoBehaviour
{
    [SerializeField] private List<GameObject> symbols;
    [SerializeField] private float spacing = 1.5f;
    private List<Transform> activeSymbols;

    private float resetThreshold;

    [SerializeField] private AnimationCurve spinSpeedCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private float maxSpeed = 20f;
    private float spinTimer = 0f;

    public bool isSpinning;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         resetThreshold = -spacing * symbols.Count;
        activeSymbols = new List<Transform>();
        for (int i = 0; i < symbols.Count; i++){
            GameObject symbol = Instantiate(symbols[i], this.transform);
            symbol.transform.localPosition = new Vector3(0,-i * spacing, 0);
            activeSymbols.Add (symbol.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
         if (!isSpinning) return;
        spinTimer += Time.deltaTime;
        float t = Mathf.Clamp01(spinTimer / spinDuration); 
        float currentSpeed = spinSpeedCurve.Evaluate(t) * maxSpeed;
        if (t >= 1f)
        {
            
            spinTimer = 0;
            isSpinning = false;
            SnapSymbols();
            return;
        }
        foreach (Transform activeSymbol in activeSymbols)
        {
            activeSymbol.localPosition += Vector3.down * currentSpeed * Time.deltaTime;
            if (activeSymbol.localPosition.y <= resetThreshold)
            {
                float highestY  = GetLimitY();
                activeSymbol.localPosition = new Vector3(0, highestY  + spacing, 0);

            }
        }
    }

    private void SnapSymbols()
    {
        Transform closestToZero = null;
        float closestDistance = float.MaxValue;

        foreach (Transform symbol in activeSymbols)
        {
            float distance = Mathf.Abs(symbol.localPosition.y);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestToZero = symbol;
            }
        }

        if (closestToZero == null) return;

        List<Transform> tempList = new List<Transform>(activeSymbols);
        tempList.Sort((a, b) => b.localPosition.y.CompareTo(a.localPosition.y));

        int centerIndex = tempList.IndexOf(closestToZero);

        for (int i = 0; i < tempList.Count; i++)
        {
            int offsetFromCenter = i - centerIndex;
            float newY = -offsetFromCenter * spacing;
            Vector3 newPos = new Vector3(0, newY, 0);
            tempList[i].localPosition = newPos;
        }
    }

    public void StartSpin()
    {
        isSpinning = true;

        spinTimer = 0f;
    }

    public List<string> GetVisibleSymbolNames()
    {
        List<string> result = new List<string>();

        for (int i = 0; i < 3; i++)
        {
            float targetY = -i * spacing;

            foreach (Transform symbol in activeSymbols)
            {
                float diff = Mathf.Abs(symbol.localPosition.y - targetY);
                if (diff < 0.01f) 
                {
                    SpriteRenderer sr = symbol.GetComponent<SpriteRenderer>();
                    if (sr != null && sr.sprite != null)
                        result.Add(sr.sprite.name);
                }
            }
        }

        return result;
    }

    float GetLimitY()
    {
        float maxY  = float.MinValue;
        foreach (Transform symbol in activeSymbols)
        {
            if (symbol.localPosition.y > maxY)
                maxY = symbol.localPosition.y;
        }
        return maxY;
    }
}
