using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject weapon;
    Hammer hammer;
    public float speed = 3f;
    public float jumpPower = 10;
    public int hp = 3;
    public float upperPower;
    public float pressedTime = 1f;

    public GameObject stateTextObj;

    public Image attackGauge;
    PlayerInput playerInput;
    [HideInInspector] public Animator weaponAnim;
    [HideInInspector] public Rigidbody rigid;


    bool canJump = true;
    bool isHitted = false;
    bool canMove = true;

    void Start()
    {
        hammer = weapon.GetComponent<Hammer>();
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
        if (!canMove) return;

        if (this.gameObject != UIManager.instance.createdClient || isHitted) return;
        AttackInput();
        Move();
        Jump();
    }

    public void Hitted()
    {
        if (!canMove) return; // 못움직이는데 처 맞고만 있으면 슬프자나..
        StartCoroutine(CoHitted());
    }

    IEnumerator CoHitted()
    {
        hp--;
        if (hp <= 0)
        {
            //GetComponent<Animator>().Play(""); // 대충 눕혀
            stateTextObj.SetActive(true);
            canMove = false;
            StartCoroutine(WakeUp());
            yield return null;
        }
        else
        {
            isHitted = true;
            yield return new WaitForSeconds(3f);
            isHitted = false;
        }
    }

    IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(5f);
        canMove = true;
        stateTextObj.SetActive(false);
        hp = 2;
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

        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1)) // 나중에 차징공격 만들거니까 UP될때 공격시전해야함.
        {
            Swing();
            Client.instance.AttackSend("Swing");
        }

        if (Input.GetMouseButton(1))
        {
            attackGauge.fillAmount = Mathf.Clamp(pressedTime / 3f, 0, 1f);
            pressedTime += Time.deltaTime;
            upperPower = Mathf.Clamp(pressedTime, 0f, 3f);
        }
        if (Input.GetMouseButtonUp(1))
        {
            UpperSwing();
            Client.instance.AttackSend("UpperSwing");
        }
    }

    public void UpperSwing()
    {
        hammer.state = Hammer.AttackState.upperSwing;
        StartCoroutine(Attack(1, "UpperSwing"));
        
    }

    public void Swing()
    {
        hammer.state = Hammer.AttackState.swing;
        StartCoroutine(Attack(2, "Swing"));
        
    }

    public void PlayAnim(Animator animator,  string animName, int attackType_notBeAbleToAttack = 3)
    {
        animator.Play(animName);
        StartCoroutine(Attack(attackType_notBeAbleToAttack, animName));
    }

    public IEnumerator Attack(int attackType, string animName)
    {
        weaponAnim.SetInteger("AttackType", attackType);
        yield return new WaitForSeconds(1.05f);
        if (attackGauge != null)
            attackGauge.fillAmount = 0;
        weaponAnim.SetInteger("AttackType", 0);
        upperPower = 0f;
        pressedTime = 1f;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            if (UIManager.instance.createdClient == this.gameObject)
            {
                UIManager.instance.OnDeath();
                UIManager.instance.OnQuit();
            }
        }
        canJump = true;
    }
}
