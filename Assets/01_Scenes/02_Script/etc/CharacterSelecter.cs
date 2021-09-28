using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelecter : MonoBehaviour
{
    public List<Button> charBtns = new List<Button>();

    public GameObject player; // �÷��̾� �ְ� ���⼭ ������ �Ҵ����ٰſ���!

    private void Start()
    {
        charBtns[0].onClick.AddListener(() =>{
            player.GetComponent<Character>().SetStats(charBtns[0].GetComponent<Character>());
        });

        charBtns[1].onClick.AddListener(() =>{
            player.GetComponent<Character>().SetStats(charBtns[1].GetComponent<Character>());
        });

        charBtns[2].onClick.AddListener(() =>{
            player.GetComponent<Character>().SetStats(charBtns[2].GetComponent<Character>());
        });
    }
}
