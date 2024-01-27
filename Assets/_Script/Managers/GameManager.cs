using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int _currentLevel = 1;
    
    protected override void Awake()
    {
        base.Awake();
        // Keep the same GameManager between scenes
        DontDestroyOnLoad(this);
    }

    public void NextLevel()
    {
        _currentLevel++;
        SceneManager.LoadScene($"Level{_currentLevel}");
    }
}
