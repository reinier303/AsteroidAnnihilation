using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [System.Serializable]
    public class Weapon : Equipment, IFire
    {
        public EnumCollections.WeaponTypes WeaponType;
        public EnumCollections.PlayerProjectiles ProjectileType;

        private UIManager uIManager;

        public virtual void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity, Vector2 weaponPosition, int weaponIndex)
        {
            //This method is meant to be overridden.
        }

        public virtual void Fire2nd(ObjectPooler objectPooler, Transform player, Vector2 velocity, Vector2 weaponPosition, int weaponIndex)
        {
            //This method is meant to be overridden.
        }

        protected bool IsCrit()
        {
            if (Random.Range(0f, 100) < playerStats.GetStatValue(EnumCollections.PlayerStats.CritRate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public WeaponData GenerateWeaponData(GeneralItemSettings generalItemSettings)
        {
            WeaponData weaponData = new WeaponData();
            weaponData.EquipmentData = GenerateEquipmentData(generalItemSettings);
            weaponData.WeaponType = WeaponType;
            weaponData.ProjectileType= ProjectileType;
            return weaponData;
        }
    }

    public interface IFire
    {
        //Use Generic T for other optional parameters
        void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity, Vector2 weaponPosition, int weaponIndex);
    }
}
