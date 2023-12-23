using System;
using NUnit.Framework;
using RSG.Trellis.Signals;
using UnityEngine;

namespace RsgSignals.Tests.Tests.Editor
{
    public class SignalTests
    {
        private BaseTestSignal _sut;

        [SetUp]
        public void Setup()
        {
            _sut = ScriptableObject.CreateInstance<BaseTestSignal>();
            Debug.unityLogger.logEnabled = false; // Since we're testing things that log exceptions
        }

        [TearDown]
        public void TearDown()
        {
            Debug.unityLogger.logEnabled = true; // Since we're testing things that log exceptions
        }
        
        [Test]
        public void WhenSubscribing_ListenerIsCalled()
        {
            var wasCalled = false;
            _sut.AddListener(() => wasCalled = true);
            
            Assert.IsTrue(wasCalled);
        }
        
        [Test]
        public void WhenSubscribing_ListenerIsNotCalled()
        {
            var wasCalled = false;
            _sut.AddListener(() => wasCalled = true, runNow: false);
            
            Assert.IsFalse(wasCalled);
        }

        [Test]
        public void WhenValueSet_ListenerIsCalled()
        {
            var wasCalled = false;
            _sut.AddListener(() => wasCalled = true, runNow: false);
            
            _sut.TriggerChange();
            Assert.IsTrue(wasCalled);
        }

        [Test]
        public void ExceptionDoesntDestroyEverything()
        {
            _sut.AddListener(() => throw new Exception("Some error"), false);
            _sut.TriggerChange();
            
            Assert.Pass("That exception above did not break the flow");
        }

        [Test]
        public void NoInfiniteLoop()
        {
            var callCount = 0;
            _sut.AddListener(() =>
            {
                if (callCount > 50)
                {
                    Assert.Fail("Too many recursions!");
                }

                callCount++;
                _sut.TriggerChange();
            }, false);
            
            _sut.TriggerChange();
            Assert.Pass("Ran less than 50 recursions");
        }
        
        [Test]
        public void WhenEnabling_ClearsListeners()
        {
            var wasCalled = false;
            _sut.AddListener(() => wasCalled = true, false);
            _sut.FakeOnEnable();

            _sut.TriggerChange();
            Assert.IsFalse(wasCalled);
        }

        /// <summary>
        /// Use this class to test the base methods of Signal, so
        /// no need to test in all subclasses
        /// </summary>
        public class BaseTestSignal : Signal<int>
        {
            public void FakeOnEnable()
            {
                ReInitialize();
            }
            
            public void TriggerChange()
            {
                SetValueInternal(default);
            }
        }
    }
}