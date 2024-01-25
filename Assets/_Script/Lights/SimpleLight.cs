using System;
using System.Collections;
using System.Collections.Generic;
using _Script.Objects;
using UnityEngine;

/// <summary>
/// Old light source object detection for comparison
/// </summary>
public class SimpleLight : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private float _lightRadius = 6.0f;

    [SerializeField] private List<GameObject> _objectsInSight;

    private List<GameObject> _pendingDetection;

    private void Awake()
    {
        _collider ??= GetComponent<CapsuleCollider>();
        _light ??= GetComponent<Light>();
        _collider.radius = _lightRadius;

        _objectsInSight = new List<GameObject>();
        _pendingDetection = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObj = other.gameObject;
        
        ILightBehaviour obj = gameObj.GetComponent<ILightBehaviour>();
        if (obj == null) return;
        
        if (!_pendingDetection.Contains(gameObj))
            _pendingDetection.Add(gameObj);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject gameObj = other.gameObject;
        if (_pendingDetection.Contains(gameObj))
        {
            _pendingDetection.Remove(gameObj);
            return;
        }
        if (_objectsInSight.Contains(gameObj))
        {
            _objectsInSight.Remove(gameObj);
            gameObj.GetComponent<ILightBehaviour>().OnExitLight();
        }
    }

    private void FixedUpdate()
    {
        CheckPendingList();
        CheckObjectsInSight();
    }

    private bool IsInSight(GameObject obj)
    {
        if (!_light.isActiveAndEnabled) return false;
        
        Vector3 dir = obj.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir);
        RaycastHit[] allHitInfos = Physics.RaycastAll(ray, _lightRadius);
        int count = allHitInfos.Length;
        for (int i = 0; i < count; i++)
        {
            RaycastHit hitInfo = allHitInfos[i];
            if (hitInfo.transform.GetComponent<ILightBehaviour>() == null) return false;
            if (hitInfo.transform.gameObject == obj) return true;
        }
        return false;
    }

    private void CheckPendingList()
    {
        // pending list -> light list
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject gameObj in _pendingDetection)
        {
            if (IsInSight(gameObj))
            {
                gameObj.GetComponent<ILightBehaviour>().OnEnterLight();
                toRemove.Add(gameObj);
                if (!_objectsInSight.Contains(gameObj))
                    _objectsInSight.Add(gameObj);
            }
        }
        foreach (GameObject gameObj in toRemove)
            _pendingDetection.Remove(gameObj);
    }

    private void CheckObjectsInSight()
    {
        // light list -> pending list
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject gameObj in _objectsInSight)
        {
            if (!IsInSight(gameObj))
            {
                gameObj.GetComponent<ILightBehaviour>().OnExitLight();
                toRemove.Add(gameObj);
                if (!_pendingDetection.Contains(gameObj))
                    _pendingDetection.Add(gameObj);
            }
        }
        foreach (GameObject gameObj in toRemove)
            _objectsInSight.Remove(gameObj);
    }
}
