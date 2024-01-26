using System;
using System.Collections;
using System.Collections.Generic;
using _Script.Objects;
using UnityEngine;
using UnityEngine.Serialization;

public class LightDetectionManager : Singleton<LightDetectionManager>
{
    [SerializeField] private float _angleDelta = 2.0f;
    [SerializeField] private bool _debugRays = false;
    
    [SerializeField] private LayerMask _objectsLayers;
    [SerializeField] private LayerMask _playerLayer;
    
    private List<LightSource> _lightSources = new List<LightSource>();
    
    [SerializeField] private List<GameObject> _objectsInLitArea = new List<GameObject>();
    [SerializeField] private List<GameObject> _objectsDetectedInCurrentFrame = new List<GameObject>();

    private int LightSourcesCount => _lightSources.Count;
    
    public void AddLightSource(LightSource lightSource)
    {
        if (!_lightSources.Contains(lightSource))
            _lightSources.Add(lightSource);
    }
    public void RemoveLightSource(LightSource lightSource)
    {
        if (_lightSources.Contains(lightSource))
            _lightSources.Remove(lightSource);
    }

    private void Start()
    {
        if (_angleDelta < 1.0f) _angleDelta = 1.0f;
    }

    private void Update()
    {
        _objectsDetectedInCurrentFrame = new List<GameObject>();
        if (LightSourcesCount < 1) return;
        
        foreach (LightSource lightSource in _lightSources)
            SendLightRays(lightSource);
        
        CheckObjectsExit();
    }

    public void SendLightRays(LightSource lightSource)
    {
        for (float angle = 0f; angle < 360f; angle += _angleDelta)
        {
            float radAngle = angle * Mathf.Deg2Rad;
            Vector3 rayDirection = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
            Vector3 rayOrigin = lightSource.Position;
            
            if (_debugRays) Debug.DrawRay(rayOrigin, rayDirection * lightSource.Radius, Color.red);
            
            RaycastObjects(rayOrigin, rayDirection, lightSource.Radius);
            RaycastPlayer(rayOrigin, rayDirection, lightSource.Radius);
        }
    }

    private void RaycastObjects(Vector3 origin, Vector3 direction, float radius)
    {
        if (!Physics.Raycast(origin, direction, out RaycastHit hitInfo, radius, _objectsLayers)) return;
            
        GameObject detectedObject = hitInfo.transform.gameObject;
        if (!detectedObject.TryGetComponent(out ILightBehaviour lightObject)) return;

        if (_objectsDetectedInCurrentFrame.Contains(detectedObject)) return;
        _objectsDetectedInCurrentFrame.Add(detectedObject);

        if (_objectsInLitArea.Contains(detectedObject)) return;
            
        lightObject.OnEnterLight();
        _objectsInLitArea.Add(detectedObject);
    }
    
    private void RaycastPlayer(Vector3 origin, Vector3 direction, float radius)
    {
        GameObject detectedObject;
        if (!Physics.Raycast(origin, direction, out RaycastHit hitInfo, radius, _playerLayer | _objectsLayers))
        {
            Collider[] hits = Physics.OverlapSphere(origin, 0.1f, _playerLayer);
            if (hits.Length < 1) return;
            detectedObject = hits[0].gameObject;
        }
        else detectedObject = hitInfo.transform.gameObject;
        
        if (!detectedObject || !detectedObject.TryGetComponent(out Player player)) return;

        if (_objectsDetectedInCurrentFrame.Contains(detectedObject)) return;
        _objectsDetectedInCurrentFrame.Add(detectedObject);

        if (_objectsInLitArea.Contains(detectedObject)) return;
            
        player.OnEnterLight();
        _objectsInLitArea.Add(detectedObject);
    }

    private void CheckObjectsExit()
    {
        List<GameObject> toRemove = new List<GameObject>();
        
        foreach (GameObject obj in _objectsInLitArea)
        {
            if (_objectsDetectedInCurrentFrame.Contains(obj)) continue;
            
            ILightBehaviour lightObject = obj.GetComponent<ILightBehaviour>();
            if (lightObject == null) continue;
            
            lightObject.OnExitLight();
            toRemove.Add(obj);
        }

        foreach (GameObject objToRemove in toRemove)
            _objectsInLitArea.Remove(objToRemove);
    }
}
