using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Berserker : WeaponModifier
{
    float effictIntensity;
    float conditionThreshold;
    bool isActive = false;

    public M_Berserker()
    {
        //this.triggerType = EntityEventType.OnHealthChanged;
        conditionThreshold = Random.Range(0.1f, 0.4f);
        effictIntensity = 1 + (1 - conditionThreshold);
        condition = new Condition_Hp();
    }

    public override void Init(Weapon weapon)
    {
        this.weapon = weapon;
        ApplyAttribute(weapon.owner);
        //Debug.Log($"Berserker Modifier Applied: Condition Threshold = {conditionThreshold}, Effect Intensity = {effictIntensity}");
    }

/*    public override bool Condition()
    {
        //Debug.Log($"Berserker Condition Check: Current Health Ratio = {weapon.owner.Health / weapon.owner.GetComponent<_GameEntity>().MaxHealth}, Threshold = {conditionThreshold}");
        if ((weapon.owner.Health /  weapon.owner.GetComponent<_GameEntity>().MaxHealth) < conditionThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }*/

    public override void Effect()
    {
        //Debug.Log("Berserker Effect Triggered!");
        if (!isActive)
        {
            weapon.owner.AttackRate *= effictIntensity;
            weapon.owner.Speed *= effictIntensity;
            weapon.owner.AttackSpeedRate *= effictIntensity;
            weapon.owner.GetComponent<SpriteRenderer>().color = Color.red;
            isActive = true;
        }
    }

    public void DisApplyEffect()
    {
        weapon.owner.AttackRate /= effictIntensity;
        weapon.owner.Speed /= effictIntensity;
        weapon.owner.AttackSpeedRate /= effictIntensity;
        weapon.owner.GetComponent<SpriteRenderer>().color = Color.white;
        isActive = false;
    }
}
