using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public GameObject weapon;

    PlayerInput playerInput;
    [HideInInspector]public Animator weaponAnim;
    [HideInInspector]public Rigidbody rigid;
    public float jumpPower = 10;

    bool canJump = true;
    bool isHitted = false;

    void Start()
    {
        if (this.gameObject == UIManager.instance.createdClient)
        {
            rigid = GetComponent<Rigidbody>();
            weaponAnim = weapon.GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();
        }
        else
        {
            rigid = GetComponent<Rigidbody>();
            weaponAnim = weapon.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (this.gameObject != UIManager.instance.createdClient || isHitted) return;
        AttackInput();
        Move();
        Jump();
    }

    public void Hitted()
    {
        StartCoroutine(CoHitted());
    }

    IEnumerator CoHitted()
    {
        isHitted = true;
        yield return new WaitForSeconds(3f);
        isHitted = false;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rigid.velocity += new Vector3(0, jumpPower, 0);
            canJump = false;
        }
    }

    void Move()
    {
        if (isHitted) return;
        rigid.velocity = new Vector3(playerInput.movedir.x * speed, rigid.velocity.y, playerInput.movedir.z * speed);
    }

    void AttackInput()
    {
        if (weaponAnim.GetInteger("AttackType") != 0) return; // 0이 Idle

        if (Input.GetMouseButtonUp(0)) // 나중에 차징공격 만들거니까 UP될때 공격시전해야함.
        {
            UpperSwing();
            Client.instance.AttackSend("UpperSwing");
        }
    }

    public void UpperSwing() => StartCoroutine(Attack(1, "UpperSwing"));


    public IEnumerator Attack(int attackType, string animName)
    {
        weaponAnim.SetInteger("AttackType", attackType);
        Debug.Log(GameManager.instance.GetAnimLength(weaponAnim, animName));
        yield return new WaitForSeconds(GameManager.instance.GetAnimLength(weaponAnim, animName));
        weaponAnim.SetInteger("AttackType", 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }
}
