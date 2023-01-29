using UnityEngine;

namespace Player
{
    public class FireModeModifier : MonoBehaviour
    {
        [SerializeField]
        private FireMode newFireMode;
        [SerializeField]
        private bool removeOnContact;

        private void OnTriggerEnter( Collider other )
        {
            if ( !other.tag.Equals("ComponentPlayer") ) return;
            if ( !other.TryGetComponent(out ComponentWeapon weapon) ) return;

            weapon.ChangeFireMode(newFireMode);

            if ( removeOnContact )
                Destroy(gameObject);
        }
    }
}