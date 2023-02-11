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
                    fireModeIcon.color = Color.red;
                break;
                case FireMode.Burst:
                    fireModeIcon.color = Color.green;
                    break;
                case FireMode.Wave:
                    fireModeIcon.color = Color.blue;
                    break;
                case FireMode.Auto:
                    fireModeIcon.color = Color.yellow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof( fireMode ), fireMode, null);
            }
        }
    }
}