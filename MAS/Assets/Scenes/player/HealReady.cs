using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealReady : MonoBehaviour
{
    public float maintain;
    private float maintainTimer;
    
    AudioSource audioSource;

    void Awake()
    {
        maintain = 1.0f;

        //audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        maintainTimer += Time.deltaTime;
        DestroyCheck ();
    }

    private void DestroyCheck () {
        if(maintainTimer >= maintain) {
            Destroy(this.gameObject);
        }
    }

}
