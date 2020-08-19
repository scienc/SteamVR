using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject BeginRoot;

    public GameObject BattleRoot;
    public Text numLbale;
    public Text timeLabel;

    public GameObject EndRoot;

    public int currentState = 0; //0:idle,1:begin,2:end
    public int currentNum = 0;
    public int currentTime = 0;

    public void RefreshGame () {
        currentState = 0;
        BeginRoot.SetActive (true);
        BattleRoot.SetActive (false);
        EndRoot.SetActive (false);

    }

    public void BeginGame () {
        currentState = 1;
        BeginRoot.SetActive (false);
        BattleRoot.SetActive (true);
        EndRoot.SetActive (false);

        this.currentNum = 0;
    }

    public void EndGame () {
        currentState = 2;
        BeginRoot.SetActive (false);
        BattleRoot.SetActive (true);
        EndRoot.SetActive (false);

    }
}