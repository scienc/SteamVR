using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSingle : MonoBehaviour
{
    // Start is called before the first frame update
    private Collider selfCollider;
    void Awake(){
        selfCollider = this.gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
