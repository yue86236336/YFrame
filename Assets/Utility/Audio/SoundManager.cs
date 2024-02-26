using System.Collections.Generic;
using UnityEngine;

namespace YF
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private AudioSource _audioSource;
        private AudioSource audioSource
        {
            get
            {
                if (_audioSource == null)
                    _audioSource = GetComponent<AudioSource>();
                return _audioSource;
            }
        }

        private Dictionary<string, AudioClip> dicAudio = new Dictionary<string, AudioClip>();


        // 辅助函数：加载音频，需要确保音频文件放在Resources文件夹下
        private AudioClip LoadAudio(string path)
        {
            return Resources.Load<AudioClip>(path);
        }

        // 辅助函数：查看下dictAudio是否缓存过该音频，已经有的不需要重新加载
        private AudioClip GetAudio(string path)
        {
            if (!dicAudio.ContainsKey(path))
            {
                dicAudio[path] = LoadAudio(path);
            }
            return dicAudio[path];
        }


        // 播放背景音乐(可以设置音量)
        public void PlayBGM(string name, float volume = 1.0f, bool isLoop = true)
        {
            audioSource.Stop();
            audioSource.clip = GetAudio(name);
            audioSource.Play();
            audioSource.volume = volume;
            audioSource.loop = isLoop;
        }
        // 播放背景音乐(可以设置音量)
        public void PlayBGM(AudioClip bgm, float volume = 1.0f, bool isLoop = true)
        {
            audioSource.Stop();
            audioSource.clip = bgm;
            audioSource.Play();
            audioSource.volume = volume;
            audioSource.loop = isLoop;
        }

        public void StopBGM()
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        // 用物体身上的audioSource播放声音碎片(可以设置音量)
        public void PlaySound(AudioSource audioSource, string path, float volume = 1.0f)
        {
            audioSource.PlayOneShot(LoadAudio(path));
            audioSource.volume = volume;
        }

        // 用GameManager身上的audioSource播放声音碎片(可以设置音量)
        public void PlaySound(string path, float volume = 1.0f)
        {
            this.audioSource.PlayOneShot(LoadAudio(path));
            this.audioSource.volume = volume;
            this.audioSource.Play();
        }

        // 用GameManager身上的audioSource播放声音碎片(可以设置音量)
        public void PlaySound(AudioClip clip, float volume = 1.0f)
        {
            this.audioSource.PlayOneShot(clip);
            this.audioSource.volume = volume;
        }

        public void StopSound()
        {
            this.audioSource.Stop();
        }

        public void StopSound(AudioSource audio)
        {
            audio.Stop();
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            Debug.Log(gameObject.activeSelf);
        }
    }
}

