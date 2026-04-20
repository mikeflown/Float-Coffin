using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterType type;
    public DistancePhase currentPhase = DistancePhase.Far;
    public Side side = Side.Left;
    public enum Side { Left, Right, Center }
    private float progress = 1f;
    public float speed = 0.35f;
    public Sprite type1FarLeft;
    public Sprite type1FarRight;
    public Sprite type1Stroboscope;
    public Sprite type2Attack;
    public Sprite type3Far;
    public Sprite type3Medium;
    public Sprite type3Near;
    public Sprite type3Attack;
    public void UpdatePhase(float deltaTime)
    {
        progress -= speed * deltaTime;
        if (progress <= 0.1f) currentPhase = DistancePhase.Attack;
        else if (progress <= 0.4f) currentPhase = DistancePhase.Near;
        else if (progress <= 0.7f) currentPhase = DistancePhase.Medium;
        else currentPhase = DistancePhase.Far;
    }
    public void Reset(MonsterType newType, Side newSide, float newSpeed = 0.35f)
    {
        type = newType;
        side = newSide;
        currentPhase = DistancePhase.Far;
        progress = 1f;
        speed = newSpeed;
    }
    public Sprite GetCurrentSprite()
    {
        if (type == MonsterType.Type1_Visual)
        {
            if (currentPhase == DistancePhase.Far)
                return (side == Side.Left) ? type1FarLeft : type1FarRight;
            return type1Stroboscope;
        }
        else if (type == MonsterType.Type2_RadarOnly)
        {
            return (currentPhase == DistancePhase.Attack) ? type2Attack : null;
        }
        else if (type == MonsterType.Type3_Central)
        {
            switch (currentPhase)
            {
                case DistancePhase.Far:    return type3Far;
                case DistancePhase.Medium: return type3Medium;
                case DistancePhase.Near:   return type3Near;
                case DistancePhase.Attack: return type3Attack;
            }
        }
        return null;
    }
    public bool ShouldShowOnMainWindow() => type == MonsterType.Type1_Visual && currentPhase == DistancePhase.Far;

    public bool IsVisibleOnStroboscope() =>
        (type == MonsterType.Type1_Visual && currentPhase >= DistancePhase.Medium) ||
        (type == MonsterType.Type2_RadarOnly && currentPhase == DistancePhase.Attack);
}