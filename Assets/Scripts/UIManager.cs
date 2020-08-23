using System;
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

    public int currentState = 0; //0:idle,1:begin,2:end
    public int currentNum = 0;
    public int currentTime = 0;
    public int MaxTime = 60;

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

    public void AddScore()
    {
        if (!scoreFx.IsPlaying)
        {
            scoreFx.animFinsih = () =>
            {
                scoreFx.gameObject.SetActive(false);
            };
            scoreFx.gameObject.SetActive(true);
            scoreFx.Rewind();
        }
        StartCoroutine(waitPlaySE());
    }

    IEnumerator waitPlaySE()
    {
        yield return new WaitForSeconds(0.2f);
        currentNum += 1;
        numLbale.text = currentNum.ToString();
        numTween.Kill();
        numTween.Append(numLbale.transform.DOScale(Vector3.one, 0.2f));
        numTween.Append(numLbale.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.4f));
        numLbale.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
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
}