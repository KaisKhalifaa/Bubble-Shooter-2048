using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    BallInteractionManager _ballInteractionManager;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _popSound;
    void Start()
    {
        _ballInteractionManager = BallInteractionManager.Instance;
    }

    void Update()
    {
        PlayPopSound();
    }
    void PlayPopSound()
    {
        if (_ballInteractionManager._popSoundShouldPlay)
        {
            _ballInteractionManager._popSoundShouldPlay = false;
            _audioSource.Play();
            _audioSource.pitch += 0.1f;
        }
        if (_ballInteractionManager.BallMergingIsFinished && !_ballInteractionManager._popSoundShouldPlay)
        {
            _audioSource.pitch = 1f;
        }
    }
}
