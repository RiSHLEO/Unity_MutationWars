using Unity.MLAgents;
using UnityEngine;

public class GUI_Agent : MonoBehaviour
{
    [SerializeField] private TrainingAgent _trainingAgent;

    private GUIStyle _defaultStyle = new GUIStyle();
    private GUIStyle _positiveStyle = new GUIStyle();
    private GUIStyle _negativeStyle = new GUIStyle();

    private void Start()
    {
        _defaultStyle.fontSize = 40;
        _defaultStyle.normal.textColor = Color.yellow;

        _positiveStyle.fontSize = 40;
        _positiveStyle.normal.textColor = Color.green;

        _negativeStyle.fontSize = 40;
        _negativeStyle.normal.textColor = Color.red;
    }

    private void OnGUI()
    {
        string debugEpisode = "Episode:  " + _trainingAgent.CurrentEpisode + " - Step: " + _trainingAgent.StepCount;
        string debugReward = "Reward:  " + _trainingAgent.CummulativeReward.ToString();

        GUIStyle rewardStyle = _trainingAgent.CummulativeReward < 0 ? _negativeStyle : _positiveStyle;

        GUI.Label(new Rect(20, 20, 500, 30), debugEpisode, _defaultStyle);
        GUI.Label(new Rect(20, 60, 500, 30), debugReward, rewardStyle);
    }
}
