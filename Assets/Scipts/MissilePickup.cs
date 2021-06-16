using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePickup : MonoBehaviour
{

   [SerializeField] private GameObject _missile2, _missile3;
    private int _randomMissileAmount;


    private void Start()
    {
        _randomMissileAmount = Random.Range(0, 4);

        switch (_randomMissileAmount)
        {
            case 1:
                _missile2.SetActive(false);
                _missile3.SetActive(false);
                break;
            case 2:
                int randomside = Random.Range(0, 2);
                if (randomside == 0)
                {
                _missile2.SetActive(true);
                _missile3.SetActive(false);
                }
                else
                {
                    _missile2.SetActive(false);
                    _missile3.SetActive(true);
                }
                break;
            case 3:
                _missile2.SetActive(true);
                _missile3.SetActive(true);
                break;
            default:
                _missile2.SetActive(false);
                _missile3.SetActive(false);
                break;
        }
    }



}
