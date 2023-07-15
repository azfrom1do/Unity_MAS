using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attachPlayer : MonoBehaviour
{
    // public GameObject playerPos;

    // private float xRotate, yRotate, xRotateMove, yRotateMove;
    // public float rotateSpeed = 10.0f;

    // void Start()
    // {
    //     Cursor.lockState = CursorLockMode.Locked;       //마우스가 화면내 못나가게 + 숨기기
    //     //Cursor.lockState = CursorLockMode.Confined;     //마우스가 윈도우 밖으로 못나가게
    //     playerPos.GetComponent<player>().rotateSpeed = rotateSpeed;
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    
    // void FixedUpdate()
    // {
    //     //Vector3 pos = playerPos;
    //     attachToPlayer();
    // }

    // void attachToPlayer(){
    //     transform.position = playerPos.transform.position + new Vector3(0, 3.0f, 0);

    //     transform.Rotate(0f, Input.GetAxisRaw("Mouse X") * Time.deltaTime * rotateSpeed, 0f, Space.World);
    //     transform.Rotate(-Input.GetAxisRaw("Mouse Y") * Time.deltaTime * rotateSpeed, 0f, 0f);

    //     /*
    //     if (Input.GetMouseButton(0)) // 클릭한 경우
    //     {
    //         xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;;
    //         yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;;

    //         yRotate = yRotate + yRotateMove;
    //         //xRotate = transform.eulerAngles.x + xRotateMove; 
    //         xRotate = xRotate + xRotateMove;

    //         xRotate = Mathf.Clamp(xRotate, -90, 90); // 위, 아래 고정

    //         //transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

    //         Quaternion quat = Quaternion.Euler(new Vector3(xRotate, yRotate, 0));
    //         transform.rotation 
    //         	= Quaternion.Slerp(transform.rotation, quat, Time.deltaTime );
    //     }*/
    // }
}
