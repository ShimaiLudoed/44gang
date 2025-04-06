using InputController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
  public class Controller 
  {
    private readonly IInputHandler _listener;
    private readonly PlayerView _playerView;

    public Controller(IInputHandler listener,PlayerView playerView)
    {
      _playerView = playerView;
      _listener = listener;
    }
    
    public void Bind()
    {
      _listener.OnKill += _playerView.Kill;
      _listener.OnMove += _playerView.Move;
      _listener.OnDash += _playerView.Dash;
    }

    public void Expose()
    {
      _listener.OnKill -= _playerView.Kill;
      _listener.OnMove -= _playerView.Move;
      _listener.OnDash -= _playerView.Dash;
    }
  } 
}
