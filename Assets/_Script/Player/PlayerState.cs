using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player _player;
    protected Renderer _playerRenderer;

    public PlayerState(Player player)
    {
        _player = player;
        _playerRenderer = _player.GetComponent<Renderer>();
    }
    
    public virtual void OnEnterState()
    {
        
    }
    
    public virtual void OnUpdateState()
    {
        
    }
    
    public virtual void OnExitState()
    {
        
    }
}
