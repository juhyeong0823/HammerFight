using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool bTryRun = false;
    public float upRun = 0f;

    public float rotateSpeed = 3f;
    public float runSpeed = 2f;
    private float bRunCheckTimer = 0.3f;

    public GameObject myCam;

    public Vector3 movedir;

    private void Start()
    {
        myCam = GameObject.Find("Camera");
    }

    void Update()
    {
        MoveInput();
        CheckRunTimer();
        Rotate();
        InitMoveDir();
    }

    void CheckRunTimer()
    {
        upRun -= Time.deltaTime;
        if (upRun < 0f) bTryRun = false;
    }

    void MoveInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            movedir = transform.forward;
            //if (bTryRun && upRun > 0f) movedir = Vector3.forward * 2;
            //upRun = bRunCheckTimer;
        }
        if (Input.GetKey(KeyCode.S)) movedir = -transform.forward;
        if (Input.GetKey(KeyCode.A)) movedir = -transform.right;
        if (Input.GetKey(KeyCode.D)) movedir = transform.right;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) movedir = transform.forward + -transform.right;
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) movedir = transform.forward + transform.right;

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) movedir = -transform.forward + -transform.right;
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) movedir = -transform.forward + transform.right;
    }

    void InitMoveDir()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) &&
            !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyUp(KeyCode.W)) movedir = Vector3.zero;
            if (Input.GetKeyUp(KeyCode.S)) movedir = Vector3.zero;
            if (Input.GetKeyUp(KeyCode.A)) movedir = Vector3.zero;
            if (Input.GetKeyUp(KeyCode.D)) movedir = Vector3.zero;
        }
    }

    void Rotate()
    {
        transform.Rotate(0f, Input.GetAxis("Mouse X") * rotateSpeed, 0f, Space.World);
        //transform.Rotate(-Input.GetAxis("Mouse Y") * rotateSpeed, 0f, 0f);
    }
}
