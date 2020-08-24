using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GameManager : MonoBehaviour {
    public GameObject EditorObj;
    public Dropdown dropdown;
    public InputField inputArea;
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
    public Transform imageFX;

    public float planeOffsetMax;
    public float canyongOffsetMax;
    public Transform planeObj;
    public Transform canyongObj;

    private Vector3 planeVec3;
    private Vector3 canyongVec3;

    public UIManager uIManager;

    public GameObject fxRoot;
    public List<FX> flockDeadList = new List<FX> ();

    public void Start () {
        Screen.SetResolution (768, 768, true);
        print ("**********");
        dropdown.options.Clear ();
        for (int i = 0; i < (int) SteamVR_TrackedObject.EIndex.Device16 + 1; i++) {
            Dropdown.OptionData data = new Dropdown.OptionData ();
            data.text = ((SteamVR_TrackedObject.EIndex) i).ToString ();
            dropdown.options.Add (data);
        }
        isShowDropDown = false;
        EditorObj.SetActive (false);
        AudioManager.PlayBgm ("BGM");
        inputArea.text = globalFlock.areaSize.ToString ();
        planeVec3 = planeObj.transform.localPosition;
        canyongVec3 = canyongObj.transform.localPosition;
        uIManager.RefreshGame ();
    }

    public void ChangeDevice (int value) {
        trackTrans.GetComponent<SteamVR_TrackedObject> ().index = (SteamVR_TrackedObject.EIndex) value;
    }

    private float offsetX = 0.0f;
    private float planeOffsetX = 0.0f;
    private float canyongOffsetX = 0.0f;
    void Update () {
        if (Input.GetKeyDown (KeyCode.F5)) {
            isShowDropDown = !isShowDropDown;
            EditorObj.SetActive (isShowDropDown);
            uIManager.AddScore (Vector3.zero);
        }

        if (Input.GetMouseButtonDown (0)) {
            currentTrack = Input.mousePosition;
            touchPos = new Vector2 (currentTrack.x, currentTrack.y);
            for (int i = 0; i < globalFlock.allButterfly.Length; i++) {
                if (globalFlock.allButterfly[i].isDestory || globalFlock.allButterfly[i].isReset)
                    continue;
                expoleDistance = Mathf.Abs (Vector2.Distance (touchPos, globalFlock.allButterfly[i].GetScreenPos ()));
                if (expoleDistance > destoryDistanc) {
                    continue;
                }
                globalFlock.allButterfly[i].Destory ();
                uIManager.AddScore(globalFlock.allButterfly[i].transform.position);
                PlayFlockDead (globalFlock.allButterfly[i].transform.localPosition);
            }
        }

        // if (currentTrack != null && currentTrack != trackTrans.position) {
        //     currentTrack = trackTrans.position;
        //     tempPos = mCamera.WorldToScreenPoint (currentTrack);
        //     touchPos = new Vector2 (tempPos.x, tempPos.y);
        //     if (uIManager.currentState == 1) {
        //         for (int i = 0; i < globalFlock.allButterfly.Length; i++) {
        //             if (globalFlock.allButterfly[i].isDestory || globalFlock.allButterfly[i].isReset)
        //                 continue;
        //             expoleDistance = Mathf.Abs (Vector2.Distance (touchPos, globalFlock.allButterfly[i].GetScreenPos ()));
        //             if (expoleDistance > destoryDistanc) {
        //                 continue;
        //             }
        //             globalFlock.allButterfly[i].Destory ();
        //             uIManager.AddScore (new Vector3 (touchPos.x - 384, touchPos.y - 384));
        //             PlayFlockDead (globalFlock.allButterfly[i].transform.localPosition);
        //         }
        //     }

        //     mouseFX.position = currentTrack;
        //     offsetX = (tempPos.x - 384);
        //     if (offsetX < 0) {
        //         planeOffsetX = Mathf.Abs (offsetX / 384 * planeOffsetMax);
        //         canyongOffsetX = Mathf.Abs (offsetX / 384 * canyongOffsetMax);
        //     } else {
        //         planeOffsetX = -Mathf.Abs (offsetX / 384 * planeOffsetMax);
        //         canyongOffsetX = -Mathf.Abs (offsetX / 384 * canyongOffsetMax);
        //     }
        //     //imageFX.localPosition = new Vector3 (tempPos.x - 384, tempPos.y - 384);
        //     planeObj.transform.localPosition = new Vector3 (planeVec3.x + planeOffsetX, planeVec3.y, planeVec3.z);
        //     canyongObj.transform.localPosition = new Vector3 (canyongVec3.x + canyongOffsetX, canyongVec3.y, canyongVec3.z);
        //     if (uIManager.currentState == 0 &&
        //         Mathf.Abs (Vector3.Distance (new Vector3 (tempPos.x - 384, tempPos.y - 384), new Vector3 (uIManager.beginImage.transform.localPosition.x, uIManager.beginImage.transform.localPosition.y))) < 50) {
        //         uIManager.PlayBegin ();
        //     }
        // }
    }

    public void ChangeFlockArea () {
        globalFlock.areaSize = int.Parse (inputArea.text);
    }

    public void PlayFlockDead (Vector3 pos) {
        FX fx = null;
        for (int i = 0; i < flockDeadList.Count; i++) {
            if (flockDeadList[i].isPlay) {
                continue;
            }
            fx = flockDeadList[i];
            break;
        }
        if (fx == null) {
            GameObject obj = GameObject.Instantiate (Resources.Load ("bufferDeadFx") as GameObject);
            obj.transform.parent = fxRoot.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            fx = obj.GetComponent<FX> ();
            flockDeadList.Add (fx);
        }
        fx.transform.localPosition = new Vector3 (pos.x, pos.y, -1);
        fx.Play ();
    }
}