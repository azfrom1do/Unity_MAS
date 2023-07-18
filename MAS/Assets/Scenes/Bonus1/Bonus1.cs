using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus1 : MonoBehaviour
{
    //public Transform player;
    GameObject player;
    GameObject planeSpawn;
    public Animator anim;
    private Vector3 direction;

    public int health;
    public float mobSpeed;

    public float _distance;
    Rigidbody rigid;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        planeSpawn = GameObject.FindWithTag("Floor");
        anim = GetComponentInChildren<Animator>();
        health = 3;
        mobSpeed = 8.0f;
    }

    private void FixedUpdate()
    {
        HearthCheck();
        Walking();

        //플레이어와의 거리
        _distance = Vector3.Distance(player.transform.position, transform.position);
    }

    //체력관리
    private void HearthCheck () {
        if(health <= 0){
            health = 1;
            anim.SetTrigger("doDie");
            mobSpeed = 0;
            Invoke("Die", 1);
        }
    }

    //몹 이동
    private void Walking()
    {
        // 플레이어와 몬스터의 위치 계산
        direction = transform.position - player.transform.position;
        direction.Normalize(); // 정규화

        // 몬스터의 이동
        float speed = mobSpeed * Time.deltaTime;
        if(_distance <= 10.0f) {
            transform.position += direction * speed;
            transform.LookAt(transform.position + direction * speed);   //이동 방향 바라보게
            anim.SetBool("isRun", true);
        }
        else anim.SetBool("isRun", false);
    }

    private void Die () {
        player.GetComponent<player>().killScore++;
        player.GetComponent<player>().potionCount++;
        player.GetComponent<player>().itemPoint++;
        Destroy(this.gameObject);
    }
}
