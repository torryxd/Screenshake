using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ShakeController : MonoBehaviour
{
    const float DEFAULT_MAGNITUDE = 0.1f;
    const float DEFAULT_DURATION = 0.1f;
    const int DEFAULT_FREEZE = 0;
    const bool DEFAULT_NORMALIZE = true;

    public static ShakeController Instance;

    private float _freezeTime;
    private float _originalTimeScale;
    private Vector3 _originalPosition;


    private void Awake()
    {
        Instance = this;
    }

    public async void Shake(float magnitude = DEFAULT_MAGNITUDE, float duration = DEFAULT_DURATION, int freezeMilliseconds = DEFAULT_FREEZE, bool normalize = DEFAULT_NORMALIZE, Vector2 direction = default)
    {
        if (freezeMilliseconds > 0)
        {
            _freezeTime = (float)freezeMilliseconds / 1000f;

            if (Time.timeScale > 0f)
                _originalTimeScale = Time.timeScale;

            await FreezeTime();
        }

        if (magnitude > 0f && duration > 0f)
        {
            await RandomShake(magnitude, duration, normalize, direction);
        }
    }

    private async Task FreezeTime()
    {
        float elapsed = 0f;

        while (elapsed < _freezeTime)
        {
            await Task.Yield();
            elapsed += Time.unscaledDeltaTime;

            Time.timeScale = 0f;
        }

        Time.timeScale = _originalTimeScale;
    }

    private async Task RandomShake(float magnitude, float duration, bool normalize, Vector2 direction)
    {
        float shakeTime = 0f;
        Vector2 v2;
        _originalPosition = this.transform.localPosition;

        while (shakeTime < duration)
        {
            await Task.Yield();
            shakeTime += Time.unscaledDeltaTime;

            float rnd = Random.Range(0f, 1f);
            if(direction == default)
            {
                float x = Random.Range(-rnd, rnd);
                float y = Random.Range(-(1f - rnd), (1f - rnd));
                v2 = new Vector2(x, y);
            }
            else
            {
                v2 = direction * ((rnd * 2f) - 1f);
            }

            if (normalize)
                v2 = v2.normalized;

            v2 *= magnitude;

            this.transform.localPosition = _originalPosition + new Vector3(v2.x, v2.y, 0);
        }
        this.transform.localPosition = _originalPosition;
    }
}
