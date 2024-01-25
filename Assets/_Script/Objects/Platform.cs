using _Script.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, ILightBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _darkMaterial;
    [SerializeField] private Material _lightMaterial;

    public void OnEnterLight()
    {
        _renderer.material = _lightMaterial;
        _collider.isTrigger = false;
    }

    public void OnExitLight()
    {
        _renderer.material = _darkMaterial;
        _collider.isTrigger = true;
    }

    private void Awake()
    {
        _renderer ??= GetComponent<Renderer>();
        _collider ??= GetComponent<Collider>();
    }

}
