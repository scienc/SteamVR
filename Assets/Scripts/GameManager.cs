using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GameManager : MonoBehaviour
{
    public Dropdown dropdown;
    public GlobalFlock globalFlock;

    public int destoryDistanc = 50;
    public Transform trackTrans;

    private bool isShowDropDown = false;

    float expoleDistance = 0.0f;
    Vector2 touchPos = Vector2.zero;
    Vector3 currentTrack = Vector3.zero;
    Vector3 tempPos = Vector3.zero;
    public Camera mCamera;

    public Transform mouseFX;

    public UIManager uIManager;
    public void Start()
    {
        dropdown.options.Clear();
        for (int i = 0; i < (int)SteamVR_TrackedObject.EIndex.Device16 + 1; i++)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = ((SteamVR_TrackedObject.EIndex)i).ToString();
            dropdown.options.Add(data);
        }
        isShowDropDown = false;
        dropdown.gameObject.SetActive(false);
        AudioManager.PlayBgm("BGM");
        uIManager.RefreshGame();
    }

    public void ChangeDevice(int value)
    {
        trackTrans.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)value;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            isShowDropDown = !isShowDropDown;
            dropdown.gameObject.SetActive(isShowDropDown);
        }

        if (currentTrack != null && currentTrack != trackTrans.position)
        {
            currentTrack = trackTrans.position;
            tempPos = mCamera.WorldToScreenPoint(currentTrack);
            touchPos = new Vector2(tempPos.x, tempPos.y);
            for (int i = 0; i < globalFlock.allButterfly.Length; i++)
            {
                if (globalFlock.allButterfly[i].isDestory || globalFlock.allButterfly[i].isReset)
                    continue;
                expoleDistance = Mathf.Abs(Vector2.Distance(touchPos, globalFlock.allButterfly[i].GetScreenPos()));
                if (expoleDistance > destoryDistanc)
                {
                    continue;
                }
                globalFlock.allButterfly[i].Destory();
                uIManager.AddScore();
            }
            mouseFX.position = currentTrack;
            //mouseFX.position = tempPos;
        }
    }
}