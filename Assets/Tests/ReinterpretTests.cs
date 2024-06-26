﻿using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using Store;

// ReSharper disable once CheckNamespace
namespace Editor.Tests
{
    [TestFixture]
    public class ReinterpretTests
    {
        struct Data0 : ISingletonData
        {
            
        }
        
        [Test]
        public void NativeArrayWriteAndGetData()
        {
            var store = new NativeStoreUnsafe(2);

            Data1 data1 = new Data1()
            {
                Index = 5,
                Multiplier = 5.44446f,
                Time = 56.77545432
            };

            Data2 data2 = new Data2()
            {
                Index = 2,
                IsEnemy = true,
                Multiplier = -4.301f,
                TimeLossy = 1234.3254f
            };
            
            store.AddToArray(data1, 0);
            store.AddToArray(data2, 1);

            var res1 = store.GetFromArray<Data1>(0);
            var res2 = store.GetFromArray<Data2>(1);

            Assert.AreEqual(res1.Index, data1.Index);
            Assert.AreEqual(res1.Multiplier, data1.Multiplier);
            Assert.AreEqual(res1.Time, data1.Time);
            Assert.AreEqual(res2.Index, data2.Index);
            Assert.AreEqual(res2.Multiplier, data2.Multiplier);
            Assert.AreEqual(res2.TimeLossy, data2.TimeLossy);
            Assert.AreEqual(res2.IsEnemy, data2.IsEnemy);
        }

        [Test]
        public void NativeHashMapAddAndGetData()
        {
            var store = new NativeStoreUnsafe(2);

            Data1 data1 = new Data1()
            {
                Index = 5,
                Multiplier = 5.44446f,
                Time = 56.77545432
            };

            Data2 data2 = new Data2()
            {
                Index = 2,
                IsEnemy = true,
                Multiplier = -4.301f,
                TimeLossy = 1234.3254f
            };
            
            store.AddOrUpdateDirect(data1);
            store.AddOrUpdateDirect(data2);

            var res1 = store.GetValueDirect<Data1>();
            var res2 = store.GetValueDirect<Data2>();
            
            Assert.AreEqual(res1.Index, data1.Index);
            Assert.AreEqual(res1.Multiplier, data1.Multiplier);
            Assert.AreEqual(res1.Time, data1.Time);

            Assert.AreEqual(res2.Index, data2.Index);
            Assert.AreEqual(res2.Multiplier, data2.Multiplier);
            Assert.AreEqual(res2.TimeLossy, data2.TimeLossy);
            Assert.AreEqual(res2.IsEnemy, data2.IsEnemy);
        }

        [Test, Performance]
        public void AddOrUpdateNormal()
        {
            var store = new Store.Store(1);

            var data = new Data0();
            
            Measure.Method(() => store.AddOrUpdate(data))
                .SampleGroup(new SampleGroup("AddOrUpdateNormal", SampleUnit.Nanosecond))
                .WarmupCount(10)
                .DynamicMeasurementCount()
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }

        [Test, Performance]
        public void TestArrayNative()
        {
            var store = new NativeStoreUnsafe(2);

            Data1 data1 = new Data1()
            {
                Index = 5,
                Multiplier = 5.44446f,
                Time = 56.77545432
            };
            
            var add = new SampleGroup("AddToNativeArray", SampleUnit.Nanosecond);
            var get = new SampleGroup("GetFromNativeArray", SampleUnit.Nanosecond);

            Measure.Method(() => store.AddToArray(data1, 0))
                .SampleGroup(add)
                .WarmupCount(100)
                .DynamicMeasurementCount()
                .IterationsPerMeasurement(5)
                .Run();
            
            // Measure.Method(() => store.GetFromArray<Data1>(0))
            //     .SampleGroup(get)
            //     .WarmupCount(10)
            //     .MeasurementCount(1)
            //     .IterationsPerMeasurement(5)
            //     .GC()
            //     .Run();
        }
        
        [Test, Performance]
        public void Vector2_MoveTowards()
        {
            var group = new SampleGroup("MoveTowards", SampleUnit.Nanosecond);

            Measure.Method(() =>
                {
                    Vector2.MoveTowards(Vector2.one, Vector2.zero, 0.5f);
                })
                .SampleGroup(group)
                .WarmupCount(5)
                .IterationsPerMeasurement(10000)
                .MeasurementCount(20)
                .Run();
        }
        
        [Test, Performance]
        public void AddOrUpdateNative()
        {
            var store = new NativeStoreUnsafe(2);

            Data1 data1 = new Data1()
            {
                Index = 5,
                Multiplier = 5.44446f,
                Time = 56.77545432
            };

            // Data2 data2 = new Data2()
            // {
            //     Index = 2,
            //     IsEnemy = true,
            //     Multiplier = -4.301f,
            //     TimeLossy = 1234.3254f
            // };

            Measure.Method(() => store.AddOrUpdateDirect(data1))
                .SampleGroup(new SampleGroup("AddOrUpdateNative", SampleUnit.Nanosecond))
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
            
            store.AddOrUpdateDirect(data1);
            //store.AddOrUpdateDirect(data2);

            Measure.Method(() => store.GetValueDirect<Data1>())
                .SampleGroup(new SampleGroup("GetValueDirect", SampleUnit.Nanosecond))
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
            var res1 = store.GetValueDirect<Data1>();
            //var res2 = store.GetValueDirect<Data2>();
        }
    }
}