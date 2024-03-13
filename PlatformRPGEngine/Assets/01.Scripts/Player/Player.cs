using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [Header("Movement Settings")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;
    public float dashDuration = 0.4f;
    public float dashSpeed = 20f;

    [Header("Attack Settings")]
    public float attackSpeed = 1f;
    public Vector2[] attackMovement;

    [field: SerializeField] public InputReader PlayerInput { get; private set; }

    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerStat PlayerStat { get; private set; }

    [HideInInspector] public SkillManager Skill { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();
        PlayerStat = CharStat as PlayerStat;

        foreach(PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            //try, catch
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Player{typeName}State");

            PlayerState newState = Activator.CreateInstance(t, StateMachine, this, typeName) as PlayerState;
            StateMachine.AddState(stateEnum, newState);
        }

    }

    private void Start()
    {
        StateMachine.Initalize(PlayerStateEnum.Idle, this);
        Skill = SkillManager.Instance;
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.UpdateState();

        //if(Keyboard.current.pKey.wasPressedThisFrame)
        //{
        //    CharStat.IncreaseStatBy(10, 4f, PlayerStat.GetStatByType(StatType.strength));
        //}
    }

    #region Handling Input
    protected void OnEnable()
    {
        PlayerInput.DashEvent += HandleDashEvent;
        PlayerInput.CrystalEvent += HandleCrystalEvent;
    }

    protected void OnDisable()
    {
        PlayerInput.DashEvent -= HandleDashEvent;
        PlayerInput.CrystalEvent -= HandleCrystalEvent;
    }

    private void HandleCrystalEvent()
    {
        Skill.GetSkill<CrystalSkill>().AtemptUseSkill();
    }

    private void HandleDashEvent()
    {
        if(Skill.GetSkill<DashSkill>().AtemptUseSkill())
        {
            StateMachine.ChangeState(PlayerStateEnum.Dash);
        }
    }

    #endregion

    public void AnimationEndTrigger()
    {
        StateMachine.CurrentState.AnimationEndTrigger();
    }

}
