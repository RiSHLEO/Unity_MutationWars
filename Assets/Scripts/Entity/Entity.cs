using Photon.Pun;
using UnityEngine;

public class Entity : MonoBehaviourPunCallbacks
{
    public StateMachine StateMachine { get; private set; }

    [Header("Components")]
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public Player_SkillManager SkillManager { get; private set; }

    [Header("Input")]
    public PlayerInputSet InputSet { get; private set; }
    public Vector2 MoveInput { get; private set; }

    [Header("States")]
    public Player_IdleState IdleState { get; protected set; }
    public Player_MoveState MoveState { get; protected set; }
    public Player_KnockbackState KnockbackState { get; private set; }

    [Header("Movement")]
    public float MoveSpeed = 8f;

    [Header("Skill Details")]
    [SerializeField] private Skill_Base mainSkill;
    private Skill_Knockback _knockbackSkill;
    private MutationHandler _mutationHandler;

    protected virtual void Awake()
    {
        StateMachine = new StateMachine();
        InputSet = new PlayerInputSet();
        rb = GetComponent<Rigidbody2D>();
        SkillManager = GetComponent<Player_SkillManager>();
        _mutationHandler = GetComponent<MutationHandler>();
        anim = GetComponentInChildren<Animator>();
        _knockbackSkill = GetComponentInChildren<Skill_Knockback>();

        IdleState = new Player_IdleState(this, StateMachine, "idle");
        MoveState = new Player_MoveState(this, StateMachine, "move");
        KnockbackState = new Player_KnockbackState(this, StateMachine, "knockback");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        InputSet.Enable();
        InputSet.Player.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        InputSet.Player.Movement.canceled += ctx => MoveInput = Vector2.zero;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        InputSet.Disable();
    }

    protected virtual void Start()
    {
        if (!photonView.IsMine)
            return;

        SkillUIManager.Instance.SetSkill(mainSkill);
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        if (photonView.IsMine)
            StateMachine.UpdateActiveState();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (photonView.IsMine && _mutationHandler._canMove)
            rb.linearVelocity = new Vector2(xVelocity, yVelocity);
    }

    [PunRPC]
    public void ExecuteKnockbackRPC()
    {
        _knockbackSkill.ExecuteKnockback();
    }

    [PunRPC]
    public virtual void ApplyKnockbackState(float dirX, float dirY, float force, float duration)
    {
        if (KnockbackState is Player_KnockbackState knockbackState)
        {
            Vector2 direction = new Vector2(dirX, dirY).normalized;
            knockbackState.Setup(direction, force, duration);
            StateMachine.ChangeState(knockbackState);
        }
    }
}
