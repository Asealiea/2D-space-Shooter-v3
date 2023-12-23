using NUnit.Framework;
using RSG.Trellis.Signals;
using UnityEngine;

namespace RsgSignals.Tests.Tests.Editor
{
    public class ManualSignalTests
    {
        private TestManualSignal _sut;

        [SetUp]
        public void Setup()
        {
            _sut = ScriptableObject.CreateInstance<TestManualSignal>();
        }

        [Test]
        public void WhenSettingValue_SetsValue()
        {
            _sut.SetValue(3);
            Assert.AreEqual(3, _sut.Value);
        }

        [Test]
        public void WhenSettingValue_Triggers()
        {
            var wasCalled = false;
            _sut.AddListener(() => wasCalled = true, false);
            
            _sut.SetValue(3);
            Assert.IsTrue(wasCalled);
        }
        
        [Test]
        public void WhenEnabling_SetsValueBackToDefault()
        {
            _sut.SetValue(3);
            _sut.CurrentDefault = 1;
            Assert.AreEqual(3, _sut.Value);
            
            _sut.FakeOnEnable();
            Assert.AreEqual(1, _sut.Value);
        }

        [Test]
        public void WhenEnabling_ClearsListeners()
        {
            var wasCalled = false;
            _sut.AddListener(() => wasCalled = true, false);
            _sut.FakeOnEnable();

            _sut.SetValue(0);
            Assert.IsFalse(wasCalled);
        }

        /// <summary>
        /// Use this class to test the base methods of Signal, so
        /// no need to test in all subclasses
        /// </summary>
        public class TestManualSignal : ManualSignal<int>
        {
            public int CurrentDefault
            {
                set => initialValue = value;
            }
            
            public void FakeOnEnable()
            {
                ReInitialize();
            }
        }
    }
}