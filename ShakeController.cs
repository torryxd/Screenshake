using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class ShakeController : MonoBehaviour
{
	public static ShakeController Instance;
	private Vector3 originalPosition;

	private void Awake() 
	{
		Instance = this;
	}

    public async void Shake(float magnitude = 0.1f, float duration = 0.1f, int freezeMS = 0)
    {
		if(freezeMS > 0 && Time.timeScale > 0){
			await justFreeze(freezeMS);
		}
		
		if(magnitude > 0 && duration > 0){
			await justShake(magnitude, duration);
		}
    }

	private async Task justFreeze(int MS) {
		float MStoS = (float)MS / 1000f;
        float d = 0f;
		float originalTimeScale = Time.timeScale;

        while (d < MStoS) {
            await Task.Yield();
            d += Time.unscaledDeltaTime;

			Time.timeScale = 0;
        }
		Time.timeScale = originalTimeScale;
    }

	private async Task justShake(float magnitude, float duration) {
        float d = 0f;
		originalPosition = this.transform.position;

        while (d < duration) {
            await Task.Yield();
            d += Time.unscaledDeltaTime;

			float rnd = Random.Range(0f, 1f);
			float x = Random.Range(-rnd, rnd);
			float y = Random.Range(-(1-rnd), (1-rnd));
            Vector2 v2 = new Vector2(x, y).normalized * magnitude;
			this.transform.position = originalPosition + new Vector3(v2.x, v2.y, 0);
        }
        this.transform.position = originalPosition;
    }

}
