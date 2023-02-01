using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDestroyOnTime : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1f);
    }
}
