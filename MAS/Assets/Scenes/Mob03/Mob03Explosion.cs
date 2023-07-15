using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob03Explosion : MonoBehaviour
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
        //if(maintainTimer >= 0.3) explodeArea.enabled = false;
        if(maintainTimer >= maintain) Destroy(this.gameObject);
    }

}
