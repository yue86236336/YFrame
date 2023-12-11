using UnityEngine;

public class Example : MonoBehaviour
{
    // Start recording with built-in Microphone and play the recorded audio right away
    void Start()
    {
        //     AudioSource audioSource = GetComponent<AudioSource>();
        //     string[] devices = Microphone.devices;
        //     if (devices.Length > 0)
        //     {
        //         Debug.Log(devices[0]);
        //         Microphone.GetDeviceCaps(devices[0], out int sampleRate, out int bitsPerSample);
        //         Debug.Log(sampleRate + "--" + bitsPerSample);
        //         audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, sampleRate);
        //         audioSource.Play();
        //     }
    }

    public void InitAndRecord()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        string[] devices = Microphone.devices;
        if (devices.Length > 0)
        {
            Microphone.GetDeviceCaps(devices[0], out int sampleRate, out int bitsPerSample);
            audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, sampleRate);
            audioSource.Play();
        }
    }

}
