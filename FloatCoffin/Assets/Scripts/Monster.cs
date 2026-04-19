using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterType type;
    public DistanceState currentDistance = DistanceState.Far;
    public Monster.Side side = Monster.Side.Left;

    public enum Side { Left, Right, Center }

    private float distanceValue = 1f; // 1 = Far → 0 = Close

    public float approachSpeed = 0.6f; // скорость приближения

    public void UpdateDistance(float deltaTime)
    {
        distanceValue = Mathf.Max(0f, distanceValue - approachSpeed * deltaTime);

        if (distanceValue <= 0.25f)      currentDistance = DistanceState.Close;
        else if (distanceValue <= 0.65f) currentDistance = DistanceState.Medium;
        else                             currentDistance = DistanceState.Far;
    }

    public void ResetMonster(MonsterType newType, Monster.Side newSide, float newApproachSpeed = 0.6f)
    {
        type = newType;
        side = newSide;
        currentDistance = DistanceState.Far;
        distanceValue = 1f;
        approachSpeed = newApproachSpeed;
    }

    public bool IsVisibleOnRadar()
    {
        if (type == MonsterType.Type1_Visual) return false;
        if (type == MonsterType.Type2_RadarOnly) return true;
        if (type == MonsterType.Type3_Central) return currentDistance == DistanceState.Close;
        return false;
    }
}