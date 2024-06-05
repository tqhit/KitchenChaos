using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;

    private AudioSource _audioSource;
    private float _warningSoundTimer;
    private bool _playWarningShow;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgerssAmount = .5f;
        _playWarningShow = _stoveCounter.IsFried() && e.progerssNormalized >= burnShowProgerssAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangeEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Pause();
        }
    }

    private void Update()
    {
        if (_playWarningShow)
        {
            _warningSoundTimer -= Time.deltaTime;
            if (_warningSoundTimer <= 0f)
            {
                float warningSoundTimerMax = .2f;
                _warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(_stoveCounter.transform.position);
            }
        }
    }
}
