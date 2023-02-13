using Enums;
using Player;
using UnityEngine;

namespace Weapon
{
    public class ProjectileTypeModifier : MonoBehaviour
    {
        [SerializeField]
        private ProjectileType newProjectileType;
        [SerializeField]
        private bool removeOnContact;

        public void SetProjectileType(int _type)
        {
            newProjectileType = (ProjectileType)_type;
            removeOnContact = true;
        }

        private void OnTriggerEnter( Collider other )
        {
            if ( !other.tag.Equals("Player") ) return;
            if ( !other.TryGetComponent(out ComponentWeapon weapon) ) return;

            weapon.ChangeAmmoType(newProjectileType);

            if ( removeOnContact )
                Destroy(gameObject);
        }
    }
}