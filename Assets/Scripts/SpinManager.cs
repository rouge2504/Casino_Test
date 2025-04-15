using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
public class SpinManager : MonoBehaviour
{
    [SerializeField] SlotColumn[] slotColumns;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private Button spinButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spin(){
        spinButton.interactable = false;
        foreach(SlotColumn slotColumn in slotColumns){
            slotColumn.StartSpin();
            yield return new WaitForSeconds(waitTime);
        }
        yield return new WaitUntil(() => AllColumnsStopped());

        spinButton.interactable = true;
    }

    private bool AllColumnsStopped()
    {
        foreach (SlotColumn slot in slotColumns)
        {
            
            if (slot.isSpinning)
                return false;
        }

        foreach (SlotColumn slot in slotColumns)
        {
            List<string> visible = slot.GetVisibleSymbolNames();
            Debug.Log($"Column {slot.name}: {string.Join(", ", visible)}");
        }

        return true;
    }
    public void StartSpin(){
        StartCoroutine(Spin());
    }
}
