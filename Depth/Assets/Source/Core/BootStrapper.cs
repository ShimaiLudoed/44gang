using InputController;
using Player;
using UnityEngine;

namespace Core
{
  public class BootStrapper : MonoBehaviour
  {
    [SerializeField] private PlayerView playerView;
    [SerializeField] private InputListener inputListener;
    private Controller _controller;

    private void Awake()
    {
      Debug.Log("START!");
      _controller = new (inputListener,playerView);
    }
    private void Start()
    {
      Debug.Log("BIND!");
      _controller.Bind();
    }
    private void OnDisable()
    {
      _controller.Expose();
    }
  }
}
