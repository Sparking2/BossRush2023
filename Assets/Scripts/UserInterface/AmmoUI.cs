using System;
using Enums;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField]
        private Image ammoTypeIcon;
        [SerializeField]
        private TMP_Text ammoCountLabel;

        private ComponentWeapon _weapon;
        
        private void Start()
        {
            _weapon = FindObjectOfType<ComponentWeapon>();
            if ( !_weapon )
                throw new Exception("Can't find the player weapon component");

            _weapon.OnAmmoTypeChanged += HandleAmmoTypeChange;
            _weapon.OnAmmoRemainingChange += HandleAmmoTypeRemainingChange;
        }

        private void OnDestroy()
        {
            _weapon.OnAmmoTypeChanged -= HandleAmmoTypeChange;
            _weapon.OnAmmoRemainingChange -= HandleAmmoTypeRemainingChange;
        }

        private void HandleAmmoTypeChange( ProjectileType type )
        {
            switch ( type )
            {
                case ProjectileType.Bullet:
                    ammoTypeIcon.color = Color.yellow;
                    break;
                case ProjectileType.HighBounce:
                    ammoTypeIcon.color = Color.green;
                    break;
                case ProjectileType.Mine:
                    ammoTypeIcon.color = Color.blue;
                    break;
                case ProjectileType.Boss1:
                case ProjectileType.Etc:
                    ammoTypeIcon.color = Color.gray;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof( type ), type, null);
            }
        }

        private void HandleAmmoTypeRemainingChange( ushort amount )
        {
            ammoCountLabel.text = $"{amount}";
        }
    }
}
