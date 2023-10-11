using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBehaviour : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private string layerMaskName;
    
    private LineRenderer _lineRenderer;
    private RaycastHit2D _hit;
    private LayerMask _layerMask;
    
    // Start is called before the first frame update
    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _layerMask = LayerMask.GetMask(layerMaskName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _hit = Physics2D.Raycast(transform.position, direction, 10f, _layerMask);
        Debug.DrawRay(transform.position, direction * 1000, Color.red);
        if (_hit.collider != null)
        {
            Debug.Log(_hit.collider.name);
        }
    }
}
