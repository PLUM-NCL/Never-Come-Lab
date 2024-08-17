using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Lever, Bullet, BindBullet, Run, RunGround, Damage, MonsterDamage, MonsterBullet, Leave}

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        //배경음 플레이어 초기화



        //효과음 플레이어 초기화 
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        
        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
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
