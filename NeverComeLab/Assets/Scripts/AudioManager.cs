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
        //����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = new AudioSource();

        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        //bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
            


        //ȿ���� �÷��̾� �ʱ�ȭ 
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

            if (sfxPlayers[loopIndex].isPlaying)    //�̹� �� �ε��� ������̸� ���� 
                continue;

            channelIndex = loopIndex;

            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];   
            sfxPlayers[loopIndex].Play();

            break;
           
        }
    }

    

    public bool isPlaying(Sfx sfx)  //�� ���� �������̸� ������ϵ��� ��(ex: �ȱ� �Ҹ�..) 
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying && sfxPlayers[loopIndex].clip == sfxClips[(int)sfx])   //�̹� �������� ȿ�����̸� ���� 
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
