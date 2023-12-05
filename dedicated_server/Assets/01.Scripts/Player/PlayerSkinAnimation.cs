using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerSkinAnimation : NetworkBehaviour
{
    [SerializeField] private SpriteLibraryAsset[] _spriteAssets;

    private readonly int _isMoveHash = Animator.StringToHash("is_move");
    private readonly int _isJumpHash = Animator.StringToHash("is_jump");
    private readonly int _yInputHash = Animator.StringToHash("y_velocity");

    private SpriteLibrary _spriteLibrary;
    Animator animator;

    public int _currentSpriteIndex = 0;

    private NetworkVariable<bool> _isFlip;
    private NetworkVariable<bool> _isMove;
    private NetworkVariable<bool> _isJump;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _spriteLibrary = GetComponent<SpriteLibrary>();

        _isFlip = new NetworkVariable<bool>();
        _isMove = new NetworkVariable<bool>();
        _isJump = new NetworkVariable<bool>();
    }

    public void SetNextSprite(int num)
    {
        _currentSpriteIndex = num;
        _spriteLibrary.spriteLibraryAsset = _spriteAssets[_currentSpriteIndex];
    }
}
