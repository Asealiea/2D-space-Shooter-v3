using System;
using System.Collections.Generic;
using System.Threading;
using RSG.Trellis.Signals.Derived;
using UnityEngine;
using UnityEngine.Events;

namespace RSG.Trellis.Signals
{
    public abstract class Signal : ScriptableObject
    {
        public abstract object UntypedValue { get; }
        protected UnityEvent OnChanged { get; } = new UnityEvent();

        private readonly HashSet<IDerivedSignal> _derivedSignals = new HashSet<IDerivedSignal>();

        private const int MaxRecursionDepth = 20;
        private int _recursionDepth = 0;

        protected virtual void OnEnable()
        {
            _recursionDepth = 0;
            OnChanged?.RemoveAllListeners();
        }
    
        protected virtual void OnDisable()
        {
            _recursionDepth = 0;
            OnChanged?.RemoveAllListeners();
        }

        public void AddListener(UnityAction call, bool runNow = true)
        {
            OnChanged.AddListener(call);
            if (runNow) call();
        }
        
        public void RemoveListener(UnityAction call)
        {
            OnChanged.RemoveListener(call);
        }
        
        public void Register(IDerivedSignal derived)
        {
            _derivedSignals.Add(derived);
        }

        public void Unregister(IDerivedSignal dependent)
        {
            _derivedSignals.Remove(dependent);
        }

        protected void NotifyAll()
        {
            NotifySubscribers();
            NotifyDerived();
        }

        private void NotifySubscribers()
        {
            WithSafety(() => OnChanged.Invoke());
        }

        protected void NotifyDerived()
        {
            foreach (var derived in _derivedSignals)
            {
                WithSafety(() =>
                {
                    derived?.Recalculate(); 
                    
                });
            }
        }

        private void WithSafety(Action action)
        {
            try
            {
                if (Interlocked.Increment(ref _recursionDepth) > MaxRecursionDepth)
                {
                    Debug.LogError($"The handler for {name} exceeded recursion depth maximum of {MaxRecursionDepth}", this);
                    return;
                }

                action();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Debug.LogError($"A handler for {name} threw an exception.  Program will continue, but this indicates a problem with error handling", this);
            }
            finally
            {
                Interlocked.Decrement(ref _recursionDepth);
            }
        }
    }

    public abstract class Signal<T> : Signal, ISignal<T>
    {
        public T Value { get; protected set; }
        protected virtual T DefaultValue => default;

        public override object UntypedValue => Value;

        protected void SetValueInternal(T value)
        {
            Value = value;
            NotifyAll();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ReInitialize();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ReInitialize();
        }

        protected virtual void ReInitialize()
        {
            OnChanged?.RemoveAllListeners();
        }
    }
}