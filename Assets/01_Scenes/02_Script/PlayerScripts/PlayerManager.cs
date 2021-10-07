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

    void AttackInput(bool isIdle) // Idle상태가 아니면 리턴 , 키 입력시 애니메이션 시작, 애니메이션 끝나면 enum을 바꾸자
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
        yield return new WaitForSeconds(45/60); // 이거는 1초에 60프레임을 돌리니까 60분의 프레임 수를 나눠주면 대충 시간이 나옴
        anim.SetBool(conditionName, !animCondition);
    }
}
