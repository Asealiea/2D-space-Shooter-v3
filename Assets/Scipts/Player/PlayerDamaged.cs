using RSG.Trellis.Signals;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour
{
    [SerializeField] private GameObject damageLeft;
    [SerializeField] private GameObject damageRight;
    [SerializeField] private GameObject damageDeath;

    [SerializeField] private IntSignal playerLives;
    [SerializeField] private BoolSignal gameOver;
    [SerializeField] private BoolSignal spawnSignal;
    [SerializeField] private FloatSignal shakeSignal;

    private void OnEnable()
    {
        playerLives.AddListener(PlayerLives);
    }

    private void OnDisable()
    {
        playerLives.RemoveListener(PlayerLives);
    }

    private void PlayerLives()
    {
        switch (playerLives.Value)
        {
            case 0:
                spawnSignal.SetValue(false);
                gameOver.SetValue(true);
                //instaniate the death explosion
                Instantiate(damageDeath, transform.position, Quaternion.identity);
                shakeSignal.SetValue(3f);
                Destroy(this.gameObject);
                break;
            case 1:
                damageRight.SetActive(true);
                damageLeft.SetActive(true);
                shakeSignal.SetValue(2f);
                break;
            case 2:
                damageLeft.SetActive(true);
                damageRight.SetActive(false);
                shakeSignal.SetValue(1f);
                break;
            default:
                damageRight.SetActive(false);
                damageLeft.SetActive(false);
                break;
        }   
    }
    

}
