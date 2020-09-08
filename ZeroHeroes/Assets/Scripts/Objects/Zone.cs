using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Zone", menuName = "World/Zone")]
public class Zone : ScriptableObject
{

    [Header("Zone")]
    public string zoneName = "Zone";
    public int sceneId = 1;
    public bool entered = false;

    private void Awake()
    {
        entered = false;
    }
}
