using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
   [SerializeField] private bool _cameraShakeEnabled = true; //used to be able to disable the screen shake
    private Vector3 _startingPoint; //starting point of the camera
    private float _startShaking; //when to start shaking
    private float _stopShaking = 5f;//When to stop shaking
        


    void Start()
    {
        _startingPoint = transform.position;
    }

    void Update()
    {
        if (_cameraShakeEnabled == true)
        {
            if (_startShaking > 0)
            {
                transform.position = _startingPoint + Random.insideUnitSphere * 0.5f;
                _startShaking -= Time.deltaTime * _stopShaking;
            }
            else
            {
                _startShaking = 0;
                transform.position = _startingPoint;
            }   
        }
    }

    public void CameraShaker(int shake)
    {
        _startShaking = shake;
    }

    public void CameraShakeOn(bool Camera)
    {
        _cameraShakeEnabled = Camera;
    }




}
