using System.Collections;
using RSG.Trellis.Signals;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    [SerializeField] private GameObject thrustersRight;
    [SerializeField] private GameObject thrustersLeft;
    [SerializeField] private IntSignal thrusterSignal;
    [SerializeField] private BoolSignal thrusterUsable;
    [SerializeField] private BoolSignal thrusting;
    
    
    private readonly Vector3 _leftInUseScale = new Vector3(0.3f, 1f, 1f);
    private readonly Vector3 _leftInUsePosition = new Vector3(-0.15f, -0.95f, 0f);
    private readonly Vector3 _leftNormalScale = new Vector3(0.15f, .5f, 0);
    private readonly Vector3 _leftNormalPosition = new Vector3(-0.15f, -0.725f, 0f);
    
    private readonly Vector3 _rightInUseScale = new Vector3(0.3f, 1, 1);
    private readonly Vector3 _rightInUsePosition = new Vector3(0.15f, -0.95f, 0);
    private readonly Vector3 _rightNormalScale = new Vector3(0.15f, .5f, 0);
    private readonly Vector3 _rightNormalPosition = new Vector3(0.15f, -0.725f, 0f);

    private bool _thrusterCharging = false;

    private WaitForSeconds _rechargeDelay;
    private WaitForSeconds _delay;


    private void OnEnable()
    {
        _rechargeDelay = new WaitForSeconds(5);
        _delay = new WaitForSeconds(0.1f);
        thrusting.AddListener(ThrustersOn);
        thrusting.AddListener(ThrustersOff);
    }

    private void OnDisable()
    {
        thrusting.RemoveListener(ThrustersOn);
        thrusting.RemoveListener(ThrustersOff);
    }

    private void ThrustersOn()
    {
        if (!thrusterUsable.Value) return;
        
        thrustersLeft.transform.localScale = _leftInUseScale;
        thrustersLeft.transform.position = transform.position + _leftInUsePosition;

        thrustersRight.transform.localScale = _rightInUseScale;
        thrustersRight.transform.position = transform.position + _rightInUsePosition;
    }
        
    private void ThrustersOff()
    {
        if (thrusting.Value) return;
        
        thrustersLeft.transform.localScale = _leftNormalScale;
        thrustersLeft.transform.position = transform.position + _leftNormalPosition;
            
        thrustersRight.transform.localScale = _rightNormalScale;
        thrustersRight.transform.position = transform.position + _rightNormalPosition;

        if (thrusterSignal.Value > 0.5f && !_thrusterCharging)
        {
            StartCoroutine(ThrusterRecharge());
        }
        if (thrusterSignal.Value <= 0)
        {
            StartCoroutine(ThrustersCoolDown());
        }
    }

    IEnumerator ThrustersCoolDown()
    {
        thrusterUsable.SetValue(false);
        yield return _rechargeDelay;
        StartCoroutine(ThrusterRecharge());
    }

    IEnumerator ThrusterRecharge()
    {
        _thrusterCharging = true;
        while (thrusterSignal.Value < 100)
        {
            yield return _delay;
            thrusterSignal.Increment(1);
        }

        thrusterUsable.SetValue(true);
        _thrusterCharging = false;
    }
    
    
}
