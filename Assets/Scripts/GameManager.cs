using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GameManager : MonoBehaviour
{
    public Dropdown dropdown;
    public GlobalFlock globalFlock;

    private bool isShowDropDown = false;

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
    }

    public void ChangeDevice(int value)
    {
        Debug.Log(value);
        globalFlock.trackTrans.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)value;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            isShowDropDown = !isShowDropDown;
            dropdown.gameObject.SetActive(isShowDropDown);
        }
    }
}
