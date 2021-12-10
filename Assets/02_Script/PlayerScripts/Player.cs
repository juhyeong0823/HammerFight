using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public GameObject weapon;

    PlayerInput playerInput;
    [HideInInspector]
    public Animator weaponAnim;
    Rigidbody rigid;


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

    void Move()
    {
        if (isHitted) return;

        //rigid.velocity = new Vector3(playerInput.moveX, rigid.velocity.y , playerInput.moveZ) * speed;
        rigid.velocity = playerInput.movedir * speed;
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
    
}
