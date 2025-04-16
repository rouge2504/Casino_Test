using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
public class SpinManager : MonoBehaviour
{
    [SerializeField] SlotColumn[] slotColumns;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private Button spinButton;

    private Symbol[,] symbols = new Symbol[3,5];

    [SerializeField] private Paytable paytable;

    [SerializeField] private TextMeshProUGUI creditText;

    [SerializeField] private int credits;

    [SerializeField] private Transform highlightContent;
    private Transform[] highlights;

    private bool startSpin;

    void Start()
    {
        startSpin = false;
        InitHighlights();
    }

    private void InitHighlights(){
        highlights = new Transform[highlightContent.childCount];
        for (int i = 0; i < highlightContent.childCount; i++){
            highlights[i] = highlightContent.GetChild(i);
            highlights[i].gameObject.SetActive(false);
        }
        
    }

    private void SetHighlight(Vector3 pos, int it){
        highlights[it].position = pos;
        highlights[it].gameObject.SetActive(true);

    }

    private void ResetHighlights(){
        for (int i = 0; i < highlightContent.childCount; i++){
            highlights[i].gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !startSpin){
            startSpin = true;
            StartCoroutine(Spin());
            ResetHighlights();
        }
    }

    IEnumerator Spin(){
        if (startSpin){
        spinButton.interactable = false;
        foreach(SlotColumn slotColumn in slotColumns){
            slotColumn.StartSpin();
            yield return new WaitForSeconds(waitTime);
        }
        yield return new WaitUntil(() => AllColumnsStopped());

        spinButton.interactable = true;
        startSpin = false;
    }
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
        
        credits += ProcessReward();


        creditText.text = "Creditos: " + credits;
        

        return true;
    }

    private int CheckLinePattern(IntMatrix2D pattern, int targetSymbolId)
    {
        int it_prize = 0;
        for (int row = 0; row < pattern.rows; row++)
        {
            for (int col = 0; col < pattern.columns; col++)
            {
                int expected = pattern.Get(row, col);

                if (expected == 1)
                {

                    if (symbols[row, col].id != targetSymbolId){
                        ResetHighlights();
                        return 0; 
                    }else{
                        it_prize++;
                        SetHighlight(symbols[row, col].gameObject.transform.position, it_prize);

                    }
                }
            }
        }
        return it_prize; 
    }

    private int ProcessReward(){
        int reward = 0;
        foreach(GameObject symbolCheck in paytable.symbolsChecker){
            int id = symbolCheck.GetComponent<Symbol>().id;
            foreach (PaytablePattern pattern in paytable.patterns)
            {
                int mult = CheckLinePattern(pattern.pattern, id);
            if (mult != 0)
            {
                mult *= paytable.GetReward(id, mult);
                Debug.Log($"<color=green> Coincidencia con patrón para símbolo {id}. Recompensa: {mult}</color>");
                return mult;
            }
            }
            Prize result = CheckPrize(symbolCheck.GetComponent<Symbol>().id);
            reward = paytable.GetReward(result.symbolId, result.matchCount);
            if (reward > 0)
            {
                Debug.Log($"<color=yellow>Premio: símbolo {result.symbolId} x{result.matchCount} → {reward} créditos</color>");
                return reward;
            }
        }

        if (reward == 0){
                 Debug.Log($"<color=red>No hubo premio</color>");

        }
        return reward;
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
                SetHighlight(symbols[row, column].gameObject.transform.position, it_prize);
                it_prize++;
                column++;

                if (it_prize >= requiredMatches)
                {
                    for (int i = column; i < Constants.MAX_COLUMN; i++){
                        if (symbols[row, i].id == id_prize){
                            SetHighlight(symbols[row, i].gameObject.transform.position, it_prize);
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
                ResetHighlights();
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
        startSpin = true;
        StartCoroutine(Spin());
        ResetHighlights();
    }
}
