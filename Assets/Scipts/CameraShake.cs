using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
   [SerializeField] private bool _cameraShakeEnabled = true;
    private Vector3 _startingPoint;
    private float _startShaking;
    private float _stopShaking = 5f;






    // Start is called before the first frame update
    void Start()
    {
        _startingPoint = transform.position;
        
    }

    // Update is called once per frame
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
