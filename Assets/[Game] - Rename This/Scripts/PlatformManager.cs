using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private Platform[] platforms;

    void Start()
    {
        platforms = GetComponentsInChildren<Platform>();
        platforms = platforms.OrderByDescending(p => p.transform.position.y).ToArray();
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].PlatformID = i;
        }
        platforms[platforms.Length - 1].isLast = true;
    }
}
