using UnityEngine;


public class PlayerInput : MonoBehaviour
{
    public enum PlayerCondition
    {
        Idle,    // 이때만 행동을 할 수 있음!
        Attack,  // 공격
        Guard,   // 방어
        Hitted,  // 피격
        Died     // 죽음
    }

    public PlayerCondition playerCondition;

    public float xMoveDir;
    public float zMoveDir;

    public bool isRight = false;
    public bool isLeft = false;
    public bool isUp = false;
    public bool isDown = false;

    public float rightRun = 0f;
    public float leftRun  = 0f;
    public float upRun    = 0f;
    public float downRun  = 0f;

    void Update()
    {
        MoveInput();
        InitMoveDir();
        CheckRunTimer();
    }

    void CheckRunTimer()
    {
        rightRun -= Time.deltaTime;
        if (rightRun < 0f) isRight = false;

        leftRun -= Time.deltaTime;
        if (leftRun < 0f) isLeft = false;

        upRun -= Time.deltaTime;
        if (upRun < 0f) isUp = false;
        
        downRun -= Time.deltaTime;
        if (downRun < 0f) isDown = false;
    }

    void MoveInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xMoveDir = 1;
            if (isRight && rightRun > 0f) xMoveDir = 2;
            rightRun = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xMoveDir = -1;
            if (isLeft && leftRun > 0f)xMoveDir = -2;
            leftRun = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            zMoveDir = 1;
            if (isUp && upRun > 0f)  zMoveDir = 2;
            upRun = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            zMoveDir = -1;
            if (isDown && downRun > 0f)zMoveDir = -2;
            downRun = 0.5f;
        }
        
        if(Input.GetKey(KeyCode.Z))
        {
            this.gameObject.transform.Rotate(new Vector3(0f, -1f, 0f));
        }
        else if (Input.GetKey(KeyCode.C))
        {
            this.gameObject.transform.Rotate(new Vector3(0f, 1f, 0f));
        }
    }



    void InitMoveDir()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            xMoveDir = 0;
            isRight = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            xMoveDir = 0;
            isLeft = true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            zMoveDir = 0;
            isUp = true;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            zMoveDir = 0;
            isDown = true;
        }
    }



}
