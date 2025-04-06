using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputController
{
    public class InputListener : MonoBehaviour, IInputHandler
    {
        private PlayerMove _inputActions;
        private Vector3 _currentDirection;
        public event Action<Vector3> OnMove;
        public event Action OnKill;
        public event Action OnDash;
        private Vector2 _lookDelta;

        private void Start()
        {
            _inputActions = new PlayerMove();
            Bind();
        }

        private void OnDisable()
        {
            Expose();
        }

        private void Update()
        {
            Vector3 moveInput = _inputActions.Player.Move.ReadValue<Vector3>();
            _currentDirection = new Vector3(moveInput.x, 0, moveInput.z).normalized;
            OnMove?.Invoke(_currentDirection);
        }
        
        
        private void Bind()
        {
            _inputActions.Player.Kill.performed += OnKillInputPerformed;
            _inputActions.Player.Dash.performed += OnDashInputPerformed;
            _inputActions.Player.Move.canceled += OnMoveInputCanceled;
            _inputActions.Enable();
        }

        private void OnKillInputPerformed(InputAction.CallbackContext context)
        {
            OnKill?.Invoke();
        }
        private void OnMoveInputCanceled(InputAction.CallbackContext context)
        {
            OnMove?.Invoke(Vector3.zero);
            Debug.Log("MoveInputCanceled");
        }
        
        private void OnDashInputPerformed(InputAction.CallbackContext context)
        {
            OnDash?.Invoke();  
            Debug.Log("Dash performed");
        }
        
        private void Expose()
        {
            _inputActions.Disable();
            _inputActions.Player.Move.canceled -= OnMoveInputCanceled;
            _inputActions.Player.Dash.performed -= OnDashInputPerformed;
            _inputActions.Player.Kill.performed -= OnKillInputPerformed;
        }
    }
}
