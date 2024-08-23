using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;
    //AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Bgm { Stage1, Stage2 }
    public enum Sfx { Lever, Bullet, BindBullet, Run, RunGround, Damage, MonsterDamage, MonsterBullet, Leave, PlayerDie}

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = new AudioSource();

        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        //bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
            


        //효과음 플레이어 초기화 
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        
        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(Bgm bgm)
    {
        bgmPlayer.clip = bgmClips[(int)bgm];
        bgmPlayer.Play();
    }

    public void StopBgm(Bgm bgm)
    {
         bgmPlayer.Stop();
    }

    //public void EffectBgm(bool isPlay)
    //{
    //    bgmEffect.enabled = isPlay;  
    //}

    public void VolumeController(float volume)
    {
        bgmVolume = volume;
        bgmPlayer.volume = bgmVolume;
    }


    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)    //이미 그 인덱스 사용중이면 ㅈㅈ 
                continue;

            channelIndex = loopIndex;

            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];   
            sfxPlayers[loopIndex].Play();

            break;
           
        }
    }

    

    public bool isPlaying(Sfx sfx)  //이 음원 실행중이면 실행안하도록 함(ex: 걷기 소리..) 
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying && sfxPlayers[loopIndex].clip == sfxClips[(int)sfx])   //이미 실행중인 효과음이면 ㅈㅈ 
                return false;
        }
        return true;
    }

    public void StopSfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying && sfxPlayers[loopIndex].clip == sfxClips[(int)sfx])
            {
                sfxPlayers[loopIndex].Stop();
                break;
            }
        }
    }
}
