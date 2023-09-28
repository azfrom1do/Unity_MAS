using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mob02 : MonoBehaviour
{
    //public Transform player;
    GameObject player;
    GameObject planeSpawn;
    public Animator anim;
    private Vector3 direction;

    public int health;
    public float mobSpeed;
    public bool doAttack = false;
    public bool getHit = false;
    public bool immune = false;
    public bool canMove = false;

    Rigidbody rigid;
    //public CapsuleCollider thisCol;
    public float jumpingCool;
    public bool canJump = false;
    private bool jumpingBool = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        planeSpawn = GameObject.FindWithTag("MainFloor");
        anim = GetComponentInChildren<Animator>();
        planeSpawn.GetComponent<WouldSystem>().mobCount++;
        health = 10;
        mobSpeed = 1.5f;
        canMove = true;

        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        jumpingCool += Time.deltaTime;
        HearthCheck();
        Walking();
        JumpReady ();
    }

    private void OnCollisionEnter(Collision col)
    {
        PlayerAttack(col);
        Landing (col);
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
            Invoke("Die", 1);
        }
    }

    //플레이어를 공격
    private void PlayerAttack (Collision col) {
        if(col.gameObject.tag == "Player"){
            doAttack = true;
            canMove = false;
            anim.SetBool("isAttack", true);
            Invoke("PlayerAttackOut", 1.0f);
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
        if(canMove && !jumpingBool) transform.position += direction * speed;

        if(!jumpingBool)
        transform.LookAt(transform.position + direction * speed);   //이동 방향 바라보게
        if(getHit){
            transform.Translate(new Vector3(0, 0.0f, -0.1f), Space.Self);
            Invoke("GetHit", 0.2f);
        }
        if(jumpingBool)
        transform.Translate(new Vector3(0, 0.0f, 100.0f) * Time.deltaTime, Space.Self);
        

        //anim.SetBool("isWalk", true);
        //anim.SetBool("isWalk", direction != Vector3.zero);
    }

    //피격
    private void GetHitOut () {
        getHit=false;
        anim.SetBool("isHit", false);
    }
    
    //피해면역
    private void Immune () {
        if(!immune){
            immune = true;
            Invoke("ImmuneOut", 0.2f);
        }
    }
    private void ImmuneOut () {
        immune = false;
    }
    private void Die () {
        planeSpawn.GetComponent<WouldSystem>().mobCount--;
        player.GetComponent<player>().killScore++;
        player.GetComponent<player>().potionCount++;
        Destroy(this.gameObject);
    }

    //몹 고유 스킬  mobSpeed
    private void JumpReady () {
        if(jumpingCool >= 3.0f){
            jumpingCool = 0.0f;
            Invoke("Jump", 0.5f);

            anim.SetTrigger("JumpReady");
            //anim.SetBool("isAttack", true);
        }
    }
    private void Jump () {
        jumpingCool = 0.0f;
        jumpingBool = true;
        mobSpeed = 100.0f;
        Invoke("JumpOut", 0.2f);
        rigid.AddForce(Vector3.up * 15.0f, ForceMode.Impulse);

        anim.SetBool("Jump", true);
    }
    private void JumpOut () {
        jumpingCool = 0.0f;
        jumpingBool = false;
        mobSpeed = 1.5f;

        anim.SetBool("Jump", false);
    }
    private void Landing (Collision col) {
        if(col.gameObject.tag == "Floor" && jumpingBool){
            jumpingCool = 0.0f;
            jumpingBool = false;
            mobSpeed = 1.5f;
            

            anim.SetBool("Jump", false);
        }
    }

    
}
