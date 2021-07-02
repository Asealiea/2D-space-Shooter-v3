using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    [SerializeField] private GameObject[] _background;
    [SerializeField] private bool _backgroundImage;
    [SerializeField] private Vector3 _startingPos;
    private bool  _bg0, _bg1;
    [SerializeField] private int _speed = 0;

    // Start is called before the first frame update
    void Start()
    {

        if (!_backgroundImage)
        {
            _background[0].SetActive(true);
            _background[0].transform.position = new Vector3(0, 53f, 0);
            _startingPos = new Vector3(0, 70.42f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (_backgroundImage)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (!_backgroundImage)
        {
            //Bg1
            if (_background[0].transform.position.y <= -65)
            {
                _background[0].SetActive(false);
            }
            else if (_background[0].transform.position.y <= -49 && !_bg0)
            {
                _background[1].SetActive(true);
                _background[1].transform.position = _startingPos - new Vector3(0, 0.062f, 0);
                _bg0 = true;
                _bg1 = false;
            }
            //Bg2
            if (_background[1].transform.position.y <= -65)
            {
                _background[1].SetActive(false);
            }
            else if (_background[1].transform.position.y <= -49 && !_bg1)
            {
                _background[0].SetActive(true);
                _background[0].transform.position = _startingPos - new Vector3(0, 0.062f, 0); // little difference needed
                _bg1 = true;
                _bg0 = false;
                
            }           
        }
    }

    public void StartMoving()
    {
        _speed = 2;
    }



}
