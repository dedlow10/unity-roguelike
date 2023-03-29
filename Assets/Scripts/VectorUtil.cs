using UnityEngine;

public static class VectorUtil
{
    public static Quaternion GetRotationAngle(Transform transform, Vector3 target)
    {
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        return Quaternion.RotateTowards(transform.rotation, targetRotation, 1f);
    }

    public static bool IsTargetInRange(Transform transform, Transform target, float range)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance <= range;
    }

    public static bool IsTargetFacingAway(Transform transform, Transform target)
    {
        return Vector3.Dot(target.forward, (transform.position - target.position).normalized) < 0;
    }
}
