using DG.Tweening;
using UnityEngine;

public static class Knockback
{
    public static Sequence PlayKnockback(
        Transform target,
        Vector3 knockbackDir,
        float distance = 0.4f,
        float knockbackTime = 0.1f,
        float returnTime = 0.1f)
    {
        Vector3 basePos = target.position;
        Vector3 knockbackPos = basePos + knockbackDir.normalized * distance;

        Sequence seq = DOTween.Sequence();
        seq.Append(target.DOMove(knockbackPos, knockbackTime).SetEase(Ease.OutQuad));
        seq.Append(target.DOMove(basePos, returnTime).SetEase(Ease.InQuad));
        return seq;
    }
}
