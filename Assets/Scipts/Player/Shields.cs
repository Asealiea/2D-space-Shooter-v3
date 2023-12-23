using UnityEngine;

public class Shields : MonoBehaviour
{
    [SerializeField] private GameObject shields;
    [SerializeField] private ShieldSignal shieldSignal;
    [SerializeField] private Renderer renderer;
    private MaterialPropertyBlock _propertyBlock;
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");
    
    private void OnEnable()
    {
        shieldSignal.AddListener(UpdateShields);
        _propertyBlock = new MaterialPropertyBlock();
    }

    private void OnDisable()
    {
        shieldSignal.RemoveListener(UpdateShields);
    }

    void ShieldColorChange(Color color)
    {
        _propertyBlock.SetColor(ColorProperty,color);
        renderer.SetPropertyBlock(_propertyBlock);
    }

    private void UpdateShields()
    {
        switch (shieldSignal.Value)
        {
            case 0:
                shields.SetActive(false);
                break;
            case 1:
                ShieldColorChange(Color.red);
                break;
            case 2:
                ShieldColorChange(Color.green);
                break;
            case 3:
                shields.SetActive(true);
                ShieldColorChange(Color.white);
                break;
        }
    }
}
