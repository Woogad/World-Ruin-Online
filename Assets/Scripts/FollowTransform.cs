using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    private Transform _targetTransform;

    public void SetTargetTransform(Transform targetTransform)
    {
        this._targetTransform = targetTransform;
    }

    public void SetTargetTransform(Transform targetTransform, Vector3 localScale)
    {
        this._targetTransform = targetTransform;
        transform.localScale = localScale;
    }

    private void LateUpdate()
    {
        if (_targetTransform == null) return;

        transform.position = _targetTransform.position;
        transform.rotation = _targetTransform.rotation;
    }
}
