using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScoreFxTemplate : MonoBehaviour
{
    //public UGUISpriteAnimation fx;
    public Transform root;

    public System.Action delegateFx;

    public bool isPlay;

    public Tween moveTween;
    public void MoveScore(Vector3 Gbegin, Vector3 end)
    {
        // float angle = Vector3.Dot(Gbegin, end.localPosition);
        // if(angle > 180){
        //     angle -= 180;
        // }
        // else if(angle < -180){
        //     angle += 180;
        // }

        //root.transform.eulerAngles = new Vector3(0, 0, angle);

        root.gameObject.SetActive(true);
        root.transform.position = Gbegin;
        //root.transform.LookAt(end);
        //fx.Play();
        end = new Vector3(0,2.5f,2.5f);
        moveTween = root.DOMove(end, 0.5f).OnComplete(() =>
        {
            isPlay = false;
            root.gameObject.SetActive(false);
            //fx.Stop();
            if (delegateFx != null)
            {
                delegateFx();
            }
        });
        isPlay = true;
    }
}
