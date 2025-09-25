using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_BattlePan : Weapon_Sword
{
    private bool isGuarding = false;
    private Quaternion originRotate;

    private void Start()
    {
        weaponCollider = GetComponent<Collider2D>();
        weaponCollider.isTrigger = true; // 确保武器碰撞体是触发器
        #region debug
        WeaponAttribute.modifiers.Add(new M_Berserker());
        #endregion
    }

    public override void SpecialAttack(Vector2 position)
    {
        isGuarding = true;

        BuffManager.instance.AddBuff(owner, new Guarding(-1)); // 添加防御状态的Buff
        originRotate = transform.localRotation;
        transform.localRotation = Quaternion.Euler(0, 0, 45); // 举盾动画
        StartCoroutine(OnGuard());
    }

    

    public IEnumerator OnGuard()
    {
        while (isGuarding)
        {
            if (Input.GetMouseButtonUp(1) || !BuffManager.instance.HasBuff(owner,BuffType.Guarding))
            {
                isGuarding = false;
                OnGuardEnd();
                yield break;
            }
            // 持续防御状态，可以添加防御逻辑
            yield return null;

        }
    }

    public void OnGuardEnd()
    {
        isGuarding = false;
        BuffManager.instance.RemoveBuff(owner, BuffType.Guarding); // 移除防御状态的Buff
        transform.localRotation = originRotate; // 还原武器旋转
    }
}
