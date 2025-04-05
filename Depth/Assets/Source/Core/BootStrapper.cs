using InputController;
using Player;
using UnityEngine;

namespace Core
{
  public class BootStrapper : MonoBehaviour
  {
    [SerializeField] private PlayerView playerView;
    [SerializeField] private InputListener inputListener;
    private PlayerController _playerController;

    private void Awake()
    {
      Debug.Log("START!");
      _playerController = new (inputListener,playerView);
    }
    private void Start()
    {
      Debug.Log("BIND!");
      _playerController.Bind();
    }
    private void OnDisable()
    {
      _playerController.Expose();
    }
  }
}
