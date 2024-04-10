using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MyAgent : Agent
{
    public Transform largeGoal;
    public Transform smallGoal;
    public float ballDistance;
    public float moveSpeed;

    // ÿ��ѵ����ʼʱ���г�ʼ��
    public override void OnEpisodeBegin()
    {
        RandomGoalPosition();
        transform.localPosition = Vector3.zero;
    }

    private void RandomGoalPosition()
    {
        int status = Random.Range(0, 2);
        switch (status)
        {
            case 0:
                largeGoal.transform.localPosition = Vector3.right * ballDistance;
                smallGoal.transform.localPosition = Vector3.left * ballDistance;
                break;
            default:
                largeGoal.transform.localPosition = Vector3.left * ballDistance;
                smallGoal.transform.localPosition = Vector3.right * ballDistance;
                break;
        }
    }

    // ÿ��һ���ռ�AI����Ҫ��������Ϣ��AIͨ����Щ��Ϣ�˽����ڵĻ���
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(largeGoal.localPosition.x);
        sensor.AddObservation(transform.localPosition.x);
    }

    // ָ��AIÿһ������Ϊ
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveDir = actions.ContinuousActions[0];
        transform.position += Vector3.right * moveDir * moveSpeed * Time.deltaTime;
        CalculateDistance();
    }

    private void CalculateDistance()
    {
        if (Vector3.Distance(largeGoal.position, transform.position) < 0.1f)
        {
            SetReward(1);
            EndEpisode();
        }
        else if (Vector3.Distance(smallGoal.position, transform.position) < 0.1f)
        {
            SetReward(-1);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
    }
}
