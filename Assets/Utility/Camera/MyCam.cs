using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


[RequireComponent(typeof(Camera))]
//[RequireComponent(typeof(CharacterController))]若需碰撞，则用此模块
public class MyCam : MonoBehaviour
{
    //public Transform target;
    public Vector3 lookPoint = Vector3.zero;
    private float deadZone = 0.01f;
    private Vector3 desEulerAngle;
    private Vector3 curEulerAngle;
    private float currentDistance;
    private float targetDistance;

    //CharacterController controller;

    [Header("方向灵敏度")]
    public float rotateSpeed = 500f;
    public float rotateDamp = 2f;

    [Header("上下最大视角")]
    public float minimumY = -60f;
    public float maximumY = 60f;

    [Header("鼠标缩放限制")]
    public float scrollSpeed = 1000f;
    public float scrollDamp = 5f;
    public float minDistance = 5f;
    public float maxDistance = 50f;

    [Header("围绕目标自动旋转")]
    public bool autoRotate = false;
    public float autoRotateSpeed = 2;


    [Header("鼠标超过多少秒静止，自动旋转(最少5秒)")]
    public bool mouseStopCheck = false;
    public int mouseStopSecond = 5;
    private DateTime timeOld;

    //[Header("移动速度")]
    //[SerializeField]
    //private float moveSpeed = 10f;


    //[Header("移动")]
    //public bool allowMove = false;
    ////W前进
    //[SerializeField]
    //private KeyCode moveForward = KeyCode.W;
    ////S后退
    //[SerializeField]
    //private KeyCode moveBack = KeyCode.S;
    ////A左移
    //[SerializeField]
    //private KeyCode moveLeft = KeyCode.A;
    ////D右移
    //[SerializeField]
    //private KeyCode moveRight = KeyCode.D;
    ////Q键升高
    //[SerializeField]
    //private KeyCode moveUp = KeyCode.Q;
    ////E键降低
    //[SerializeField]
    //private KeyCode moveDown = KeyCode.E;


    // Start is called before the first frame update
    void Start()
    {
        //controller = GetComponent<CharacterController>();
        transform.LookAt(lookPoint, Vector3.up);
        curEulerAngle = desEulerAngle = transform.eulerAngles;
        currentDistance = targetDistance = Vector3.Distance(transform.position, lookPoint);

        timeOld = DateTime.Now;
        if (mouseStopCheck == true)
            InvokeRepeating("CheckMouseStopSecond", 2.0f, 1.0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (allowMove)
        //{
        //    //W前进
        //    if (Input.GetKey(moveForward))
        //    {
        //        Vector3 forward = transform.TransformDirection(Vector3.forward);
        //        //controller.Move(forward*moveSpeed*Time.deltaTime);
        //        transform.Translate(forward * moveSpeed * Time.deltaTime);
        //    }
        //    //S后退
        //    if (Input.GetKey(moveBack))
        //    {
        //        Vector3 back = transform.TransformDirection(Vector3.back);
        //        //controller.Move(back * moveSpeed * Time.deltaTime);
        //        transform.Translate(back * moveSpeed * Time.deltaTime);
        //    }
        //    //A左移
        //    if (Input.GetKey(moveLeft))
        //    {
        //        Vector3 left = transform.TransformDirection(Vector3.left);
        //        //controller.Move(left * moveSpeed * Time.deltaTime);
        //        transform.Translate(left * moveSpeed * Time.deltaTime);
        //    }
        //    //D右移
        //    if (Input.GetKey(moveRight))
        //    {
        //        Vector3 right = transform.TransformDirection(Vector3.right);
        //        //controller.Move(right * moveSpeed * Time.deltaTime);
        //        transform.Translate(right * moveSpeed * Time.deltaTime);
        //    }
        //    //升高
        //    if (Input.GetKey(moveUp))
        //    {
        //        Vector3 up = transform.TransformDirection(Vector3.up);
        //        //controller.Move(up * moveSpeed * Time.deltaTime);
        //        transform.Translate(up * moveSpeed * Time.deltaTime);
        //    }
        //    //降低
        //    if (Input.GetKey(moveDown) && transform.position.y > 0)
        //    {
        //        Vector3 down = transform.TransformDirection(Vector3.down);
        //        //controller.Move(down * moveSpeed * Time.deltaTime);
        //        transform.Translate(down * moveSpeed * Time.deltaTime);
        //    }
        //}

        if (autoRotate)
            Rotate(autoRotateSpeed, 0f); //摄像机围绕目标旋转

#if ENABLE_INPUT_SYSTEM
        // New input system backends are enabled.
        float scroll = Mouse.current.scroll.ReadValue().normalized.y;

        if (scroll < -deadZone || scroll > deadZone)
        {

            targetDistance -= scroll * scrollSpeed * Time.deltaTime;

            autoRotate = false;
            timeOld = DateTime.Now;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue().normalized;
            float x = mouseDelta.x;
            float y = mouseDelta.y;
            Vector3 delta = new Vector3(-y, x, 0) * rotateSpeed * Time.deltaTime;
            desEulerAngle += delta;
            desEulerAngle.x = Mathf.Clamp(desEulerAngle.x, minimumY, maximumY);
            autoRotate = false;
            timeOld = DateTime.Now;
        }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
        // Old input backends are enabled.
        // get Mouse Wheel Input
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue < -deadZone || scrollValue > deadZone)
        {

            targetDistance -= scrollValue * scrollSpeed * Time.deltaTime;

            autoRotate = false;
            timeOld = DateTime.Now;
        }

        if (Input.GetMouseButton(0))
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            Vector3 delta = new Vector3(-y, x, 0) * rotateSpeed * Time.deltaTime;
            desEulerAngle += delta;
            desEulerAngle.x = Mathf.Clamp(desEulerAngle.x, minimumY, maximumY);
            autoRotate = false;
            timeOld = DateTime.Now;
        }
#endif


        curEulerAngle = Vector3.Lerp(curEulerAngle, desEulerAngle, 1 - Mathf.Exp(-rotateDamp * Time.deltaTime));//1-N/e
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, 1f - Mathf.Exp(-scrollDamp * Time.deltaTime));
        transform.eulerAngles = curEulerAngle;
        transform.position = lookPoint - transform.forward * currentDistance;

    }

    public void Rotate(float x, float y)
    {
        desEulerAngle.x -= y * Time.deltaTime;//垂直
        desEulerAngle.y -= x * Time.deltaTime;//水平
    }

    void CheckMouseStopSecond()//检测鼠标静止时长
    {
        TimeSpan ts = DateTime.Now - timeOld;
        if (ts.TotalSeconds > mouseStopSecond && autoRotate == false)
        {
            autoRotate = true;
        }
        else if (ts.TotalSeconds < mouseStopSecond && autoRotate == true)
        {
            autoRotate = false;
        }
    }
}
