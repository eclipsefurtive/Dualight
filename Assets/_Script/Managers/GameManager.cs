using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        // Keep the same GameManager between scenes
        DontDestroyOnLoad(this);
    }
}
