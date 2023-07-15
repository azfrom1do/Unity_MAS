using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob3Explosion : MonoBehaviour
{
    public SphereCollider explodeArea;
    public float maintain;
    private float maintainTimer;
    
    AudioSource audioSource;

    void Awake()
    {
        maintain = 1.0f;
        maintainTimer = 0;
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
        //if(maintainTimer >= 0.2f) explodeArea.enabled = false;
        if(maintainTimer >= maintain) Destroy(gameObject);
    }

}
