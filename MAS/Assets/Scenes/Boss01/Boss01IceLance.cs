//using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01IceLance : MonoBehaviour
{
    public float maintain;
    private float maintainTimer;
    
    AudioSource audioSource;

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Awake()
    {
        maintain = 3.0f;

        //audioSource.Play();

        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
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

    // private void OnCollisionEnter(Collider col) {
    //     if(col.gameObject.tag == "Player"){
    //         //col.GetComponent<Player>().health -= 1;
    //         Debug.Log("1");
    //     }
    // }

    void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag == "Player"){
            //other.GetComponent<player>().health -= 1;
            other.GetComponent<player>().getHit_bossSkill = true;
            Debug.Log("플레이어 명중");
        }
    }
}
