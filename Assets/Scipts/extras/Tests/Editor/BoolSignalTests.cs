using NUnit.Framework;
using RSG.Trellis.Signals;
using UnityEngine;

namespace RsgSignals.Tests.Tests.Editor
{
    public class BoolSignalTests
    {
        private BoolSignal _sut;

        [SetUp]
        public void Setup()
        {
            _sut = ScriptableObject.CreateInstance<BoolSignal>();
        }

        [Test]
        public void WhenToggled_ValueChanges()
        {
            _sut.SetValue(false);
            _sut.Toggle();
            Assert.IsTrue(_sut.Value);

            _sut.Toggle();
            Assert.IsFalse(_sut.Value);
        }

        [Test]
        public void WhenToggled_EventTriggered()
        {
            var wasCalled = false;
            _sut.AddListener(() => wasCalled = true, false);
            
            _sut.Toggle();
            Assert.IsTrue(wasCalled);
        }
    }
}