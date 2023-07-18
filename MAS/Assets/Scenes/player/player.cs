using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class player : MonoBehaviour
{
    Rigidbody rigid;

    public GameObject planeSpawn;
    public GameObject weapon;
    public Animator anim;
    public Camera mainCamera;
    public TextMeshProUGUI hearthText;
    public TextMeshProUGUI scoreText;
    public GameObject fireBall;
    public GameObject potionReady;
    public GameObject potionHeal;
    public GameObject levelUp;
    public GameObject FB_Icon;

    AudioSource audioSource;
    public AudioClip audioSwing1;
    public AudioClip audioSwing2;
    public AudioClip audioDodge;
    public AudioClip audioHit1;
    public AudioClip audioHit2;
    public AudioClip audioHit3;

    public Vector3 moveDirection;
    Vector3 targetDirection;
    public int level;
    public int maxHealth;
    public int health;
    public float playerSpeed;
    private bool doMove = false;
    public bool canAction;
    private bool canJump;
    public float jumpPower;
    private bool canDodge = true;
    public bool dodgeCool = true;
    private float dodgeSpeed;
    public float dodgeCT;
    private bool canAttack = true;
    public bool immune = false;
    public int potionCount = 3;
    private bool canPotion = true;
    private bool canSkill = true;
    public bool getHit_bossSkill;

    public int score;
    public int killScore;
    public int skillPoint;
    public int AD;
    public int FB_Level;

    public float rotateSpeed;
    public Vector3 cameraRotate;
    private float currentCameraRotationX;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        //감도조정과 마우스 고정
        rotateSpeed = 50.0f;    //감도
        Cursor.lockState = CursorLockMode.Locked;       //마우스가 화면내 못나가게 + 숨기기
        //Cursor.lockState = CursorLockMode.Confined;     //마우스가 윈도우 밖으로 못나가게

        //어째서인지 여기에도 초기화를 해줘야 전달이 됨
        level = 1;
        maxHealth = 3;;
        health = maxHealth;     //체력
        playerSpeed = 10;     //이동속도
        jumpPower = 5;  //점프력

        //회피
        dodgeSpeed = 0; //회피속도
        dodgeCT = 2;    //쿨타임

        //오디오소스 호출
        audioSource = GetComponent<AudioSource>();
    }
    
    void FixedUpdate()
    {
        //점프
        Jump();

        //회피
        Dodge();

        //이동
        RotatePlayer();
        Moving ();
        Walking();
        rotateCamera();

        //평타
        Attack();


        //
        //rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
        //카메라 붙이지 않고는 이렇게 사용 가능

        //UI
        StatusCheck ();
        systemCheck ();

        //스킬
        potion ();
        FireBall();

        //보스전용코드
        GetHit_to_bossSkill();
    }

    private void OnCollisionEnter(Collision col)
    {
        JumpReset (col);    //바닥에 닿으면 다시 점프가 가능
    }
    private void OnCollisionStay(Collision col)
    {
        PlayerHit(col);
    }
    
    








    //스테이터스UI + 체력관리
    private void StatusCheck () {
        //최대체력 설정 (레벨만큼 추가)
        
        hearthText.text = 
            "Level : "+ level + 
            "\nHealth : " + health + 
            "\nPotion (" + potionCount + "/3)";
        if(health <= 0){
            hearthText.text = "GAME OVER";
        }

        //행동 가능 상태
        canAction = (canJump && canDodge  && canAttack && canPotion && canSkill);

        if(killScore >= 10) {
            killScore -= 10;
            score += 10;
            LevelUp ();
        }
        SkillSelect();

        //무기공격력에 레벨만큼 추가
        weapon.GetComponent<weaponAttack>().damage = (AD + 1);
    }
    //시스템UI
    private void systemCheck () {
        score = (level-1) * 10;
        scoreText.text = 
            "Score = " + (score + killScore) + 
            "\nDay = " + (level-1) + 
            "\nEXP = " + killScore;
    }

    //이동
    private void Moving() {
        float speed = playerSpeed * Time.deltaTime + dodgeSpeed;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(h, 0, v).normalized;
        transform.Translate(moveDirection * speed, Space.Self);
        //rigid.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);    //위랑 비슷함

        //transform.LookAt(moveDirection);   //이동 방향 바라보게
    }
    private void Walking () {
        //moveDirection
        //anim.SetBool("Moving", true);
        anim.SetBool("Moving", moveDirection != Vector3.zero);
        if(moveDirection != Vector3.zero) doMove = true;
        else doMove = false;
    }
    private void RotatePlayer(){
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 rotateY = new Vector3(0f, _yRotation, 0f) * rotateSpeed;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(rotateY));



        // // 마우스의 위치를 월드 좌표로 변환
        // Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;
        // if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        // {
        //     // 플레이어의 위치와 마우스의 위치 사이의 벡터를 계산하여 플레이어를 회전시킴
        //     targetDirection = hit.point - transform.position;
        //     targetDirection.y = 0f; // 수평 방향으로만 회전하도록 y 값은 0으로 설정
        //     if (targetDirection != Vector3.zero)
        //     {
        //         Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //         transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        //     }
        // }

        // this.transform.Rotate(0f, Input.GetAxisRaw("Mouse X") * Time.deltaTime * rotateSpeed, 0f, Space.World);
        //transform.Rotate(-Input.GetAxisRaw("Mouse Y") * Time.deltaTime * rotateSpeed, 0f, 0f);
        //cameraRotate
    }
    private void rotateCamera()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float rotateX = _xRotation * rotateSpeed;

        currentCameraRotationX -= rotateX;

        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -45, 45);
        //위아래 볼수있는 최대 각도

        mainCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        // 좌우로 움직이면 안되므로 0f값을 줘서 고정시키기
    }


    
    //피격
    private void PlayerHit (Collision col) {
        if(col.gameObject.tag == "Mob" || col.gameObject.tag == "Mob_Skill"){
            //planeSpawn.GetComponent<spawn>().mobCount--;
            //rigid.AddForce(Vector3.back * 20, ForceMode.Impulse);
            if(!immune){
                health--;
                Immune(1.0f);
                Invoke("PlayerHitOut", 0.3f);

                anim.SetBool("GetHit", true);
                PlaySound("HIT");
            }
        }
    }
    private void PlayerHitOut () {
        anim.SetBool("GetHit", false);
    }
    private void GetHit_to_bossSkill () {
        if(getHit_bossSkill){
            getHit_bossSkill = false;
            if(!immune){
                health--;
                Immune(1.0f);
                Invoke("PlayerHitOut", 0.3f);

                anim.SetBool("GetHit", true);
                PlaySound("HIT");
            }
        }
    }

    //점프
    private void Jump(){
        if(Input.GetKey(KeyCode.Space) && canAction){
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            canJump = false;
        }

        anim.SetBool("Jump", !canJump);
    }
    private void JumpReset (Collision col) {
        if(col.gameObject.tag == "Floor"){
            canJump = true;
        }
    }

    //회피
    private void Dodge(){
        if(Input.GetMouseButton(1) && dodgeCool && canAction){
            dodgeSpeed += 0.5f;
            Debug.Log("회피");
            canDodge = false;
            dodgeCool = false;
            Immune(0.3f);
            Invoke("DodgeOut", 0.2f);
            Invoke("DodgeReady", dodgeCT);

            anim.SetBool("Dodge", true);
            PlaySound("DODGE");
        }
    }
    private void DodgeOut(){
        dodgeSpeed -= 0.5f;
        canDodge = true;
        anim.SetBool("Dodge", false);
    }
    private void DodgeReady(){
        dodgeCool = true;
    }

    //평타
    void Attack(){
        if(Input.GetMouseButton(0) && canAction){
            weapon.GetComponent<weaponAttack>().doAttack = true;
            Debug.Log("평타");
            canAttack = false;
            Invoke("AttackOut", 0.6f);
            Invoke("AttackReady", 0.8f);

            anim.SetBool("Attack", true);
            PlaySound("SWING");
        }
    }
    void AttackOut(){
        weapon.GetComponent<weaponAttack>().doAttack = false;

        anim.SetBool("Attack", false);
    }
    private void AttackReady(){
        canAttack = true;
    }

    //피해면역
    private void Immune (float timer) {
        if(!immune) immune = true;

        Invoke("ImmuneOut", timer);
        //Invoke("ImmuneOut", 0.3f);
    }
    private void ImmuneOut () {
        immune = false;
    }

    //레벨
    private void LevelUp () {
        level++;
        Instantiate(levelUp, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);

        skillPoint++;
    }
    private void SkillSelect () {
        if(skillPoint > 0){
            if(Input.GetKey("1")) {
                skillPoint--;
                maxHealth++;
                health++;
                AD++;
                
                Debug.Log("스텟 증가");
            }
            if(Input.GetKey("2")) {
                skillPoint--;
                FB_Level++;
                
                FB_Icon.SetActive(true);    //스킬 획득시 아이콘 표시
                Debug.Log("화염구 강화");
            }
            if(Input.GetKey("3")) {
                maxHealth++;
                AD++;
                skillPoint--;
                Debug.Log("스텟증가");
            }
        }
    }

    //아이템_포션
    private void potion () {
        if(Input.GetKey("q") && potionCount >= 3 && health < maxHealth && canAction){
            canPotion = false;
            Invoke("potionOut", 1.0f);
            Instantiate(potionReady, this.transform.position + new Vector3(0, 3, 0), this.transform.rotation);

            anim.SetBool("Potion", true);
        }
    }
    private void potionOut () {
        health++;
        potionCount -= 3;
        canPotion = true;
        Instantiate(potionHeal, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);

        anim.SetBool("Potion", false);
    }

    //스킬
    //화염구
    private void FireBall () {
        //moveDirection
        if(Input.GetKey("e") && FB_Level > 0 && canAction){
            canSkill = false;
            Instantiate(fireBall, this.transform.position + new Vector3(0, 3, 0), this.transform.rotation);
            Invoke("FireBallOut", 0.3f);

            anim.SetBool("Shoting", true);
        }
    }
    private void FireBallOut () {
        canSkill = true;

        anim.SetBool("Shoting", false);
    }
    //
    
    
    //효과음
    void PlaySound(string acton){
        int randomInt = Random.Range(1, 4);
        switch (acton) {
            case "SWING":
                if(randomInt == 1) audioSource.clip = audioSwing1;
                if(randomInt >= 2) audioSource.clip = audioSwing2;
                break;
            case "DODGE":
                audioSource.clip = audioDodge;
                break;
            case "HIT":
                if(randomInt == 1) audioSource.clip = audioHit1;
                if(randomInt == 2) audioSource.clip = audioHit2;
                if(randomInt == 3) audioSource.clip = audioHit3;
                break;
            case "FIREBALL":
                audioSource.clip = audioSwing1;
                break;
            default :
                
                break;
        }
        audioSource.Play();
    }
}
