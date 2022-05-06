using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : Singleton<ScreenShake>
{
    public CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin noise;

    int shakeCount = 0;

    void Awake()
    {
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private IEnumerator _ProcessShake(float shakeDuration, float shakeIntensity, float amplitudeIntensity)
    {
        shakeCount++;
        Noise(amplitudeIntensity, shakeIntensity);
        yield return new WaitForSeconds(shakeDuration);
        shakeCount--;
        if (shakeCount == 0)
        {
            Noise(0, 0);
        }
    }

    public void Shake(float shakeDuration = 0.5f, float shakeIntensity = 5f, float amplitudeIntensity = 1f)
    {
        StartCoroutine(_ProcessShake(shakeDuration, shakeIntensity, amplitudeIntensity));
    }

    public void StopAllShakes()
    {
        StopAllCoroutines();
        shakeCount = 0;
        Noise(0, 0);
    }

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;
    }
}
