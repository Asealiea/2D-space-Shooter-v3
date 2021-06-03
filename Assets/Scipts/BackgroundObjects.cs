using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObjects : MonoBehaviour
{
    [SerializeField] private float _spinSpeed = 0;
    [SerializeField] private float _downwardSpeed = 2;
    [SerializeField] private float _forwardSpeed = 0;
    [SerializeField] private int _ID;
    [SerializeField] private float _randomSize;

    //ID 0 = just down, ID 1 = Down and rotate, ID 2 = just forward

    // Start is called before the first frame update
    void Start()
    {
        _randomSize = Random.Range(0.3f, 0.7f);
        transform.localScale = new Vector3( _randomSize,_randomSize,1);

    }

    // Update is called once per frame
    void Update()
    {
        switch (_ID)
        {
            case 0: // just down
                transform.Translate(Vector3.down * _downwardSpeed * Time.deltaTime);
                break;
            case 1: // down and rotate
                transform.Translate(Vector3.down * _downwardSpeed * Time.deltaTime);
                transform.Rotate(Vector3.forward * _spinSpeed * Time.deltaTime);
                break;
            case 2: // forward (Comets)
                transform.Translate(Vector3.forward * _forwardSpeed * Time.deltaTime);
                break;
            case 3: // just rotate
                transform.Rotate(Vector3.forward * _spinSpeed * Time.deltaTime);
                break;



            default:
                Debug.Log("Default called");
                Destroy(this.gameObject);
                break;
        }

        if (transform.position.y <= -6f)
        {
            Destroy(this.gameObject);
        }
    }
}
