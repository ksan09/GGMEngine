using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.U2D.Animation;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAnimation : NetworkBehaviour
{
    [SerializeField] private SpriteLibraryAsset[] _spriteAssets;
    private SpriteLibrary _spriteLibrary;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public int _currentSpriteIndex = 0;

    private readonly int _isMoveHash = Animator.StringToHash("is_move");
    private readonly int _isJumpHash = Animator.StringToHash("is_jump");
    private readonly int _yInputHash = Animator.StringToHash("y_velocity");

    private NetworkVariable<bool> _isFlip;
    private NetworkVariable<bool> _isMove;
    private NetworkVariable<bool> _isJump;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteLibrary = GetComponent<SpriteLibrary>();

        _isFlip = new NetworkVariable<bool>();
        _isMove = new NetworkVariable<bool>();
        _isJump = new NetworkVariable<bool>();
    }


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            _isFlip.OnValueChanged += HandleFlipChanged;
            _isMove.OnValueChanged += HandleMoveChanged;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            _isFlip.OnValueChanged -= HandleFlipChanged;
            _isMove.OnValueChanged -= HandleMoveChanged;
        }
    }

    private void HandleFlipChanged(bool previousValue, bool newValue)
    {
        _spriteRenderer.flipX = newValue;
    }

    private void HandleMoveChanged(bool previousValue, bool newValue)
    {
        _animator.SetBool(_isMoveHash, newValue);
    }

    public void SetMove(bool value)
    {
        if (_isMove.Value != value)
        {
            SetIsMoveServerRpc(value);
        }
        _animator.SetBool(_isMoveHash, value);
    }

    public void SetNextSprite(int num)
    {
        _currentSpriteIndex = num;
        _spriteLibrary.spriteLibraryAsset = _spriteAssets[_currentSpriteIndex];
    }

    [ServerRpc]
    private void SetIsMoveServerRpc(bool value)
    {
        _isMove.Value = value;
    }

    [ServerRpc]
    private void SetIsFlipServerRpc(bool value)
    {
        _isFlip.Value = value;
    }

    public void FlipController(float xDirection)
    {
        bool isRightTurn = xDirection > 0 && _spriteRenderer.flipX;
        bool isLeftTurn = xDirection < 0 && !_spriteRenderer.flipX;
        if (isRightTurn || isLeftTurn)
        {
            Flip();
        }
    }

    public void Flip()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
        SetIsFlipServerRpc(_spriteRenderer.flipX);
    }
}