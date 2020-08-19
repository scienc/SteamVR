using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject BeginRoot;
    public GameObject BattleRoot;
    public Text numLbale;
    public Text timeLabel;

    public GameObject EndRoot;
    public Text endNumLabel;

    public int currentState = 0; //0:idle,1:begin,2:end
    public int currentNum = 0;
    public int currentTime = 0;
    public int MaxTime = 60;

    public void RefreshGame()
    {
        currentState = 0;
        BeginRoot.SetActive(true);
        BattleRoot.SetActive(false);
        EndRoot.SetActive(false);

        currentNum = 0;
        currentTime = 0;
        CancelInvoke("RefreshTime");
    }

    public void BeginGame()
    {
        currentState = 1;
        BeginRoot.SetActive(false);
        BattleRoot.SetActive(true);
        EndRoot.SetActive(false);

        this.currentNum = 0;
        this.currentTime = 0;
        InvokeRepeating("RefreshTime", 0, 1);
    }

    public void EndGame()
    {
        CancelInvoke("RefrehsTime");
        currentState = 2;
        BeginRoot.SetActive(false);
        BattleRoot.SetActive(false);
        EndRoot.SetActive(true);
        endNumLabel.text = currentNum.ToString();
        StartCoroutine(waitBeginTime());
    }

    IEnumerator waitBeginTime()
    {
        yield return new WaitForSeconds(5);
        RefreshGame();
    }

    public void RefreshTime()
    {
        if (currentTime >= MaxTime)
        {
            EndGame();
            return;
        }
        timeLabel.text = (MaxTime - currentTime).ToString();
        currentTime += 1;
    }

    public void AddScore()
    {
        currentNum += 1;
        numLbale.text = currentNum.ToString();
    }
}