using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    [SerializeField] GameObject _visualFingerPosition;

    PlayerInput _playerInput;
    Vector2 _tapPosition;
    bool _fingerReleased = false;

    public bool FingerReleased{ get {return _fingerReleased;} set { _fingerReleased = value; }}
    public Vector2 TapPosition{ get { return _tapPosition; } private set { _tapPosition = value; }}


    void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Touch.TouchPosition.performed += OnTapInput;
        _playerInput.Touch.TouchPosition.started += OnTapInput;
        _playerInput.Touch.TouchPress.canceled += OnTapCanceled;
    }

    void OnTapInput(InputAction.CallbackContext ctx)
    {
        _tapPosition = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        _visualFingerPosition.transform.position = _tapPosition;
        //Debug.Log(_tapPosition);
    }

    void OnTapCanceled(InputAction.CallbackContext ctx)
    {
        _fingerReleased = true;                                         // boolean value for the ballshooter to know when to shoot
        //Debug.Log("finger released!");
    }

    void OnEnable()
    {
        _playerInput.Touch.Enable();
    }

    void OnDisable()
    {
        _playerInput.Touch.Disable();
    }
}
