using Photon.Pun;
using UnityEngine;

public class Skill_Knockback : Skill_Base
{
    [Header("Knockback Details")]
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _force = 10f;
    [SerializeField] private LayerMask _playerLayer;

    private Transform _ownerTransform;
    private PhotonView _photonView;

    protected override void Awake()
    {
        base.Awake();
        _ownerTransform = transform.root;
        _photonView = _ownerTransform.GetComponent<PhotonView>();
    }

    public void TryActivate()
    {
        if (!_photonView.IsMine) return;

        _photonView.RPC(nameof(Entity.ExecuteKnockbackRPC), RpcTarget.All);
        SetSkillCooldown();
    }

    [PunRPC]
    public void ExecuteKnockback()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_ownerTransform.position, _radius, _playerLayer);

        foreach (var collider in colliders)
        {
            if (collider.gameObject == _ownerTransform.gameObject) continue;

            Entity entity = collider.GetComponent<Entity>();
            if (entity != null && entity.photonView != null)
            {
                Vector2 direction = (entity.transform.position - _ownerTransform.position).normalized;
                entity.photonView.RPC(nameof(Stone.ApplyKnockbackState), entity.photonView.Owner, direction.x, direction.y, _force, 0.3f);
            }
        }
        Debug.Log("Knockback skill triggered!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.root.position, _radius);
    }
}
