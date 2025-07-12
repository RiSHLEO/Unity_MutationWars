using Photon.Pun;

public class Veil : Entity
{
    public Veil_InvulnerableState InvulState { get; private set; }
    public bool IsInvulnerable { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        InvulState = new Veil_InvulnerableState(this, StateMachine, "invul");
    }

    protected override void Start()
    {
        base.Start();
    }

    [PunRPC]
    public void SetInvulnerability(bool state)
    {
        IsInvulnerable = state;
    }
}
