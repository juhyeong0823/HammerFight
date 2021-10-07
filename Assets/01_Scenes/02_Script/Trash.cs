using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public event Action OnDeath;
    public delegate void OnDeath2();
    OnDeath2 a;

    private void Start()
    {
        a += () =>
        {
            Debug.Log("¤·");
        };
        a();
    }

}
