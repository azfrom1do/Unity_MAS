using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    // GameObject player;
    public GameObject explosionEffect;
    public float skillSpeed; // 이동 속도
    // public int damage;
    public float range;
    private float rangeTimer;

    AudioSource audioSource;

    void Awake()
    {
        // player = GameObject.FindWithTag("Player");
        skillSpeed = 100.0f;
        // damage = 3 + player.GetComponent<player>().level;
        range = 1.0f;
        GetComponent<Rigidbody>().AddForce(this.transform.forward * skillSpeed);

        //audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(this.transform.forward * skillSpeed);
        rangeTimer += Time.deltaTime;
        DestroyCheck ();
    }

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Mob"){
            // if(col.gameObject.name == "Mob1(Clone)"){
            //     col.GetComponent<mob1>().getHit = true;
            //     col.GetComponent<mob1>().health -= damage;
            // }
            // if(col.gameObject.name == "Mob2(Clone)"){
            //     col.GetComponent<mob2>().getHit = true;
            //     col.GetComponent<mob2>().health -= damage;
            // }

            // if(col.gameObject.name == "Mob01(Clone)"){
            //     col.GetComponent<mob01>().getHit = true;
            //     col.GetComponent<mob01>().health -= damage;
            // }
            // if(col.gameObject.name == "Mob02(Clone)"){
            //     col.GetComponent<mob02>().getHit = true;
            //     col.GetComponent<mob02>().health -= damage;
            // }
            Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    private void DestroyCheck () {
        if(rangeTimer >= range) {
            Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
