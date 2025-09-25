using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace WeaponRoaster
{
    public enum RoasterShootType
    {
        rapid,
        heavy,
        single
    }
    /// <summary>
    /// 还在想怎么存档，明天继续改
    /// </summary>

    public class RoasterAttribute : WeaponAttribute
    {
        [SerializeField] private int mpCost = 2;
        [SerializeField] private float breadSpeed = 10f;
        [SerializeField] private float breadDamage = 5f;
        [SerializeField] private float reloadTime = 0.5f;
        [SerializeField] private float MaxLoad = -1;
        [SerializeField] private RoasterShootType shootType = RoasterShootType.single;
        public RoasterAttribute() : base("Roaster", "Ranged", 5f)
        {
            mpCost = 2;
            breadSpeed = 10f;
            breadDamage = 5f;
            reloadTime = 0.5f;
            MaxLoad = -1;
            shootType = RoasterShootType.single;
        }
        public RoasterAttribute(int mpCost, float breadSpeed, float breadDamage, float reloadTime, float maxLoad, RoasterShootType shootType) : base("Roaster", "Ranged", 5f)
        {
            this.mpCost = mpCost;
            this.breadSpeed = breadSpeed;
            this.breadDamage = breadDamage;
            this.reloadTime = reloadTime;
            this.MaxLoad = maxLoad;
            this.shootType = shootType;


        }

        public class Weapon_Roaster : Weapon_Sword
        {
            [SerializeField] private GameObject breadPrefab;
            [SerializeField] private float lastShootTime = -1;
            [SerializeField] private float currentLoad = 0;
            [SerializeField] private bool reloading = false;
            private RoasterAttribute rosterAttribute;


            private void Start()
            {
                weaponCollider = GetComponent<Collider2D>();
                weaponCollider.isTrigger = true; // 确保武器碰撞体是触发器
                rosterAttribute = weaponAttribute as RoasterAttribute;
                switch (rosterAttribute.shootType)
                {
                    case RoasterShootType.rapid:
                        rosterAttribute.mpCost = 2;
                        rosterAttribute.breadDamage = 0.3f * weaponAttribute.attackDamage;
                        rosterAttribute.reloadTime = 0.1f;
                        rosterAttribute.MaxLoad = 1;
                        currentLoad = 1;
                        break;
                    case RoasterShootType.heavy:
                        rosterAttribute.mpCost = 5;
                        rosterAttribute.breadDamage = 1f * weaponAttribute.attackDamage;
                        rosterAttribute.reloadTime = 1f;
                        rosterAttribute.MaxLoad = 2;
                        currentLoad = 2;
                        break;
                    case RoasterShootType.single:
                        rosterAttribute.mpCost = 10;
                        rosterAttribute.breadDamage = 3f * weaponAttribute.attackDamage;
                        rosterAttribute.reloadTime = 1.5f;
                        rosterAttribute.MaxLoad = 1;
                        currentLoad = 1;
                        break;
                    default:
                        break;
                }
                weaponCollider = GetComponent<Collider2D>();
                weaponCollider.isTrigger = true; // 确保武器碰撞体是触发器
            }

            public override void SpecialAttack(Vector2 position)
            {
                if (currentLoad > 0)
                {
                    currentLoad--;
                    ShootBreadRapid(position);
                }
                else if (!reloading)
                {
                    reloading = true;
                    StartCoroutine(Reload());
                }

            }

            public IEnumerator Reload()
            {
                Debug.Log("Reloading...");
                yield return new WaitForSeconds(rosterAttribute.reloadTime);
                currentLoad = rosterAttribute.MaxLoad;
                owner.Mp -= rosterAttribute.mpCost * rosterAttribute.MaxLoad;
                reloading = false;
                Debug.Log("Reloaded.");
            }

            private void ShootBreadRapid(Vector2 direction)
            {
                if (breadPrefab != null)
                {
                    GameObject bread = Instantiate(breadPrefab, transform.position, Quaternion.identity);
                    //owner.Mp -= mpCost;
                    bread.GetComponent<BreadController>().InitBread(direction - (Vector2)owner.transform.position, rosterAttribute.breadSpeed, this, rosterAttribute.breadDamage);
                }
            }
            public override bool CanSpecialAttack()
            {
                return !reloading;
            }
        }
    }
} 
    