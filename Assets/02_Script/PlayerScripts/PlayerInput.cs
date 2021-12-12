using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float rotateSpeed = 3f;
    
    public Vector3 movedir;
    public Player p;

    private void Start()
    {
        p = GetComponent<Player>();
    }

    void Update()
    {
        MoveInput();
        Rotate();
        InitMoveDir();
    }

    void MoveInput()
    {
        if (Input.GetKey(KeyCode.W)) movedir =  (transform.forward);
        if (Input.GetKey(KeyCode.S)) movedir =  (-transform.forward);
        if (Input.GetKey(KeyCode.A)) movedir =  (-transform.right);
        if (Input.GetKey(KeyCode.D)) movedir =  (transform.right);

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) movedir = (transform.forward +  (-transform.right));
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) movedir = (transform.forward +  ( transform.right));
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) movedir = (-transform.forward + (-transform.right));
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) movedir = (-transform.forward + ( transform.right));
    }

    void InitMoveDir()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyUp(KeyCode.W)) movedir = Vector3.zero;
            if (Input.GetKeyUp(KeyCode.S)) movedir = Vector3.zero;
            if (Input.GetKeyUp(KeyCode.A)) movedir = Vector3.zero;
            if (Input.GetKeyUp(KeyCode.D)) movedir = Vector3.zero;
        }
    }

    void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(0f, Input.GetAxis("Mouse X") * rotateSpeed, 0f, Space.World);
        }
    }
}
