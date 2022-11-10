using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpinnerLaser : MonoBehaviour
{

    private Transform _spinnerTransform;
    private LineRenderer _lineRenderer;
    private float _laserAngle;
    private float _laserSpinSpeed;
    private float _laserMaxDistance;
    private float _laserDistance;
    private bool _collisionsEnabled;
    
    [SerializeField] private OnLaserHitScriptableObject onLaserHitScriptableObject;

    private void Awake()
    {
        _lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        _spinnerTransform = transform.parent.gameObject.transform;
        _collisionsEnabled = false;
        _laserAngle = Random.Range(0f, 360f);
        _laserSpinSpeed = Random.Range(1f, 2f);
        _laserMaxDistance = Random.Range(1f, 1.5f);
    }

    private void Start()
    {
        Invoke(nameof(EnableCollisions), 0.5f);
    }

    private void EnableCollisions()
    {
        _collisionsEnabled = true;
    }
    
    private void Update()
    {
        UpdateLaser();
    }
    
    private void FixedUpdate()
    {
        _laserAngle += _laserSpinSpeed;
        if (_laserDistance < _laserMaxDistance && _collisionsEnabled)
        {
            _laserDistance = Mathf.Clamp(_laserDistance + 0.02f, 0, _laserMaxDistance);
        }
    }
    
    private void UpdateLaser()
    {
        Vector3 position = _spinnerTransform.position;

        Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * _laserAngle), Mathf.Cos(Mathf.Deg2Rad * _laserAngle), 0);

        Debug.DrawRay(position, direction * _laserDistance);
        Vector3 laserStartVector = new Vector3(position.x, position.y, position.z + 3);
        Vector3 laserTargetVector = laserStartVector + direction * _laserDistance;
        
        int enemyLayer = LayerMask.GetMask("Enemies");
        RaycastHit2D hit = Physics2D.Raycast(laserStartVector, direction, _laserDistance, ~enemyLayer);
        //Draw line from start to target
        _lineRenderer.SetPosition(0, laserStartVector);
        _lineRenderer.SetPosition(1, laserTargetVector);
        
        if (hit.collider)
        {
            //If laser hits something, adjust end position of laser
            _lineRenderer.SetPosition(1, hit.point);
            if (hit.collider.gameObject.CompareTag("Player") && _collisionsEnabled)
            {
                Debug.Log("Laser hit player", this);
                onLaserHitScriptableObject.onLaserHitEvent.Invoke();
            }
        }
    }
}
