using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponAttack : MonoBehaviour
{
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public GameObject planeSpawn;
    public GameObject player;

    AudioSource audioSource;
    public AudioClip audioAttack1;
    public AudioClip audioAttack2;
    public AudioClip audioAttack3;

    public bool doAttack = false;
    public int damage;

    void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        Use();
    }

    //접촉(활성화 필요)
    private void OnTriggerEnter(Collider col) {
        AttackSound();
        if(col.gameObject.tag == "Mob"){
            if(col.gameObject.name == "Boss01(Clone)"){
                col.GetComponent<Boss01>().getHit = true;
                col.GetComponent<Boss01>().health -= 1;
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
                col.GetComponent<Bonus1>().health -= 1;
            }
        }
    }

    //활성화
    public void Use(){
        if(doAttack){
            meleeArea.enabled = true;
            trailEffect.enabled = true;
        }else{
            meleeArea.enabled = false;
            trailEffect.enabled = false;
        }
    }
    

    void AttackSound(){
        int randomInt = Random.Range(1, 4);
        switch (randomInt) {
            case 1:
                audioSource.clip = audioAttack1;
                break;
            case 2:
                audioSource.clip = audioAttack2;
                break;
            case 3:
                audioSource.clip = audioAttack3;
                break;
            default :
                audioSource.clip = audioAttack1;
                break;
        }
        audioSource.Play();
    }
}
