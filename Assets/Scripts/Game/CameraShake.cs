using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float defaultDuration = 0.3f;
    [SerializeField] private float defaultStrength = 0.3f;
    [SerializeField] [Range(0, 1)] private float rotationStrengthMultiplier = 0.5f;
    [SerializeField] private bool useRotationShake = true;

    private static CameraShake _instance;
    public static CameraShake Instance 
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<CameraShake>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }

    // shake camera based on duration and strength parameters
    public static void Shake()
    {
        Instance.OnShake(Instance.defaultDuration, Instance.defaultStrength);
    }

    private void OnShake(float duration, float strength)
    {
        // shake camera in x and y axis
        transform.DOShakePosition(duration, new Vector3(strength, strength, 0));
        
        // rotation for shake (not required)
        if (useRotationShake)
        {
            transform.DOShakeRotation(duration, new Vector3(0, 0, strength * rotationStrengthMultiplier));
        }
    }
}
