using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScoreFxTemplate : MonoBehaviour
{
    public UGUISpriteAnimation fx;
    public Transform root;

    public System.Action delegateFx;

    public bool isPlay;

    public Tween moveTween;
    public void MoveScore(Vector3 Gbegin, Transform end)
    {
        root.gameObject.SetActive(true);
        root.transform.localPosition = Gbegin;
        //root.transform.LookAt(end);
        fx.Play();
        moveTween = root.DOLocalMove(end.localPosition, 0.5f).OnComplete(() =>
        {
            isPlay = false;
            root.gameObject.SetActive(false);
            fx.Stop();
            if (delegateFx != null)
            {
                delegateFx();
            }
        });        
        isPlay = true;
    }
}
