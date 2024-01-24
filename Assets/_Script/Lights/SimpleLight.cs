using System;
using System.Collections;
using System.Collections.Generic;
using _Script.Objects;
using UnityEngine;

public class SimpleLight : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private float _lightRadius = 6.0f;

    [SerializeField] private List<GameObject> _objectsInSight;

    private List<GameObject> _pendingDetection;

    private void Awake()
    {
        _collider ??= GetComponent<CapsuleCollider>();
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
        List<GameObject> toRemove = new List<GameObject>();
        
        foreach (GameObject gameObj in _pendingDetection)
        {
            if (IsInSight(gameObj))
            {
                gameObj.GetComponent<ILightBehaviour>().OnEnterLight();
                toRemove.Add(gameObj);
                _objectsInSight.Add(gameObj);
            }
        }
        
        foreach (GameObject gameObj in toRemove)
            _pendingDetection.Remove(gameObj);
    }

    private bool IsInSight(GameObject obj)
    {
        Vector3 dir = obj.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, _lightRadius))
        {
            return hitInfo.transform.gameObject == obj;
        }
        return false;
    }

}
