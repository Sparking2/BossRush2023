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
        
        /// <summary>
        /// 0 bullet
        /// 1 bouncing
        /// 2 mine
        /// </summary>
        [SerializeField]
        private Sprite[] ammoSpriteArray;
        
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
                    ammoTypeIcon.sprite = ammoSpriteArray[0];
                    break;
                case ProjectileType.HighBounce:
                    ammoTypeIcon.sprite = ammoSpriteArray[1];
                    break;
                case ProjectileType.Mine:
                    ammoTypeIcon.sprite = ammoSpriteArray[2];
                    break;
                case ProjectileType.Boss1:
                case ProjectileType.Etc:
                    ammoTypeIcon.sprite = null;
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
