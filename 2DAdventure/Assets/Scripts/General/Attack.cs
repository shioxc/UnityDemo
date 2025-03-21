using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attack;

    public float attackRange;

    public float attackRate;

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(this);
    }
}
