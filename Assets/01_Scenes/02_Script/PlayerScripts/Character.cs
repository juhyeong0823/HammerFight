using UnityEngine;

public class Character : MonoBehaviour
{
    // ���� �޺�, ���� ���, �̹��� ���� �� ����
    // �߰��� ������ playerStats Ŭ���� ���� �Ű������� �־���� ��!
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
