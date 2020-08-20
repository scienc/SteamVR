using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour {
    public ParticleSystem fx;

    public bool isPlay = false;

    void Start () {
        fx.Stop ();
        fx.gameObject.SetActive (false);
    }

    public void Play () {
        fx.gameObject.SetActive (true);
        fx.Simulate (0.0f);
        fx.Play ();
        isPlay = true;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update () {
        if (!isPlay)
            return;
        if (!fx.isPlaying) {
            isPlay = false;
            fx.Stop ();
            fx.gameObject.SetActive (false);
        }
    }
}