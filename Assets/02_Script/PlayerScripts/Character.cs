using UnityEngine;

public class Character : MonoBehaviour
{
    // 스탯 콤보, 공격 방식, 이미지 등이 들어갈 예정
    // 추가할 때마다 playerStats 클래스 가서 매개변수에 넣어줘야 함!
    public int hp;
    public int atk;
    public int stamina;

    public void SetStats(Character character)
    {
        this.hp = character.hp;
        this.atk = character.atk;
        this.stamina = character.stamina;

        string check = "" + hp + ", " + atk + ", " + stamina;
        print(check);
    }
}
