using System;
using Enums;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class FireModeUI : MonoBehaviour
    {
        [SerializeField]
        private Image  fireModeIcon;
        [SerializeField]
        private TMP_Text fireModeCountLabel;
        
        private ComponentWeapon _weapon;
        
        /// <summary>
        /// 0 Single
        /// 1 TripleShot
        /// 2 Auto
        /// </summary>
        [SerializeField]
        private Sprite[] fireModeSpriteArray;
        
        private void Start()
        {
            _weapon = FindObjectOfType<ComponentWeapon>();
            if ( !_weapon )
                throw new Exception("Can't find the player weapon component");

            _weapon.OnFireModeChanged += HandleFireModeTypeChange;
            _weapon.OnFireModeRemainingChange += HandleFireModeTypeRemainingChange;
        }

        private void OnDestroy()
        {
            _weapon.OnFireModeChanged -= HandleFireModeTypeChange;
            _weapon.OnFireModeRemainingChange -= HandleFireModeTypeRemainingChange;
        }

        private void HandleFireModeTypeRemainingChange( ushort num ) => fireModeCountLabel.text = $"{num}";

        private void HandleFireModeTypeChange( FireMode fireMode )
        {
            switch ( fireMode )
            {
                case FireMode.Single:
                    fireModeIcon.sprite = fireModeSpriteArray[0];
                break;
                case FireMode.Burst:
                    fireModeIcon.sprite = fireModeSpriteArray[1];
                    break;
                case FireMode.Wave:
                    fireModeIcon.sprite = fireModeSpriteArray[3];
                    break;
                case FireMode.Auto:
                    fireModeIcon.sprite = fireModeSpriteArray[2];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof( fireMode ), fireMode, null);
            }
        }
    }
}