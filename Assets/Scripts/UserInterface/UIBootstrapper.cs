using System;
using UnityEngine;

namespace UserInterface
{
    public class UIBootstrapper : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabUI;

        private void Awake()
        {
            if ( prefabUI )
                Instantiate(prefabUI);
        }
    }
}