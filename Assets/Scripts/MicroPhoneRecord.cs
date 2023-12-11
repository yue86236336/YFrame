using System;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicroPhoneRecord : MonoBehaviour
{
    private bool micConnected = false;//麦克风是否连接
    private bool startRecord = false;
    private string defaultDeviceName;
    private int frequency = 44100;
    private int minFreq;
    private int maxFreq;
    private AudioClip recordedClip;
    private AudioSource audioSource;
    //private byte[] data;
    private string fileName;//保存的文件名
    public bool isLoop = false;//录音到达预设时间之后是否自动覆盖，重新录音
    public bool syncPlay;//是否同步播放
    public bool autoSave;//是否保存音频
    public int recordMaxLength = 30;//表示录音的最大时长



    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!syncPlay && startRecord) return;
        audioSource.timeSamples = Microphone.GetPosition(defaultDeviceName);
    }

    private void Init()
    {
        string[] devices = Microphone.devices;
        audioSource = GetComponent<AudioSource>();
        if (devices.Length > 0)
        {
            micConnected = true;
            defaultDeviceName = devices[0];
            Microphone.GetDeviceCaps(devices[0], out minFreq, out maxFreq);
            frequency = UnityEngine.Random.Range(minFreq, maxFreq);
            Debug.LogFormat("设备名称为:{0} 采样频率为:{1} min-{2} max-{3}", devices[0], frequency, minFreq, maxFreq);
        }
        else
        {
            Debug.Log("缺少麦克风设备");
        }
    }

    public void StartRecord()
    {
        if (micConnected)
        {
            if (Microphone.IsRecording(defaultDeviceName))
            {
                Debug.Log("录音已经开始"); return;
            }
            recordedClip = Microphone.Start(defaultDeviceName, false, recordMaxLength, frequency);
            startRecord = true;
            Debug.Log("开始录音");
            if (syncPlay)
            {
                audioSource.clip = recordedClip;
                audioSource.Play();
                //audioSource.timeSamples = Microphone.GetPosition(defaultDeviceName);
                audioSource.timeSamples = 0;
            }
        }
        else
        {
            Debug.LogError("请确认麦克风设备是否已连接！");
        }
    }

    public void PlayRecordedClip()
    {
        if (!Microphone.IsRecording(defaultDeviceName))
        {
            audioSource.clip = recordedClip;
            audioSource.Play();
            Debug.Log("正在播放录音！");
        }
        else
        {
            Debug.Log("正在录音中，请先停止录音！");
        }
    }

    public void StopRecord()
    {
        if (Microphone.IsRecording(defaultDeviceName))
        {
            Microphone.End(defaultDeviceName);//停止录音
            startRecord = false;
            Debug.Log("停止录音");
            if (autoSave)
                Save(GetRealAudio(ref recordedClip));
        }
        else
        {
            Debug.Log("当前无录音");
        }
    }

    /// <summary>
    /// 获取真正大小的录音
    /// </summary>
    /// <param name="recordedClip"></param>
    /// <returns></returns>
    public byte[] GetRealAudio(ref AudioClip recordedClip)
    {
        int position = Microphone.GetPosition(defaultDeviceName);
        if (position <= 0 || position > recordedClip.samples)
        {
            position = recordedClip.samples;
        }
        float[] soundata = new float[position * recordedClip.channels];
        recordedClip.GetData(soundata, 0);
        recordedClip = AudioClip.Create(recordedClip.name, position,
        recordedClip.channels, recordedClip.frequency, false);
        recordedClip.SetData(soundata, 0);
        int rescaleFactor = 32767;
        byte[] outData = new byte[soundata.Length * 2];
        for (int i = 0; i < soundata.Length; i++)
        {
            short temshort = (short)(soundata[i] * rescaleFactor);
            byte[] temdata = BitConverter.GetBytes(temshort);
            outData[i * 2] = temdata[0];
            outData[i * 2 + 1] = temdata[1];
        }
        Debug.Log("position=" + position + "  outData.leng=" + outData.Length);
        return outData;
    }

    /// <summary>
    /// 保存录音
    /// </summary>
    public void Save(byte[] data)
    {
        if (!Microphone.IsRecording(defaultDeviceName))
        {
            fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            if (!fileName.ToLower().EndsWith(".wav"))
            {//如果不是“.wav”格式的，加上后缀
                fileName += ".wav";
            }
            string path = Path.Combine(Application.persistentDataPath, fileName);//录音保存路径
            Debug.Log(path);//输出路径
            using (FileStream fs = CreateEmpty(path))
            {
                fs.Write(data, 0, data.Length);
                WriteHeader(fs, recordedClip); //wav文件头
            }
        }
        else
        {
            Debug.Log("正在录音中，请先停止录音！");
        }
    }

    /// <summary>
    /// 写文件头
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="clip"></param>
    public static void WriteHeader(FileStream stream, AudioClip clip)
    {
        int hz = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        stream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        stream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(stream.Length - 8);
        stream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        stream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        stream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        stream.Write(subChunk1, 0, 4);

        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        stream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        stream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        stream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2);
        stream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        stream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        stream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        stream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        stream.Write(subChunk2, 0, 4);
    }

    /// <summary>
    /// 创建wav格式文件头
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private FileStream CreateEmpty(string filepath)
    {
        FileStream fileStream = new FileStream(filepath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < 44; i++) //为wav文件头留出空间
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }
}
