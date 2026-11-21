using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer m_audioMixer;
    [SerializeField] Slider m_volumeSlider;
    [SerializeField] string m_volumeName;
    [SerializeField] float m_minValue;
    [SerializeField] float m_minVolume;

    private void Awake()
    {
        if (m_audioMixer.GetFloat(m_volumeName, out float startValue))
        {
            m_volumeSlider.value = startValue;
        }
    }

    public void SetMixerVolume()
    {
        if (m_volumeSlider.value == m_minValue)
        {
            m_audioMixer.SetFloat(m_volumeName, m_minVolume);
        }
        else
        {
            m_audioMixer.SetFloat(m_volumeName, m_volumeSlider.value);
        }
    }
}
