using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01 : MonoBehaviour
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

    public float _distance;
    public GameObject iceEffect;
    Rigidbody rigid;
    public bool getFireHit = false;
    public float skillCool;
    public bool isFlying = false;
    private bool FlyingBool = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        planeSpawn = GameObject.FindWithTag("Floor");
        anim = GetComponentInChildren<Animator>();
        planeSpawn.GetComponent<WouldSystem>().mobCount++;
        health = 100;
        mobSpeed = 5.0f;
        canMove = true;

        rigid = GetComponent<Rigidbody>();
        isFlying = false;
    }

    private void FixedUpdate()
    {
        skillCool += Time.deltaTime;
        HearthCheck();
        Walking();
        Skill ();

        //플레이어와의 거리
        _distance = Vector3.Distance(player.transform.position, transform.position);
    }

    private void OnCollisionEnter(Collision col)
    {
        PlayerAttack(col);
        //Landing (col);
        GetMobSkillDamage(col);
    }


    //체력관리
    private void HearthCheck () {
        if(getHit){
            getHit = false;
            anim.SetBool("isHit", true);
            Invoke("GetHitOut", 0.2f);
        }
        if(health <= 0){
            health = 1;
            anim.SetTrigger("doDie");
            mobSpeed = 0;
            Invoke("Die", 4.0f);
        }
        if(getFireHit){
            getFireHit = false;
            mobSpeed = 0;
            skillCool = 0;
            anim.SetBool("GetFireHit", true);
            Invoke("GetHitOut", 1.0f);
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

    //화염피격
    private void GetMobSkillDamage (Collision col) {
        if(col.gameObject.tag == "Mob_Skill"){
            health -= 10;
            getFireHit = true;
        }
    }

    //몹 이동
    private void Walking()
    {
        // 플레이어와 몬스터의 위치 계산
        direction = player.transform.position - transform.position;
        direction.Normalize(); // 정규화

        // 몬스터의 이동
        float speed = mobSpeed * Time.deltaTime;
        if(canMove && !FlyingBool) transform.position += direction * speed;
        if(!FlyingBool)
        transform.LookAt(transform.position + direction * speed);   //이동 방향 바라보게
        if(getHit){
            transform.Translate(new Vector3(0, 0.1f, -1.0f) * speed, Space.Self);
            Invoke("GetHit", 0.2f);
        }
        if(FlyingBool)
        transform.Translate(new Vector3(0, 0.0f, 200.0f) * Time.deltaTime, Space.Self);

        // if(isFlying){
        //     transform.Translate(new Vector3(0, 20.0f, -1.0f) * Time.deltaTime, Space.Self);
        // }
        // if(FlyingBool){
        //     transform.Translate(new Vector3(0, -40.0f, 0.0f) * Time.deltaTime, Space.Self);
        // }
        

        //anim.SetBool("isWalk", true);
        //anim.SetBool("isWalk", direction != Vector3.zero);
    }

    //피격
    private void GetHitOut () {
        anim.SetBool("isHit", false);
        anim.SetBool("GetFireHit", false);
        mobSpeed = 5.0f;
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

    //몹 고유 스킬
    private void Skill () {
        if(skillCool >= 6.0f){
            if(_distance >= 30) //플레이어와의 거리에 따라 다른 스킬 구사
                FlyReady();
            else
                Scream();
        }
    }
    //비행Fly
    private void FlyReady () {
        skillCool = 0.0f;
        isFlying = true;
        mobSpeed = 0.1f;
        Invoke("Fly", 1.0f);

        anim.SetTrigger("FlyReady");
        //anim.SetBool("isAttack", true);
    }
    private void Fly () {
        skillCool = 0.0f;
        isFlying = false;
        FlyingBool = true;
        mobSpeed = 100.0f;
        Invoke("FlyOut", 0.5f);
        //rigid.AddForce(Vector3.up * 15.0f, ForceMode.Impulse);

        anim.SetBool("Fly", true);
    }
    private void FlyOut () {
        if(FlyingBool){
            skillCool = 0.0f;
            FlyingBool = false;
            mobSpeed = 0.1f;
            Invoke("Landing", 1.5f);

            anim.SetBool("Landing", true); //착지후 포효
        }
    }
    private void Landing () {
        skillCool = 0.0f;
        Invoke("moveReady", 2.0f);  //착지후 다시 움직이기까지
    }
    private void moveReady () { //스킬 완전 종료
        mobSpeed = 5.0f;
        skillCool = 0.0f;

        anim.SetBool("Fly", false);
    }
    //얼음Ice
    private void Scream () {
        mobSpeed = 0.1f;
        skillCool = 0.0f;
        Invoke("TakeOff", 3.5f);

        anim.SetTrigger("Scream");
    }
    private void TakeOff () {
        Invoke("IceLance", 2.0f);
        skillCool = 0.0f;

        anim.SetTrigger("FlyReady");
    }
    private void IceLance () {
        for(int i = 0; i < 10; i++) {
            float randomFloat = Random.Range(0, 1.0f);
            Invoke("IceLanceRandomAngle", randomFloat);
        }
        skillCool = 0.0f;
        Invoke("IceLanceOut", 2.0f);
    }
    private void IceLanceRandomAngle () { //스킬 완전 종료
        skillCool = 0.0f;
        float randomFloat = Random.Range(-15, 15);
        IceLanceRandomSpawn(randomFloat);
    }
    private void IceLanceRandomSpawn (float angle) { //스킬 완전 종료
        float rdFx = Random.Range(-4, 5);
        float rdFy = Random.Range(-2, 3);
        float rdFz = Random.Range(-4, 5);
        float rdFa = Random.Range(-5, 40);
        Instantiate(iceEffect, this.transform.position + new Vector3(rdFx, rdFy+10, rdFz), this.transform.rotation * Quaternion.Euler(10.0f + rdFa, angle, 0));
    }
    private void IceLanceOut () {
        skillCool = 0.0f;
        Invoke("moveReady", 5.0f);  //위에거 그대로 채용

        anim.SetTrigger("Descent");
    }

    
}
