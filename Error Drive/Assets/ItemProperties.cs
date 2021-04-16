using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProperties
{
    public Transform transform;
    public MeshCollider collider;
    public MeshFilter meshFilter;
    public MeshRenderer meshRend;
    public Stats stats;

    public ItemProperties(Transform _transform, MeshCollider _collider, 
        MeshFilter _meshFilter, MeshRenderer _meshRend, Stats _stats)
    {
        transform = _transform;
        collider = _collider;
        meshFilter = _meshFilter;
        meshRend = _meshRend;
        stats = _stats;
    }
    public ItemProperties(GameObject item)
    {
        transform = item.GetComponent<Transform>();
        collider = item.GetComponent<MeshCollider>();
        meshFilter = item.GetComponent<MeshFilter>();
        meshRend = item.GetComponent<MeshRenderer>();
        stats = item.GetComponent<Stats>();
    }
}
