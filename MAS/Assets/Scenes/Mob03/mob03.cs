using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mob03 : MonoBehaviour
{
    //public Transform player;
    GameObject player;
    GameObject planeSpawn;
    public GameObject energyEffect;
    public GameObject explosionEffect;
    public Animator anim;
    private Vector3 direction;

    public int health;
    public float mobSpeed;
    public bool doAttack = false;
    public bool getHit = false;
    public bool immune = false;
    public bool canMove = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        planeSpawn = GameObject.FindWithTag("Floor");
        anim = GetComponentInChildren<Animator>();
        planeSpawn.GetComponent<WouldSystem>().mobCount++;
        health = 40;
        mobSpeed = 7.0f;
        canMove = true;
    }

    private void Update()
    {
        HearthCheck();
        Walking();
    }

    private void OnCollisionEnter(Collision col)
    {
        PlayerAttack(col);
    }


    //체력관리
    private void HearthCheck () {
        if(getHit){
            anim.SetBool("isHit", true);
            Invoke("GetHitOut", 0.2f);
        }
        if(health <= 0){
            health = 1;
            anim.SetTrigger("doDie");
            mobSpeed = 0;

            Instantiate(energyEffect, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Invoke("Die", 3);  //자폭 대기시간
        }
    }

    //플레이어를 공격
    private void PlayerAttack (Collision col) {
        if(col.gameObject.tag == "Player"){
            doAttack = true;
            canMove = false;
            anim.SetBool("isAttack", true);
            Invoke("PlayerAttackOut", 1.0f);

            player.GetComponent<player>().health--;
        }
    }
    private void PlayerAttackOut () {
        doAttack = false;
        anim.SetBool("isAttack", false);
        canMove = true;
    }

    //몹 이동
    private void Walking()
    {
        // 플레이어와 몬스터의 위치 계산
        direction = player.transform.position - transform.position;
        direction.Normalize(); // 정규화

        // 몬스터의 이동
        float speed = mobSpeed * Time.deltaTime;
        if(canMove && !getHit) transform.position += direction * speed;

        transform.LookAt(transform.position + direction * speed);   //이동 방향 바라보게
        if(getHit){
            transform.Translate(new Vector3(0, 0.0f, 0.0f), Space.Self);
            Invoke("GetHit", 0.2f);
        }
    }

    //피격
    private void GetHitOut () {
        getHit=false;
        anim.SetBool("isHit", false);
    }
    
    // //피해면역
    // private void Immune () {
    //     if(!immune){
    //         immune = true;
    //         Invoke("ImmuneOut", 0.2f);
    //     }
    // }
    // private void ImmuneOut () {
    //     immune = false;
    // }
    private void Die () {
        planeSpawn.GetComponent<WouldSystem>().mobCount--;
        player.GetComponent<player>().killScore++;
        player.GetComponent<player>().potionCount++;

        Instantiate(explosionEffect, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        Destroy(this.gameObject);
    }

    
}