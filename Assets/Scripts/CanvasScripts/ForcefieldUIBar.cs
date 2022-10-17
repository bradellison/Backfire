using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasScripts
{
    public class ForcefieldUIBar : MonoBehaviour
    {

        
        
        public GameObject leftSide;
        public GameObject rightSide;
    
        private Image _image;
        private Image _leftSideImage;
        private Image _rightSideImage;

        private Transform _transform;
        private Vector3 _leftSideVector;
        private Vector3 _rightSideVector;

        private float _maxDistance;

        private PlayerController _playerController;
    
        public Color forcefieldInactiveColor;
        public Color forcefieldActiveColor;
        
        private void OnEnable()
        {
            //Wait 0.1 seconds before finding player to ensure player has been spawned
            Invoke(nameof(FindPlayer), 0.1f);
        }

        private void FindPlayer()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        
        private void OnDisable()
        {
            DrawLine(resetBar:true);
        }

        private void Start()
        {
            _transform = transform;
            _leftSideVector = leftSide.transform.localPosition;
            _rightSideVector = rightSide.transform.localPosition;
        
            _image = this.GetComponent<Image>();
            _leftSideImage = leftSide.GetComponent<Image>();
            _rightSideImage = rightSide.GetComponent<Image>();
        
            _maxDistance = _rightSideVector.x - _leftSideVector.x;
        }

        private void DrawLine(bool resetBar = false)
        {
            float barLength;
            
            if (!resetBar)
            {
                barLength = _playerController.forcefieldCounter / _playerController.forcefieldTarget *
                                  _maxDistance;
            }
            else
            {
                barLength = 0;
            }

            Vector3 localPosition = _transform.localPosition;
            _transform.localPosition = new Vector3(_leftSideVector.x + (barLength / 2), localPosition.y, localPosition.z);

            Vector3 localScale = _transform.localScale;
            _transform.localScale = new Vector3(barLength / 100, localScale.y, localScale.z);
        }

        public void UpdateColor()
        {
            Color newColor = _playerController.isForcefieldActive ? forcefieldActiveColor : forcefieldInactiveColor;
            _image.color = newColor;
            _leftSideImage.color = newColor;
            _rightSideImage.color = newColor;
        }

        private void Update()
        {
            //Once player is destroyed, return out of update immediately
            if (!_playerController)
            {
                return;
            }
            DrawLine();
        }
    }
}

