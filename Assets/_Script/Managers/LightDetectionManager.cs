using System;
using System.Collections;
using System.Collections.Generic;
using _Script.Objects;
using UnityEngine;

public class LightDetectionManager : Singleton<LightDetectionManager>
{
    [SerializeField] private LayerMask objectsLayers;
    [SerializeField] private LayerMask playerLayer;
    
    private List<LightSource> _lightSources = new List<LightSource>();
    
    private List<GameObject> _objectsInLitArea = new List<GameObject>();
    private List<GameObject> _objectsDetectedInCurrentFrame = new List<GameObject>();

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

    private void Update()
    {
        if (LightSourcesCount < 1) return;
        
        _objectsDetectedInCurrentFrame.Clear();
        
        foreach (LightSource lightSource in _lightSources)
            SendLightRays(lightSource);
        
        CheckObjectsExit();
    }
    
    public void SendLightRays(LightSource lightSource)
    {
        for (float angle = 0f; angle < 360f; angle += lightSource.AngleDelta)
        {
            float radAngle = angle * Mathf.Deg2Rad;
            Vector3 rayDirection = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
            Vector3 rayOrigin = lightSource.Position;

            RaycastObjects(rayOrigin, rayDirection, lightSource.Radius);
            RaycastPlayer(rayOrigin, rayDirection, lightSource.Radius);
            /*
            if (!Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, lightSource.Radius)) continue;
            
            GameObject detectedObject = hitInfo.transform.gameObject;
            if (!detectedObject.TryGetComponent(out ILightBehaviour lightObject)) continue;

            if (_objectsDetectedInCurrentFrame.Contains(detectedObject)) continue;
            _objectsDetectedInCurrentFrame.Add(detectedObject);

            if (_objectsInLitArea.Contains(detectedObject)) continue;
            
            lightObject.OnEnterLight();
            _objectsInLitArea.Add(detectedObject);*/
        }
    }

    private void RaycastObjects(Vector3 origin, Vector3 direction, float radius)
    {
        if (!Physics.Raycast(origin, direction, out RaycastHit hitInfo, radius, objectsLayers)) return;
            
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
        if (!Physics.Raycast(origin, direction, out RaycastHit hitInfo, radius, playerLayer|objectsLayers)) return;
            
        GameObject detectedObject = hitInfo.transform.gameObject;
        if (!detectedObject.TryGetComponent(out ILightBehaviour lightObject)) return;

        if (_objectsDetectedInCurrentFrame.Contains(detectedObject)) return;
        _objectsDetectedInCurrentFrame.Add(detectedObject);

        if (_objectsInLitArea.Contains(detectedObject)) return;
            
        lightObject.OnEnterLight();
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
