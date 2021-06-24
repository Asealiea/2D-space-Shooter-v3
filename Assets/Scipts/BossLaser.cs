using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [SerializeField] private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, _timer) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
