using Photon.Pun;

public class Veil_InvulnerableState : EntityState
{
    private Veil _veil;
    private PhotonView _photonView;
    private Skill_Invulnerability _invulSkill;

    public Veil_InvulnerableState(Entity entity, StateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        _veil = _entity.GetComponent<Veil>();
        _photonView = _entity.GetComponent<PhotonView>();
    }

    public override void Enter()
    {
        base.Enter();

        if (_invulSkill == null)
            _invulSkill = _skillManager?.Invul;

        _veil.photonView.RPC(nameof(_veil.SetInvulnerability), RpcTarget.All, true);
        _stateTimer = _invulSkill.Duration;
    }

    public override void Update()
    {
        base.Update();

        if (_stateTimer < 0)
            _stateMachine.ChangeState(_entity.IdleState);

        _entity.SetVelocity(_entity.MoveInput.x * _entity.MoveSpeed, _entity.MoveInput.y * _entity.MoveSpeed);
    }

    public override void Exit()
    {
        base.Exit();
        _skillManager.Invul.SetSkillCooldown();
        _photonView.RPC(nameof(_veil.SetInvulnerability), RpcTarget.All, false);
    }
}
