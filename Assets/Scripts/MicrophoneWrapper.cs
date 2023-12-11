using System;
using UnityEngine;
using YF;

public class MicrophoneWrapper : MonoSingleton<MicrophoneWrapper>
{

    private string TAG = "MicrophoneWrapper: ";

    //标记是否有麦克风
    private bool isHaveMic = false;

    //当前录音设备名称
    string currentDeviceName = string.Empty;

    //录音频率,控制录音质量(8000,16000)
    int recordFrequency = 8000;

    //上次按下时间戳
    double lastPressTimestamp = 0;

    //表示录音的最大时长
    int recordMaxLength = 10;

    //实际录音长度(由于unity的录音需先指定长度,导致识别上传时候会上传多余的无效字节)
    //通过该字段,获取有效录音长度,上传时候剪切到无效的字节数据即可
    int trueLength = 0;

    //存储录音的片段
    [HideInInspector]
    public AudioClip saveAudioClip;


    // Start is called before the first frame update
    void Start()
    {

    }


    public void Init()
    {
        //获取麦克风设备，判断是否有麦克风设备
        if (Microphone.devices.Length > 0)
        {
            isHaveMic = true;
            currentDeviceName = Microphone.devices[0];
        }
        else
        {
            Debug.Log(TAG + " Microphone.devices is null(0) ");
        }
    }



    /// <summary>
    /// 按下录音按钮
    /// </summary>
    /// <param name="eventData"></param>
    public void OnStartRecord()
    {

        StartRecording();
    }

    /// <summary>
    /// 放开录音按钮
    /// </summary>
    /// <param name="eventData"></param>
    public AudioClip OnStopRecord()
    {

        trueLength = EndRecording();
        if (trueLength > 1)
        {

            Debug.Log(TAG + " return AudioClip data ");
            return saveAudioClip;

        }

        Debug.Log(TAG + " return AudioClip is null ");
        return null;

    }


    /// <summary>
    /// 开始录音
    /// </summary>
    /// <param name="isLoop"></param>
    /// <param name="lengthSec"></param>
    /// <param name="frequency"></param>
    /// <returns></returns>
    private bool StartRecording(bool isLoop = false) //8000,16000
    {
        Debug.Log(TAG + "StartRecording   ");

        if (isHaveMic == false || Microphone.IsRecording(currentDeviceName))
        {
            return false;
        }

        //开始录音
        /*
         * public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency);
         * deviceName   录音设备名称.
         * loop         如果达到长度,是否继续记录
         * lengthSec    指定录音的长度.
         * frequency    音频采样率   
         */

        lastPressTimestamp = GetTimestampOfNowWithMillisecond();

        saveAudioClip = Microphone.Start(currentDeviceName, isLoop, recordMaxLength, recordFrequency);

        return true;
    }

    /// <summary>
    /// 录音结束,返回实际的录音时长
    /// </summary>
    /// <returns></returns>
    private int EndRecording()
    {
        Debug.Log(TAG + "EndRecording   ");

        if (isHaveMic == false || !Microphone.IsRecording(currentDeviceName))
        {

            Debug.Log(TAG + "EndRecording  Failed ");

            return 0;
        }

        //结束录音
        Microphone.End(currentDeviceName);

        //向上取整,避免遗漏录音末尾
        return Mathf.CeilToInt((float)(GetTimestampOfNowWithMillisecond() - lastPressTimestamp) / 1000f);
    }

    /// <summary>
    /// 获取毫秒级别的时间戳,用于计算按下录音时长
    /// </summary>
    /// <returns></returns>
    private double GetTimestampOfNowWithMillisecond()
    {
        return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
    }
}
