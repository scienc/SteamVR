﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject BeginRoot;
    public RectTransform beginImage;
    public GameObject BattleRoot;
    public Text numLbale;
    private Sequence numTween;
    public Text timeLabel;
    private Sequence timeTween;
    public UGUISpriteAnimation scoreFx;

    public GameObject EndRoot;
    public Text endNumLabel;

    public int currentState = 0; //0:idle,1:begin,2:end,3:playbeign
    public int currentNum = 0;
    public int currentTime = 0;
    public int MaxTime = 60;

    public Transform ScoreMoveRoot;
    public ScoreFxTemplate scoreFxTemplate;
    public List<ScoreFxTemplate> scoreFxList = new List<ScoreFxTemplate>();


    void Start()
    {
        timeTween = DOTween.Sequence();
        numTween = DOTween.Sequence();
    }

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
        numLbale.text = this.currentNum.ToString();
        CancelInvoke("RefreshTime");
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
        if (currentState != 1)
        {
            return;
        }
        if (currentTime >= MaxTime)
        {
            EndGame();
            return;
        }
        if (MaxTime - currentTime <= 5)
        {
            AudioManager.PlaySe("se_countdown");
            timeTween.Kill();
            timeTween.Append(timeLabel.transform.DOScale((Vector3.one), 0.2f));
            timeTween.Append(timeLabel.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.4f));
            timeLabel.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            timeTween.PlayForward();
        }
        //timeLabel.text = (MaxTime - currentTime).ToString ();
        TimeSpan ts = TimeSpan.FromSeconds(MaxTime - currentTime);
        timeLabel.text = ToStringMinute(ts);
        currentTime += 1;
    }

    public void AddScore(Vector3 uiPos)
    {
        ScoreFxTemplate template = null;
        for (int i = 0; i < scoreFxList.Count; i++)
        {
            if (scoreFxList[i].isPlay)
                continue;
            template = scoreFxList[i];
        }
        if (template == null)
        {
            GameObject obj = GameObject.Instantiate(scoreFxTemplate.gameObject) as GameObject;
            obj.transform.parent = ScoreMoveRoot;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            template = obj.GetComponent<ScoreFxTemplate>();
            template.isPlay = true;
            scoreFxList.Add(template);
        }
        template.delegateFx = waitPlaySE;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(numLbale.transform.localPosition.x + 384, numLbale.transform.localPosition.y + 384));
        template.MoveScore(uiPos, new Vector3(worldPos.x, worldPos.y));
    }

    private void waitPlaySE()
    {
        //yield return new WaitForSeconds (0.2f);
        if (!scoreFx.IsPlaying)
        {
            scoreFx.animFinsih = () =>
            {
                scoreFx.gameObject.SetActive(false);
            };
            scoreFx.gameObject.SetActive(true);
            scoreFx.Rewind();
        }

        currentNum += 1;
        numLbale.text = currentNum.ToString();
        numTween.Kill();
        numTween.Append(numLbale.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.2f));
        numTween.Append(numLbale.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.4f));
        numLbale.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        numTween.PlayForward();
        AudioManager.PlaySe("se_number");
    }
    public string ToStringMinute(TimeSpan span)
    {
        TimeSpan t = span;
        if (t.TotalSeconds < 0)
        {
            return "00:00";
        }
        else
        {
            var format = "{0:D2}:{1:D2}";
            return string.Format(format, (int)t.TotalMinutes, t.Seconds);
        }
    }

    public void PlayBegin()
    {
        currentState = 3;
        Sequence sq = DOTween.Sequence();
        sq.Append(beginImage.transform.DOScale(Vector3.one * 1.2f, 0.2f));
        sq.Append(beginImage.transform.DOScale(Vector3.one, 0.2f));
        sq.OnComplete(() =>
        {
            BeginGame();
        });
        sq.Play();
    }
}