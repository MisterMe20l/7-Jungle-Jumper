using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class WorldSound : MonoBehaviour, IStartFight, IWinGame
{
    [SerializeField] LayerMask m_playerLayer;

    [SerializeField] AudioSource m_whiteNoiseOutside;
    [SerializeField] AudioSource m_whiteNoiseInside;
    [SerializeField] AudioSource m_cillingMusic;
    [SerializeField] AudioSource m_battleMusic;
    [SerializeField] AudioSource m_winnerMusic;

    [SerializeField] AnimationCurve m_musicAnmCurve;
    [SerializeField] Transform m_distanceLobby;
    [SerializeField] Transform m_distanceCave;
    [SerializeField] float m_maxVolume_wn;
    [SerializeField] float m_minVolume_wn;
    [SerializeField] float m_maxVolume_m;
    [SerializeField] float m_maxDistance;
    [SerializeField] float m_timeToStartMusic;
    [SerializeField] float m_1stMusicAnmDuration_s;
    [SerializeField] float m_normalMusicAnmDuration_s;

    GameObject player;
    float volume;
    float distance;
    float musicTimer;
    bool volumeToChange;
    bool outsideCave;
    bool playingMusic;
    bool firstStart;
    bool fighting;
    bool won;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        m_whiteNoiseOutside.volume = m_minVolume_wn;
        m_whiteNoiseOutside.Play();

        m_whiteNoiseInside.volume = 0.0f;
        m_whiteNoiseInside.Play();

        m_cillingMusic.volume = 0.0f;
        m_cillingMusic.Play();
        playingMusic = false;
        firstStart = true;
        won = false;

        m_battleMusic.volume = 0.0f;
        m_battleMusic.Play();
    }

    private void Update()
    {
        if (!playingMusic) MusicStart();

        if (!fighting)
        {
            GetDistance();
            SetDistanceToVolume();
        }
    }

    private void GetDistance()
    {
        outsideCave = PlayerOutside();

        if (outsideCave)
        {
            distance = m_distanceLobby.transform.position.x - player.transform.position.x;
        }
        else
        {
            distance = player.transform.position.x - m_distanceCave.transform.position.x;
        }
    }

    private void SetDistanceToVolume()
    {
        if (distance > 0 && distance < m_maxDistance) // In Transition
        {
            volumeToChange = true;
            var distanceToVolumeDown = (1 - (distance / m_maxDistance)); // Volume decreasing with distance
            var distanceToVolumeUp = (distance / m_maxDistance); // Volume increasing with distance

            if (outsideCave) // Lobby
            {
                m_whiteNoiseOutside.volume = Mathf.Max(m_minVolume_wn, distanceToVolumeDown * m_maxVolume_wn);
            }
            else // Cave
            {
                StopCoroutine(StartMusic(m_cillingMusic));

                m_cillingMusic.volume = distanceToVolumeDown * m_maxVolume_m;
                m_whiteNoiseOutside.volume = distanceToVolumeDown * m_maxVolume_wn;
                m_whiteNoiseInside.volume = distanceToVolumeUp * m_maxVolume_wn;
            }
        }
        else if (volumeToChange && distance < 0.0f) // In Nature
        {
            volumeToChange = false;

            m_whiteNoiseInside.volume = 0.0f;
            m_whiteNoiseOutside.volume = m_maxVolume_wn;
        }
        else if (volumeToChange && distance > m_maxDistance) // Inside
        {
            volumeToChange = false;

            if (outsideCave) // In Lobby
            {
                m_cillingMusic.volume = m_maxVolume_m;
                m_whiteNoiseOutside.volume = m_minVolume_wn;
                m_whiteNoiseInside.volume = 0.0f;
            }
            else // In Cave
            {
                m_cillingMusic.volume = (!won) ? 0.0f : m_cillingMusic.volume;
                m_whiteNoiseInside.volume = m_maxVolume_wn;
                m_whiteNoiseOutside.volume = 0.0f;
            }
        }
    }

    private void MusicStart()
    {
        musicTimer += Time.deltaTime;
        if (musicTimer > m_timeToStartMusic)
        {
            playingMusic = true;
            musicTimer = 0;
            StartCoroutine(StartMusic(m_cillingMusic));
        }
    }

    private IEnumerator StartMusic(AudioSource _music)
    {
        var timer = 0.0f;
        var anmDuration = (firstStart) ? m_1stMusicAnmDuration_s : m_normalMusicAnmDuration_s;
        firstStart = false;

        while (timer < anmDuration)
        {
            timer += Time.deltaTime;
            _music.volume = m_musicAnmCurve.Evaluate(timer / anmDuration);
            yield return new WaitForEndOfFrame();
        }
    }

    private bool PlayerOutside()
    {
        return Physics2D.OverlapBox(transform.position,
            transform.lossyScale, 0.0f, m_playerLayer) != null;
    }

    public void StartFight()
    {
        fighting = true;
        StartCoroutine(StartMusic(m_battleMusic));
    }

    public void RestartFight()
    {
        StartCoroutine(RestartFightSounds()); 
    }

    private IEnumerator RestartFightSounds()
    {
        m_battleMusic.volume = 0.0f;
        m_cillingMusic.volume = 0.0f;

        yield return new WaitForSeconds(1.5f);

        fighting = false;
        playingMusic = false;
    }

    public void WinGame()
    {
        won = true;
        m_winnerMusic.Play();
        StartCoroutine(RestartFightSounds());
    }
}
// To Download Still: jumping; you win;
// Exit door -> Main Menu