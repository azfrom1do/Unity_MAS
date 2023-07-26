using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallExplosion : MonoBehaviour
{
    GameObject player;
    public int damage;
    public float maintain;
    private float maintainTimer;
    
    AudioSource audioSource;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        damage = 2;
        damage += (player.GetComponent<player>().FB_Level * 4);
        maintain = 1.5f;
        
        Debug.Log(player.GetComponent<player>().FB_Level + "레벨 염구 폭발");


        //player.GetComponent<player>().level;
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

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Mob" && maintainTimer <= 0.5f){
            if(col.gameObject.name == "Boss01(Clone)"){
                col.GetComponent<Boss01>().getFireHit = true;
                col.GetComponent<Boss01>().health -= 10;
            }
            if(col.gameObject.name == "Execut(Clone)"){
                col.GetComponent<Execut>().getHit = true;
                col.GetComponent<Execut>().health -= damage;
            }

            if(col.gameObject.name == "Mob0(Clone)"){
                col.GetComponent<mob0>().getHit = true;
                col.GetComponent<mob0>().health -= damage;
            }
            if(col.gameObject.name == "Mob1(Clone)"){
                col.GetComponent<mob1>().getHit = true;
                col.GetComponent<mob1>().health -= damage;
            }
            if(col.gameObject.name == "Mob2(Clone)"){
                col.GetComponent<mob2>().getHit = true;
                col.GetComponent<mob2>().health -= damage;
            }
            if(col.gameObject.name == "Mob3(Clone)"){
                col.GetComponent<mob3>().getHit = true;
                col.GetComponent<mob3>().health -= damage;
            }
            // if(col.gameObject.name == "Mob4(Clone)"){
            //     col.GetComponent<mob4>().getHit = true;
            //     col.GetComponent<mob4>().health -= damage;
            // }
            // if(col.gameObject.name == "Mob5(Clone)"){
            //     col.GetComponent<mob5>().getHit = true;
            //     col.GetComponent<mob5>().health -= damage;
            // }

            if(col.gameObject.name == "Mob00(Clone)"){
                col.GetComponent<mob00>().getHit = true;
                col.GetComponent<mob00>().health -= damage;
            }
            if(col.gameObject.name == "Mob01(Clone)"){
                col.GetComponent<mob01>().getHit = true;
                col.GetComponent<mob01>().health -= damage;
            }
            if(col.gameObject.name == "Mob02(Clone)"){
                col.GetComponent<mob02>().getHit = true;
                col.GetComponent<mob02>().health -= damage;
            }
            if(col.gameObject.name == "Mob03(Clone)"){
                col.GetComponent<mob03>().getHit = true;
                col.GetComponent<mob03>().health -= damage;
            }
            // if(col.gameObject.name == "Mob04(Clone)"){
            //     col.GetComponent<mob04>().getHit = true;
            //     col.GetComponent<mob04>().health -= damage;
            // }
            // if(col.gameObject.name == "Mob05(Clone)"){
            //     col.GetComponent<mob05>().getHit = true;
            //     col.GetComponent<mob05>().health -= damage;
            // }
        }
        if(col.gameObject.tag == "Bonus"){
            if(col.gameObject.name == "Bonus1(Clone)"){
                col.GetComponent<Bonus1>().getHit = true;
                col.GetComponent<Bonus1>().health -= 1;
            }
        }
    }
    private void DestroyCheck () {
        if(maintainTimer >= maintain) {
            Destroy(this.gameObject);
        }
    }

}
