using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class TrainingAgent : Agent
{
    [SerializeField] private Transform _goal;
    [SerializeField] private SpriteRenderer _groundRenderer;
    [SerializeField] private float _moveSpeed = 6f;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    [HideInInspector] public int CurrentEpisode = 0;
    [HideInInspector] public float CummulativeReward = 0f;

    private Color _defaultGroundColor;
    private Coroutine _flashGroundCoroutine;
    private float _distanceOld;

    public override void Initialize()
    {
        base.Initialize();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        CurrentEpisode = 0;
        CummulativeReward = 0f;
    }

    public override void OnEpisodeBegin()
    {

        if (_groundRenderer != null && CummulativeReward != 0f)
        {
            Color flashColor = (CummulativeReward > 0f) ? Color.green : Color.red;

            if (_flashGroundCoroutine != null)
            {
                StopCoroutine(_flashGroundCoroutine);
            }

            _flashGroundCoroutine = StartCoroutine(FlashGround(flashColor, 3.0f));
        }

        _rb.linearVelocity = Vector2.zero;
        CurrentEpisode++;
        CummulativeReward = 0f;

        SpawnObjects();
        _distanceOld = Vector2.Distance(transform.localPosition, _goal.localPosition);
    }

    private IEnumerator FlashGround(Color targetColor, float duration)
    {
        float elapsedTime = 0f;

        _groundRenderer.color = targetColor;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _groundRenderer.color = Color.Lerp(targetColor, _defaultGroundColor, elapsedTime / duration);
            yield return null;
        }
    }

    private void SpawnObjects()
    {
        transform.localRotation = Quaternion.identity;

        Vector3 agentPos = Vector3.zero;
        int attempts = 0;
        do
        {
            agentPos = new Vector3(Random.Range(-23f, 20f), Random.Range(-9f, 11f), 0f);
            attempts++;
            if (attempts > 100)
            {
                Debug.LogWarning("Failed to find valid agent spawn position after 100 attempts.");
                break;
            }
        } while (!IsPositionValid(agentPos, 0.5f));

        transform.localPosition = agentPos;

        Vector3 goalPos = Vector3.zero;
        attempts = 0;
        do
        {
            goalPos = new Vector3(Random.Range(-23f, 20f), Random.Range(-9f, 11f), 0f);
            attempts++;
            if (attempts > 100)
            {
                Debug.LogWarning("Failed to find valid goal spawn position after 100 attempts.");
                break;
            }
        } while (!IsPositionValid(goalPos, 0.5f));

        _goal.localPosition = goalPos;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float goalPosX_normalized = _goal.localPosition.x / 21.5f;
        float goalPosY_normalized = _goal.localPosition.y / 10f;

        float agentPosX_normalized = transform.localPosition.x / 21.5f;
        float agentPosY_normalized = transform.localPosition.y / 10f;

        Vector2 velocity = _rb.linearVelocity;

        Vector2 toGoal = (_goal.localPosition - transform.localPosition).normalized;

        sensor.AddObservation(goalPosX_normalized);
        sensor.AddObservation(goalPosY_normalized);
        sensor.AddObservation(agentPosX_normalized);
        sensor.AddObservation(agentPosY_normalized);
        sensor.AddObservation(velocity.x);
        sensor.AddObservation(velocity.y);
        sensor.AddObservation(toGoal.x);
        sensor.AddObservation(toGoal.y);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        continuousActionsOut[0] = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        continuousActionsOut[1] = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float moveY = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        Vector2 move = new Vector2(moveX, moveY);
        _rb.linearVelocity = move * _moveSpeed;

        float distanceNew = Vector2.Distance(transform.localPosition, _goal.localPosition);
        float deltaDistance = Mathf.Clamp(_distanceOld - distanceNew, -1f, 1f);
        AddReward(deltaDistance * 0.01f);
        _distanceOld = distanceNew;

        AddReward(-2f / MaxStep);
        CummulativeReward = GetCumulativeReward();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != this.gameObject && other.CompareTag("Goal"))
        {
            AddReward(3.0f);
            CummulativeReward = GetCumulativeReward();
            EndEpisode();
        }

        if (other.gameObject.tag == "Wall")
        {
            AddReward(-0.05f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.05f);
            if (_spriteRenderer != null)
                _spriteRenderer.color = Color.yellow;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.1f * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (_spriteRenderer != null)
                _spriteRenderer.color = Color.blue;
        }
    }

    private bool IsPositionValid(Vector3 position, float radius = 1f)
    {
        int wallLayerMask = LayerMask.GetMask("Wall");
        Collider2D hit = Physics2D.OverlapCircle(position, radius, wallLayerMask);
        return hit == null;
    }
}
