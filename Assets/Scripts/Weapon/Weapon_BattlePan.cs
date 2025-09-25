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
        weaponCollider.isTrigger = true; // ȷ��������ײ���Ǵ�����
        #region debug
        WeaponAttribute.modifiers.Add(new M_Berserker());
        #endregion
    }

    public override void SpecialAttack(Vector2 position)
    {
        isGuarding = true;

        BuffManager.instance.AddBuff(owner, new Guarding(-1)); // ��ӷ���״̬��Buff
        originRotate = transform.localRotation;
        transform.localRotation = Quaternion.Euler(0, 0, 45); // �ٶܶ���
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
            // ��������״̬��������ӷ����߼�
            yield return null;

        }
    }

    public void OnGuardEnd()
    {
        isGuarding = false;
        BuffManager.instance.RemoveBuff(owner, BuffType.Guarding); // �Ƴ�����״̬��Buff
        transform.localRotation = originRotate; // ��ԭ������ת
    }
}
