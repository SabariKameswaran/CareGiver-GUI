using UnityEngine;
using Interaxon.Libmuse;

public class EyeBlinkHandler : MonoBehaviour
{
    public AudioClip blinkAudioClip;
    public AudioClip continuousCloseAudioClip;
    public float continuousCloseThreshold = 2.0f; // Time in seconds to consider eyes continuously closed

    private AudioSource _audioSource;
    private float _blinkTimer;
    private float _closeTimer;
    private bool _eyesClosed;
    private int _blinkCount;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _blinkTimer = 0.0f;
        _closeTimer = 0.0f;
        _eyesClosed = false;
        _blinkCount = 0;
    }

    void Update()
    {
        if (InteraxonInterfacer.Instance.Artifacts.blink)
        {
            _blinkCount++;
            _blinkTimer = 0.0f;
            _closeTimer = 0.0f;
            _eyesClosed = false;

            if (_blinkCount == 2)
            {
                PlayAudio(water);
                _blinkCount = 0; // Reset blink count after playing audio
            }
        }
        else
        {
            _blinkTimer += Time.deltaTime;

            if (InteraxonInterfacer.Instance.Artifacts.headbandOn)
            {
                _closeTimer += Time.deltaTime;

                if (_closeTimer >= continuousCloseThreshold)
                {
                    if (!_eyesClosed)
                    {
                        PlayAudio(continuousCloseAudioClip);
                        _eyesClosed = true;
                    }
                }
            }
            else
            {
                _closeTimer = 0.0f;
                _eyesClosed = false;
            }
        }
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
}