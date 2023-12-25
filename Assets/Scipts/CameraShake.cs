using System;
using System.Collections;
using System.Collections.Generic;
using RSG.Trellis.Signals;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
   [SerializeField] private bool _cameraShakeEnabled = true; //used to be able to disable the screen shake
    private Vector3 _startingPoint; //starting point of the camera
    private float _startShaking; //when to start shaking
    private float _stopShaking = 5f;//When to stop shaking
    [SerializeField] private FloatSignal shakeSignal;


    void Start()
    {
        _startingPoint = transform.position;
    }

    void Update()
    {
        if (!_cameraShakeEnabled) return;
        
        if (shakeSignal.Value > 0)
        {
            transform.position = _startingPoint + Random.insideUnitSphere * 0.5f;
            shakeSignal.Decrement(Time.deltaTime * _stopShaking);
        }
        else
        {
            shakeSignal.SetValue(0);
            transform.position = _startingPoint;
        }
    }



    public void CameraShakeOn(bool Camera)
    {
        _cameraShakeEnabled = Camera;
    }




}
