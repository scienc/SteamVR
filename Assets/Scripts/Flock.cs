using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class Flock : MonoBehaviour {
    public float speed = 1f;
    float rotationSpeed = 4.0f;
    Vector3 avarageHeading;
    Vector3 avaragePosition;
    float neighbourDistance = 1.0f;
    bool turning = false;
    private Vector3 parentPos;
    private GlobalFlock GlobalFlock;
    private BoxCollider boxCollider;
    public bool isDestory = false;
    public bool isReset = false;
    private int createIndex = -1;
    public System.Action<int> delegateDestoryFlock;

    private Camera mCamera;

    private float size = 0.04f;

    
    public void Start () {
        mCamera = Camera.main;
    }

    public void Init (GlobalFlock gf, int index, float size) {
        GlobalFlock = gf;
        createIndex = index;
        this.gameObject.tag = "Flock";
        if (boxCollider == null) {
            boxCollider = this.gameObject.AddComponent<BoxCollider> ();
            boxCollider.center = new Vector3 (0, 1, 0.5f);
            boxCollider.size = new Vector3 (2, 2, 2);
            boxCollider.isTrigger = true;
        }
        Play ();
    }

    public void Play (bool isRest = false) {
        if (isRest) {
            Vector3 pos = new Vector3 (Random.Range (-GlobalFlock.areaSize, GlobalFlock.areaSize),
                Random.Range (-GlobalFlock.areaSize, GlobalFlock.areaSize), -GlobalFlock.areaSize);
            this.isReset = true;
            Invoke ("BeginTrigger", 1.0f);
            this.transform.localScale = Vector3.zero;
            this.transform.DOScale (Vector3.one * size, 1.0f);
        }

        this.gameObject.SetActive (true);
        parentPos = GlobalFlock.transform.position;
        speed = Random.Range (1f, 2);
        GetComponent<Animator> ().Play ("Butterfly Effect 2 Fly Idle", -1, Random.Range (0f, 0.9f));
        isDestory = false;
    }

    private void BeginTrigger () {
        this.isReset = false;
    }

    private void OnTriggerEnter (Collider collider) {
        // if(this.isReset)
        //     return;
        // var tag = collider.tag;
        // if (tag.Equals("Tracker") && !isDestory)
        // {
        //     Destory();
        // }
    }
    public void Destory () {
        AudioManager.PlaySe ("se_get");
        PlayDead ();
        if (delegateDestoryFlock != null) {
            delegateDestoryFlock (createIndex);
        }
    }

    private void PlayDead () {
        isDestory = true;
        this.gameObject.SetActive (false);
    }

    // Update is called once per frame
    void Update () {
        if (isDestory) {
            return;
        }
        if (Vector3.Distance (transform.position, parentPos) >= GlobalFlock.areaSize) {
            turning = true;
        } else {
            turning = false;
        }
        if (turning) {
            Vector3 direction = parentPos - transform.position;
            transform.rotation = Quaternion.Slerp (transform.rotation,
                Quaternion.LookRotation (direction), rotationSpeed * Time.deltaTime);
            speed = Random.Range (0.5f, 1);
            //Debug.Log("Buffer screen pos " + GetScreenPos());
        } else {
            if (Random.Range (0, 5) < 1) {
                ApplyRules ();
            }
        }
        transform.Translate (0, 0, Time.deltaTime * speed);
    }
    void ApplyRules () {
        Vector3 goalPos = GlobalFlock.goalPos;
        Vector3 Vcentre = parentPos;
        Vector3 vavoid = parentPos;
        float gSpeed = 0.1f;
        float dist;
        int groupSize = 0;
        foreach (Flock go in GlobalFlock.allButterfly) {
            if (!go.isDestory && go.createIndex != createIndex) {
                dist = Vector3.Distance (go.transform.position, this.transform.position);
                if (dist <= neighbourDistance) {
                    Vcentre += go.transform.position;
                    groupSize++;

                    if (dist < 1.0f) {
                        vavoid = vavoid + (this.transform.position - go.transform.position);

                    }
                    Flock anotherFlock = go.GetComponent<Flock> ();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        if (groupSize > 0) {
            Vcentre = Vcentre / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (Vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp (transform.rotation,
                    Quaternion.LookRotation (direction),
                    rotationSpeed * Time.deltaTime);
        }
    }

    Vector3 screenPoint3 = Vector3.zero;
    public Vector2 GetScreenPos () {
        screenPoint3 = mCamera.WorldToScreenPoint (this.transform.position);
        return new Vector2 (screenPoint3.x, screenPoint3.y);
    }
}