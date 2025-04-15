using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
public class SpinManager : MonoBehaviour
{
    [SerializeField] SlotColumn[] slotColumns;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private Button spinButton;

    private Symbol[,] symbols = new Symbol[3,5];

    [SerializeField] private Paytable paytable;
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

        int it_column = 0;
        foreach (SlotColumn slot in slotColumns)
        {
            List<Symbol> symbolVisible = slot.GetVisibleSymbolNames();
            for (int i = 0; i < Constants.MAX_ROW; i++){
                symbols[i, it_column] = symbolVisible[i];
            }
            it_column++;
        }
        DebugMatrix();
        

        int reward = 0;
        foreach(GameObject symbolCheck in paytable.symbolsChecker){
            Prize result = CheckPrize(symbolCheck.GetComponent<Symbol>().id);

            reward = paytable.GetReward(result.symbolId, result.matchCount);

            if (reward > 0)
            {
                Debug.Log($"<color=yellow>Premio: símbolo {result.symbolId} x{result.matchCount} → {reward} créditos</color>");
                return true;
            }
        }

        if (reward == 0){
                 Debug.Log($"<color=red>No hubo premio</color>");
            
        }

        return true;
    }

    private Prize CheckPrize(int id_prize)
    {
        int requiredMatches = 2;
        int it_prize = 0;

        int row = 0;
        int column = 0;

        bool check = false;

        Prize prize = new Prize();

        while (!check)
        {
            if (symbols[row, column].id == id_prize)
            {
                it_prize++;
                column++;

                if (it_prize >= requiredMatches)
                {
                    for (int i = column; i < Constants.MAX_COLUMN; i++){
                        if (symbols[row, i].id == id_prize){
                            it_prize++;

                        }else{
                            break;
                        }
                    }
                    Debug.Log($" <color=green>Ganaste en fila {row} el premio {id_prize} con {it_prize} símbolos consecutivos</color>");
                    prize = new Prize(id_prize, it_prize);
                    return prize; 
                }
            }
            else
            {
                it_prize = 0;
                row++;
                column = 0;
            }

            



            if (column >= Constants.MAX_COLUMN)
            {
                row++;
                column = 0;
                it_prize = 0;
            }

            if (row >= Constants.MAX_ROW)
            {
                check = true;
            }
        }
       
        return prize;
    }



    private void DebugMatrix(){
        string debugger = "";
        for (int i = 0; i < Constants.MAX_ROW; i++){
            for (int j = 0; j < Constants.MAX_COLUMN; j++){
                debugger += symbols[i,j].id + ",";
            }
            debugger += "\n";
        }

        Debug.Log(debugger);
    }
    public void StartSpin(){
        StartCoroutine(Spin());
    }
}
