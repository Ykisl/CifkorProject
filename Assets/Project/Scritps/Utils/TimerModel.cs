using System;
using UnityEngine;

namespace CifkorApp.Utils
{
    public class TimerModel
    {
        private float _time;
        private float _targetTime;

        private bool _isInitialized;
        private bool _isFinished;

        public event Action<TimerModel> OnTimerUpdated;
        public event Action<TimerModel> OnTimerFinished;

        public float Time
        {
            get => _time;
        }

        public float TargetTime
        {
            get => _targetTime;
        }

        public float NormalizedTime
        {
            get => Mathf.Clamp01(Time / TargetTime);
        }

        public bool IsInitialized
        {
            get => _isInitialized;
        }

        public bool IsFinished
        {
            get => _isFinished;
        }

        public void Initialize(float targetTime)
        {
            _targetTime = targetTime;
            Reset();

            _isInitialized = true;
        }

        public void Deinitialize()
        {
            _isInitialized = false;
            _targetTime = 0;
            Reset();
        }

        public void Reset()
        {
            _time = 0;
            _isFinished = false;
        }

        public void Update(float delta)
        {
            if(!_isInitialized || _isFinished)
            {
                return;
            }

            _time += delta;
            OnTimerUpdated?.Invoke(this);

            if (_time >= _targetTime)
            {
                FinishTimer();
            }
        }

        public void FinishTimer()
        {
            _time = _targetTime;
            _isFinished = true;

            OnTimerFinished?.Invoke(this);
        }
    }
}
