using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class JoystickPlayerController : MonoBehaviour
{
    private PlayerController _playerController;
    private DynamicJoystick _fixedJoystick;
    private Vector3 _previousDirection; 

    [SerializeField] private OnGameStartScriptableObject onGameStartScriptableObject;
    
    private void Start()
    {
        _fixedJoystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
        _playerController = this.gameObject.GetComponent<PlayerController>();
        _previousDirection = Vector3.zero;
    }

    public void FixedUpdate()
    {
        HandleJoystickOutput();
        //rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private void HandleJoystickOutput()
    {
        Vector3 currentDirection;
        if(Mathf.Abs(_fixedJoystick.Horizontal) > Mathf.Abs(_fixedJoystick.Vertical)) {
            if(_fixedJoystick.Horizontal > 0) {
                currentDirection = Vector3.right;
            } else {
                currentDirection = Vector3.left;
            }
        } else if(Mathf.Abs(_fixedJoystick.Vertical) > Mathf.Abs(_fixedJoystick.Horizontal)) {
            if (_fixedJoystick.Vertical > 0) {
                currentDirection = Vector3.up;
            } else {
                currentDirection = Vector3.down;            
            } 
        } else {
            return;
        }

        //Debug.Log(variableJoystick.Vertical + " " + variableJoystick.Horizontal);

        if (currentDirection != _previousDirection)
        {
            Debug.Log(_fixedJoystick.Vertical + " " + _fixedJoystick.Horizontal);
            _playerController.swipeVector = currentDirection;
            _previousDirection = currentDirection;
        }
    }
    
    private void GameStart()
    {

    }
}