using InputController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
  public class PlayerController 
  {
    private readonly IInputHandler _listener;
    private readonly PlayerView _playerView;

    public PlayerController(IInputHandler listener,PlayerView playerView)
    {
      _playerView = playerView;
      _listener = listener;
    }
    
    public void Bind() => _listener.OnMove += _playerView.Move;
    public void Expose  () => _listener.OnMove -= _playerView.Move;
  } 
}
