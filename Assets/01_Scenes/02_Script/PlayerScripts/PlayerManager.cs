using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator anim;
    //PlayerStats playerStats;

    [SerializeField] public float speed = 10f;

    PlayerInput playerInput;
    public GameObject weapon;
    void Start()
    {
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        AttackInput(true);
        Move();
    }

    void Move()
    {
        transform.Translate(new Vector3(playerInput.xMoveDir, 0, playerInput.zMoveDir) * Time.deltaTime * speed);
    }

    void AttackInput(bool isIdle) // Idle���°� �ƴϸ� ���� , Ű �Է½� �ִϸ��̼� ����, �ִϸ��̼� ������ enum�� �ٲ���
    {
        if (!isIdle) return;

        

        if (Input.GetKeyDown(KeyCode.A) && !anim.GetBool("IsAttack")) {
            SetState(PlayerInput.PlayerCondition.Attack);
            StartCoroutine(WaitAnimEnd("IsAttack",true));
        }
        else if(Input.GetKeyDown(KeyCode.D)){
        }
    }

    public void SetState(PlayerInput.PlayerCondition condition)
    {
        playerInput.playerCondition = condition;
    }

    IEnumerator WaitAnimEnd(string conditionName, bool animCondition)
    {
        anim.SetBool(conditionName, animCondition);
        yield return new WaitForSeconds(45/60); // �̰Ŵ� 1�ʿ� 60�������� �����ϱ� 60���� ������ ���� �����ָ� ���� �ð��� ����
        anim.SetBool(conditionName, !animCondition);
    }
}
