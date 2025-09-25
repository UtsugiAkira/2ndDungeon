using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class BreadController : MonoBehaviour
{
    private Weapon ownerWeapon;
    private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out _GameEntity entity))
        {
            if(entity != ownerWeapon.owner) // ȷ�����ǹ������Լ�
            {
                ownerWeapon.HandleHit(entity,damage);
                Destroy(gameObject); // ײ�����������
            }
        }
    }

    public void InitBread(Vector2 dircetion,float speed,Weapon owner,float damage)
    {
        Destroy(gameObject, 3f);
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = dircetion.normalized * speed;
        ownerWeapon = owner;
        this.damage = damage;
    }

}
