using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WouldSystem : MonoBehaviour
{
    public float systemTime;    //인게임 시간
    public int systemDay;
    public bool nightBool;

    public int mobNumber;

    AudioSource audioSource;
    public AudioClip dayBGM;
    public AudioClip nightBGM;
    private bool controlBGM;

    public GameObject sunLight;
    public GameObject prefabPlayer;
    public GameObject thisPlane;

    public GameObject prefabMob0;   // 소환할 Prefab을 저장하는 변수
    public GameObject prefabMob1;
    public GameObject prefabMob2;
    public GameObject prefabMob3;
    public GameObject prefabMob4;
    public GameObject prefabMob5;

    public GameObject prefabMob00;
    public GameObject prefabMob01;
    public GameObject prefabMob02;
    public GameObject prefabMob03;
    public GameObject prefabMob04;
    public GameObject prefabMob05;

    public GameObject prefabBonus1;
    public GameObject prefabBoss01;
    public GameObject prefabExecut;

    public float sunTimeSpeed;

    public int mobCount = 0;
    public int mobMax;      //몹 최대 스폰수
    public bool canSpawn;
    public int spawnIntTimer;
    public float spawnInterval_day0;   // 소환 간격
    public float spawnInterval_day1;
    public float spawnInterval_day2;
    public float spawnInterval_day3;
    public float spawnInterval_night0;
    public float spawnInterval_night1;
    public float spawnInterval_night2;
    public float spawnInterval_night3;
    public float spawnCool_Execut;
    public bool spawn_Execut;

    void Start()
    {
        sunTimeSpeed = 2.0f; //1초당 n도 회전
        systemTime = 0;
        systemDay = 0;

        audioSource = GetComponent<AudioSource>();

        mobMax = 10;    //몹 최대 스폰수
        canSpawn = true;
        //몹 스폰 간격
        spawnInterval_day0 = 5;     //노멀
        spawnInterval_day1 = 4;     //쾌속
        spawnInterval_day2 = 13;    //돌진
        spawnInterval_day3 = 3;     //자폭
        spawnInterval_night0 = 4;   //노멀
        spawnInterval_night1 = 3;   //쾌속
        spawnInterval_night2 = 4;   //돌진
        spawnInterval_night3 = 7;   //자폭
    }

    private void Awake(){
        //Instantiate(prefabBonus1, new Vector3(0, 1.5f, -20), Quaternion.identity);
        //Instantiate(prefabBoss01, new Vector3(0, 1.5f, -20), Quaternion.identity);
        //Instantiate(prefabExecut, new Vector3(0, 1.5f, -20), Quaternion.identity);
        //Instantiate(prefabMob03, new Vector3(0, 1.5f, -20), Quaternion.identity);
    }
    void Update()
    {
        //
    }
    void FixedUpdate() {

        //시간, 날짜
        systemTimeDay();

        // 타이머 갱신
        // spawnTimer_first += Time.deltaTime;
        // if(systemDay >= 1) spawnTimer_second += Time.deltaTime;
        // if(systemDay >= 2) spawnTimer_third += Time.deltaTime;

        //몹스폰
        MobSpawn();

        //BackGroundMusic();
    }




    //BGM
    void BackGroundMusic(){
        if(!nightBool && !controlBGM) {
            controlBGM = true;
            audioSource.Stop();
            audioSource.clip = dayBGM;
            audioSource.Play();
            audioSource.loop = true;
        }
        else if(nightBool && controlBGM) {
            controlBGM = false;
            audioSource.Stop();
            audioSource.clip = nightBGM;
            audioSource.Play();
            audioSource.loop = true;
        }
    }

    //시간과 날짜와 낮밤구현
    private void systemTimeDay () {
        sunLight.transform.Rotate(new Vector3(sunTimeSpeed, 0, 0) * Time.deltaTime);    //지정속도로 회전
        systemTime += Time.deltaTime;
        if(systemTime * sunTimeSpeed >= 160.0f && systemTime * sunTimeSpeed <= 340.0f) nightBool = true;
        else nightBool = false;

        if(systemTime * sunTimeSpeed >= 360) {
            systemDay++;
            systemTime = 0;

            //하루가 끝나면 레벨업
            //prefabPlayer.GetComponent<player>().level += 1;
        }
    }

    //소환함수
    // void SpawnPrefab(GameObject prefab, Vector3 vec)
    // {
    //     Instantiate(prefab, vec, Quaternion.identity);
    // }

    //몹스폰
    void MobSpawn(){
        // 일정 시간마다 mob Prefab 소환
        spawnIntTimer = (int)systemTime + 1;

        float randomHor = Random.Range(-5, 5);
        float randomVer = Random.Range(-5, 5);

        float spawnHor, spawnVer;
        if(randomHor >= 0) spawnHor = randomHor + prefabPlayer.transform.position.x + 20;
        else spawnHor = randomHor + prefabPlayer.transform.position.x - 20;
        if(randomVer >= 0) spawnVer = randomHor + prefabPlayer.transform.position.z + 20;
        else spawnVer = randomVer + prefabPlayer.transform.position.z - 20;

        if(canSpawn){
            if(spawnIntTimer == 1 && !nightBool && systemDay != 0)  Instantiate(prefabBonus1, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
            if(spawnIntTimer == 1 && !nightBool && systemDay == 3)  Instantiate(prefabBoss01, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);

            if(mobCount <= 30){
                if((spawnIntTimer % (spawnInterval_day0 + (systemDay * 3))) == 0 && !nightBool) Instantiate(prefabMob0, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
                if((spawnIntTimer % (spawnInterval_day1 + (systemDay * 3))) == 0 && !nightBool) Instantiate(prefabMob1, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
                if((spawnIntTimer % spawnInterval_day2) == 0 && !nightBool && systemDay >= 1)   Instantiate(prefabMob2, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
                if((spawnIntTimer % spawnInterval_day3) == 0 && !nightBool && systemDay >= 2)   Instantiate(prefabMob3, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
                //if((spawnIntTimer % spawnInterval_day4) == 0 && !nightBool) Instantiate(prefabMob1, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);

                if((spawnIntTimer % (spawnInterval_night0 + (systemDay * 2))) == 0 && nightBool)Instantiate(prefabMob00, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
                if((spawnIntTimer % (spawnInterval_night1 + (systemDay * 2))) == 0 && nightBool)Instantiate(prefabMob01, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
                if((spawnIntTimer % spawnInterval_night2) == 0 && nightBool && systemDay >= 1)  Instantiate(prefabMob02, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
                if((spawnIntTimer % spawnInterval_night3) == 0 && nightBool && systemDay >= 2)  Instantiate(prefabMob03, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
                //if((spawnIntTimer % spawnInterval_night4) == 0 && nightBool) Instantiate(prefabMob01, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
            }
            else if(!spawn_Execut){
                Instantiate(prefabExecut, new Vector3(spawnHor, 1.5f, spawnVer), Quaternion.identity);
                spawn_Execut = true;
            }
            if(spawn_Execut) {
                spawnCool_Execut += Time.deltaTime;
                if(spawnCool_Execut >= 60) spawn_Execut = false;
            }
            canSpawn = false;
            Invoke("MobSpawnOut", 0.99f);
        }
    }
    void MobSpawnOut(){
        canSpawn = true;
    }
}
