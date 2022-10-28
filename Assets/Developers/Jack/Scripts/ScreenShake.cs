using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ScreenShake : MonoBehaviour
{
    public float currentHeavy = 0;
    public float currentLight = 0;
    public static ScreenShake Instance { get; private set; }

    public CinemachineVirtualCamera cinemachineCamera;
    public float shakeTimer;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        Gamepad.current.SetMotorSpeeds(0f, 0f);

    }
    // Start is called before the first frame update
    public void ShakeTheScreen(float amp, float freq, float duration, float heavyRumble = 0, float lightRumble = 0)
    {
        CinemachineBasicMultiChannelPerlin MultiChannelPerlin = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        MultiChannelPerlin.m_AmplitudeGain = amp;
        MultiChannelPerlin.m_FrequencyGain = freq;

        currentHeavy = heavyRumble;
        currentLight = lightRumble;
        shakeTimer = duration;
    }

    public void Update()
    {
        if (shakeTimer > 0)
        {
            Gamepad.current.SetMotorSpeeds(currentHeavy, currentLight);


            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                Gamepad.current.ResetHaptics();
                //Timer Over!
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                Gamepad.current.SetMotorSpeeds(0f, 0f);

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
                cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
                currentHeavy = 0;
                currentLight = 0;
            }
        }
    }
}