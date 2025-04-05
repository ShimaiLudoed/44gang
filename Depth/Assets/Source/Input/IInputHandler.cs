using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputController
{
  public interface IInputHandler
  {
    event Action<Vector3> OnMove;
  } 
}