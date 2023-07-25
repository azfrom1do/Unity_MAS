using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item_EXPpotion{
    private string name;
    private int number;

    public Item_EXPpotion(){
        name = "";
        number = 0;
    }
    public Item_EXPpotion(string str, int num){
        name = str;
        number = num;
    }

    public string getName(){
        return name;
    }
}
public class Item_List{
    public Item_EXPpotion[] list = new Item_EXPpotion[5];

    public Item_List(){
        list[0] = new Item_EXPpotion("테스트아이템", 0);
        list[1] = new Item_EXPpotion("경험치포션", 1);
        list[2] = new Item_EXPpotion("생고기", 2);
        list[3] = new Item_EXPpotion("녹슨갑옷", 3);
        list[4] = new Item_EXPpotion("조제도구", 4);
        list[5] = new Item_EXPpotion("은화", 5);
    }

    public string getItemName(int n){
        return list[n].getName();
    }
}

public class player : MonoBehaviour
{
    Rigidbody rigid;

    public GameObject planeSpawn;
    public GameObject weapon;
    public Animator anim;
    public Camera mainCamera;
    public Image healthImag;
    public Image expImag;
    public TextMeshProUGUI hearthText;
    public TextMeshProUGUI statText;
    public TextMeshProUGUI levelText;
    public GameObject fireBall;
    public GameObject potionReady;
    public GameObject potionHeal;
    public GameObject levelUp;
    public GameObject select1_Icon;
    public GameObject select2_Icon;
    public GameObject select3_Icon;
    public GameObject FB_Icon;
    public Image FB_Imag;
    public GameObject stat1_Icon;
    public GameObject stat2_Icon;
    public GameObject stat3_Icon;
    public GameObject statUp;
    public GameObject[] item_GOlist;
    public GameObject item_EXPpotion_Icon;      //아이콘 경험치포션
    public GameObject item_RawMeat_Icon;        //아이콘 생고기
    public GameObject item_RustyArmor_Icon;     //아이콘 녹슨갑옷
    public GameObject item_PotionTool_Icon;     //아이콘 생고기
    public GameObject item_InvisiCloak_Icon;    //아이콘 생고기

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
    public bool doMove;
    
    public bool canAction;
    private bool canJump;
    public float jumpPower;
    private bool canDodge = true;
    public bool dodgeReady = true;
    private float dodgeSpeed;
    public float dodgeCT;
    private bool canAttack = true;
    public bool immune = false;
    public float getHit_Immune;
    public int needPotion;
    public int potionCount;
    private bool canPotion = true;
    private bool canSkill = true;
    public bool getHit_bossSkill;

    public int score;
    public int killScore;
    private bool canSelect;
    public int itemPoint;
    public int[] item_Array;
    public int skillPoint;
    public int AD;
    public int FB_Level;
    public float FB_CT;
    public bool FB_Ready = true;

    public float rotateSpeed;
    public Vector3 cameraRotate;
    private float currentCameraRotationX;
    public bool wallBack;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        //감도조정과 마우스 고정
        rotateSpeed = 30.0f;    //감도
        Cursor.lockState = CursorLockMode.Locked;       //마우스가 화면내 못나가게 + 숨기기
        //Cursor.lockState = CursorLockMode.Confined;     //마우스가 윈도우 밖으로 못나가게

        //어째서인지 여기에도 초기화를 해줘야 전달이 됨
        level = 1;
        maxHealth = 3;;
        health = maxHealth;     //체력
        playerSpeed = 10;     //이동속도
        jumpPower = 5;  //점프력
        FB_CT = 15.0f;    //염구쿨

        //회피
        dodgeSpeed = 0; //회피속도
        dodgeCT = 2;    //쿨타임

        //오디오소스 호출
        audioSource = GetComponent<AudioSource>();

        //아이템
        item_Array = new int[3];
        item_GOlist = new GameObject[]{
            item_EXPpotion_Icon,
            item_RawMeat_Icon,
            item_RustyArmor_Icon,
            item_PotionTool_Icon,
            item_InvisiCloak_Icon
        };
        potionCount = 5;
        needPotion = 5;
        getHit_Immune = 1.0f;
        canSelect = true;
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

        //보스전용코드
        GetHit_to_bossSkill();
    }

    private void OnCollisionEnter(Collision col)
    {
        JumpReset (col);    //바닥에 닿으면 다시 점프가 가능
        WallBack(col);
    }
    private void OnCollisionStay(Collision col)
    {
        PlayerHit(col);
    }

    void Update(){
        //스킬
        potion ();      //포션
        FireBall();     //화염구

        //선택
        SkillSelect();  //스킬선택
        ItemSelect ();  //유물선택
    }
    
    








    //스테이터스UI + 체력관리
    private void StatusCheck () {
        //최대체력 설정
        
        //체력이미지
        healthImag.fillAmount = (float)health / (float)maxHealth;

        hearthText.text = 
            health + "/" + maxHealth;
        statText.text = 
            "LV : "+ level + 
            "\nAD: " + (1 + AD) + 
            "\n " + potionCount + " / " + needPotion;

        if(health <= 0){
            hearthText.text = "GAME OVER";
        }

        //행동 가능 상태
        canAction = (canJump && canDodge  && canAttack && canPotion && canSkill);

        if(killScore >= 15 + (level * 5)) {
            killScore -= (15+ (level * 5));
            score += 10;
            LevelUp ();
        }

        //무기공격력에 레벨만큼 추가
        weapon.GetComponent<weaponAttack>().damage = (AD + 1);
    }
    //시스템UI
    private void systemCheck () {
        score = (level-1) * 10;
        levelText.text = 
            // "Score = " + (score + killScore) + 
            killScore + "/" + (15 + (level * 5));

        //경험치이미지
        expImag.fillAmount = (float)killScore / (float)(15 + (level * 5));
    }

    //이동
    private void Moving() {
        float speed = playerSpeed * Time.deltaTime + dodgeSpeed;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(h, 0, v).normalized;
        if(!wallBack) transform.Translate(moveDirection * speed, Space.Self);
        else transform.Translate(moveDirection * speed * (-1), Space.Self);
        //rigid.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);    //위랑 비슷함

        //transform.LookAt(moveDirection);   //이동 방향 바라보게

        wallBack = false;
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
    private void WallBack (Collision col) {
        if(col.gameObject.tag == "Wall") wallBack = true;
        else wallBack = false;
    }
    
    //피격
    private void PlayerHit (Collision col) {
        if(col.gameObject.tag == "Mob" || col.gameObject.tag == "Mob_Skill"){
            //planeSpawn.GetComponent<spawn>().mobCount--;
            //rigid.AddForce(Vector3.back * 20, ForceMode.Impulse);
            if(!immune){
                health--;
                if(col.gameObject.tag == "Mob_Skill") health--; 
                Immune(getHit_Immune);
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
        if(Input.GetMouseButton(1) && dodgeReady && canAction)
            StartCoroutine(DodgeCRT());
    }
    IEnumerator DodgeCRT(){
        yield return null;
        dodgeSpeed += 0.5f;
        Debug.Log("회피");
        canDodge = false;
        dodgeReady = false;
        anim.SetBool("Dodge", true);
        PlaySound("DODGE");
        Immune(0.3f);

        yield return new WaitForSeconds(0.2f);
        dodgeSpeed -= 0.5f;
        canDodge = true;
        anim.SetBool("Dodge", false);

        yield return new WaitForSeconds(dodgeCT);
        dodgeReady = true;
    }

    //평타
    void Attack(){
        if(Input.GetMouseButton(0) && canAction && canAttack)
            StartCoroutine(AttackCRT());
    }
    IEnumerator AttackCRT(){
        yield return null;
        canAttack = false;
        weapon.GetComponent<weaponAttack>().doAttack = true;
        Debug.Log("평타");
        anim.SetBool("Attack", true);
        PlaySound("SWING");

        yield return new WaitForSeconds(0.6f);
        weapon.GetComponent<weaponAttack>().doAttack = false;

        yield return new WaitForSeconds(0.2f);
        canAttack = true;
        anim.SetBool("Attack", false);
    }

    //피해면역
    private void Immune (float timer) {
        if(!immune) {
            immune = true;
            Invoke("ImmuneOut", timer);
        }
    }
    private void ImmuneOut () {
        immune = false;
    }

    //레벨
    public void LevelUp () {
        level++;
        Instantiate(levelUp, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);

        skillPoint++;
    }
    IEnumerator SelectBoolCRT(){    //선택했다는 효과 코루틴
        while (!Input.GetKeyDown("1") && !Input.GetKeyDown("2") && !Input.GetKeyDown("3"))
            yield return null;

        if(Input.GetKeyDown("1"))
            select1_Icon.SetActive(true);
        if(Input.GetKeyDown("2"))
            select2_Icon.SetActive(true);
        if(Input.GetKeyDown("3"))
            select3_Icon.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        select1_Icon.SetActive(false);
        select2_Icon.SetActive(false);
        select3_Icon.SetActive(false);
    }

    private void SkillSelect () {   //스킬 선택
        if(skillPoint > 0 && canSelect){
            StartCoroutine(SkillCRT());
            StartCoroutine(SelectBoolCRT());    //선택 이펙트 코루틴
        }
    }
    IEnumerator SkillCRT(){         //스킬 코루틴
        canSelect = false;
        stat1_Icon.SetActive(true);
        stat2_Icon.SetActive(true);
        stat3_Icon.SetActive(true);

        // while (!Input.anyKeyDown)
        //     yield return null;  //아무키 입력될때까지 대기

        while (!Input.GetKeyDown("1") && !Input.GetKeyDown("2") && !Input.GetKeyDown("3"))
            yield return null;

        if(Input.GetKeyDown("1")) {
            skillPoint--;
            maxHealth++;
            health++;
            AD++;
            Instantiate(statUp, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);
            
            stat2_Icon.SetActive(false);
            stat3_Icon.SetActive(false);
            Debug.Log("스텟 증가");
        }
        if(Input.GetKeyDown("2")) {
            skillPoint--;
            FB_Level++;
            
            FB_Icon.SetActive(true);    //스킬 획득시 아이콘 표시
            stat1_Icon.SetActive(false);
            stat3_Icon.SetActive(false);
            Debug.Log("화염구 강화");
        }
        if(Input.GetKeyDown("3")) {
            skillPoint--;

            stat1_Icon.SetActive(false);
            stat2_Icon.SetActive(false);
            Debug.Log("보상미정");
        }

        yield return new WaitForSeconds(1.0f);
        stat1_Icon.SetActive(false);
        stat2_Icon.SetActive(false);
        stat3_Icon.SetActive(false);
        canSelect = true;
    }

    //아이템
    private void ItemList (int num) {
        Debug.Log(num+"호출");
        switch (num) {
            case 0:
                //경험치포션
                //즉시 레벨업 2번
                LevelUp();
                Invoke("LevelUp", 0.3f);
                
                Debug.Log("경험치포션 선택");
                item_GOlist[1].SetActive(false);
                item_GOlist[2].SetActive(false);
                item_GOlist[3].SetActive(false);
                item_GOlist[4].SetActive(false);
                break;
            case 1:
                //생고기
                //최대체력+2
                //풀힐
                maxHealth += 2;
                health = maxHealth;

                Debug.Log("생고기 선택");
                item_GOlist[0].SetActive(false);
                item_GOlist[2].SetActive(false);
                item_GOlist[3].SetActive(false);
                item_GOlist[4].SetActive(false);
                break;
            case 2:
                //녹슨갑옷
                //최대체력+5
                //10초 무적
                maxHealth += 5;
                Immune(10.0f);

                Debug.Log("녹슨갑옷 선택");
                item_GOlist[0].SetActive(false);
                item_GOlist[1].SetActive(false);
                item_GOlist[3].SetActive(false);
                item_GOlist[4].SetActive(false);
                break;
            case 3:
                //조제도구
                //포션획득 필요수-1
                if(needPotion > 1) needPotion -= 1;

                Debug.Log("조제도구 선택");
                item_GOlist[0].SetActive(false);
                item_GOlist[1].SetActive(false);
                item_GOlist[2].SetActive(false);
                item_GOlist[4].SetActive(false);
                break;
            case 4:
                //투명망토 
                //피격후 1초간 무적
                getHit_Immune += 1.0f;

                Debug.Log("투명망토  선택");
                item_GOlist[0].SetActive(false);
                item_GOlist[1].SetActive(false);
                item_GOlist[2].SetActive(false);
                item_GOlist[3].SetActive(false);
                break;
            default :
                break;
        }
        itemPoint--;
    }
    private void ItemListOut () {
        for(int i = 0; i < 5; i++){
            item_GOlist[i].SetActive(false);
            item_GOlist[i].transform.position = new Vector3(stat1_Icon.transform.position.x, stat1_Icon.transform.position.y);
        }

        // item_GOlist[item_Array[1]].transform.position = 
        //     new Vector3(item_GOlist[item_Array[1]].transform.position.x -100, item_GOlist[item_Array[1]].transform.position.y);
        // item_GOlist[item_Array[2]].transform.position = 
        //     new Vector3(item_GOlist[item_Array[2]].transform.position.x -200, item_GOlist[item_Array[2]].transform.position.y);
    }



    private void ItemSelect() {
        if (itemPoint > 0 && skillPoint == 0 && canSelect)
            StartCoroutine(ItemCRT());
    }
    IEnumerator ItemCRT(){
        canSelect = false;
        List<int> indexList = new List<int>() { 0, 1, 2, 3, 4 }; // 아이템 인덱스 후보 리스트
        for (int i = 0; i < 3; i++){
            int randomIndex = Random.Range(0, indexList.Count); // 후보 리스트에서 무작위 인덱스 선택
            item_Array[i] = indexList[randomIndex]; // 선택된 인덱스를 결과 배열에 할당
            indexList.RemoveAt(randomIndex); // 이미 선택된 인덱스는 후보 리스트에서 제거

            Debug.Log(item_Array[i]);

            if(i == 0) item_GOlist[item_Array[i]].transform.position = new Vector3(stat1_Icon.transform.position.x, stat1_Icon.transform.position.y);
            if(i == 1) item_GOlist[item_Array[i]].transform.position = new Vector3(stat2_Icon.transform.position.x, stat2_Icon.transform.position.y);
            if(i == 2) item_GOlist[item_Array[i]].transform.position = new Vector3(stat3_Icon.transform.position.x, stat3_Icon.transform.position.y);
            item_GOlist[item_Array[i]].SetActive(true);
        }

        while (!Input.GetKeyDown("1") && !Input.GetKeyDown("2") && !Input.GetKeyDown("3"))
            yield return null;

        if(Input.GetKeyDown("1")) ItemList(item_Array[0]);
        if(Input.GetKeyDown("2")) ItemList(item_Array[1]);
        if(Input.GetKeyDown("3")) ItemList(item_Array[2]);

        yield return new WaitForSeconds(1.0f);
        ItemListOut();
        canSelect = true;
    }
    private void potion () {
        if(Input.GetKeyDown("q") && potionCount >= needPotion && health < maxHealth && canAction){
            potionCount -= needPotion;
            canPotion = false;
            Invoke("potionOut", 1.0f);
            Instantiate(potionReady, this.transform.position + new Vector3(0, 3, 0), this.transform.rotation);

            anim.SetBool("Potion", true);
        }
    }
    private void potionOut () {
        health++;
        canPotion = true;
        Instantiate(potionHeal, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);

        anim.SetBool("Potion", false);
    }

    //스킬
    //쿨타임
    IEnumerator CoolTimeCRT(Image img, float cool)
    {
        print("쿨타임 코루틴 실행");

        float tempTime=0;
 
        while (tempTime <= cool) {
            tempTime += Time.deltaTime;
            img.fillAmount = tempTime/cool;
            yield return new WaitForFixedUpdate();
        }
        
        print("쿨타임 코루틴 완료");
    }
    //화염구
    private void FireBall () {
        if(Input.GetKeyDown("e") && FB_Level > 0 && FB_Ready && canAction)
            StartCoroutine (FireBallCRT());
    }
    IEnumerator FireBallCRT() {
        canSkill = false;
        FB_Ready = false;
        Instantiate(fireBall, this.transform.position + new Vector3(0, 3, 0), this.transform.rotation);
        StartCoroutine (CoolTimeCRT(FB_Imag, FB_CT));
        anim.SetBool("Shoting", true);

        yield return new WaitForSeconds(0.3f);
        canSkill = true;
        anim.SetBool("Shoting", false);

        yield return new WaitForSeconds(FB_CT);
        FB_Ready = true;
    }
    
    //효과음
    void PlaySound(string action){
        int randomInt = Random.Range(1, 4);
        switch (action) {
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
