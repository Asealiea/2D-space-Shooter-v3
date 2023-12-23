using System.Collections.Generic;
using NUnit.Framework;
using RSG.Trellis.Signals;
using RSG.Trellis.Signals.Derived;
using UnityEngine;

namespace RsgSignals.Tests.Tests.Editor
{
    public class DerivedSignalTests
    {
        private IntSignal _source;
        private TestDerivedIsEvenSignal _sut;

        [SetUp]
        public void Setup()
        {
            _source = ScriptableObject.CreateInstance<IntSignal>();
            _source.SetValue(0);
            _sut = ScriptableObject.CreateInstance<TestDerivedIsEvenSignal>();
            _sut.Source = _source;
            _sut.FakeOnEnable();
        }

        [Test]
        public void WhenInitialized_CalculatedStraightAway()
        {
            _source.SetValue(1);
            _sut.FakeOnEnable();

            Assert.IsFalse(_sut.Value);
        }
        
        [Test]
        public void WhenSourceChanges_ValueIsUpdated()
        {
            Assert.IsTrue(_sut.Value);
            
            _source.SetValue(1);
            Assert.IsFalse(_sut.Value);
        }
        
        [Test]
        public void WhenSourceChanges_Triggered()
        {
            var wasCalled = false;
            _sut.AddListener(() => wasCalled = true, false);

            Assert.IsFalse(wasCalled);
            
            _source.SetValue(1);
            Assert.IsTrue(wasCalled);
        }
        
        public class TestDerivedIsEvenSignal : DerivedSignal<bool>
        {
            public IntSignal Source { get; set; }

            public void FakeOnEnable()
            {
                ReInitialize();
            }
            
            public override void Recalculate()
            {
                if (Source != null)
                {
                    SetValueInternal(Source.Value % 2 == 0);
                }
            }

            public override IEnumerable<Signal> Dependees => new[] { Source };
        }
    }
}