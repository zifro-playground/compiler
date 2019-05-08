using System;
using System.Collections.Generic;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    public class PyRangeTests : BaseEntityTester<PyRange, (int from, int to, int step)>
    {
        protected override string ExpectedTypeName
            => Localized_Python3_Entities.Type_Range_Name;

        protected override Type ExpectedTypeDef
            => typeof(PyRangeType);

        protected override (int from, int to, int step) DefaultValue
            => (from: 0, to: 10, step: 1);

        protected override PyRange CreateEntity(PyProcessor processor, (int from, int to, int step) value)
        {
            (int from, int to, int step) = value;
            return new PyRange(processor, from, to, step);
        }

        [TestMethod]
        public void EnumeratePositiveSteps()
        {
            // Arrange
            var entity = CreateEntity((0, 5, 1));
            var iter = entity.GetEnumerator();
            var values = new List<int>();

            // Act
            for (int i = 0; i < 10 && iter.MoveNext(); i++)
            {
                values.Add(((PyInteger)iter.Current).Value);
            }

            // Assert
            CollectionAssert.AreEqual(new[] {
                0, 1, 2, 3, 4
            }, values);
        }

        [TestMethod]
        public void EnumerateBigSteps()
        {
            // Arrange
            var entity = CreateEntity((0, 250, 37));
            var iter = entity.GetEnumerator();
            var values = new List<int>();

            // Act
            for (int i = 0; i < 10 && iter.MoveNext(); i++)
            {
                values.Add(((PyInteger)iter.Current).Value);
            }

            // Assert
            CollectionAssert.AreEqual(new[] {
                0, 37, 74, 111, 148, 185, 222
            }, values);
        }

        [TestMethod]
        public void EnumerateNegativeSteps()
        {
            // Arrange
            var entity = CreateEntity((5, 1, -1));
            var iter = entity.GetEnumerator();
            var values = new List<int>();

            // Act
            for (int i = 0; i < 10 && iter.MoveNext(); i++)
            {
                values.Add(((PyInteger)iter.Current).Value);
            }

            // Assert
            CollectionAssert.AreEqual(new[] {
                5, 4, 3, 2
            }, values);
        }

        [TestMethod]
        public void EnumerateStandStillSteps()
        {
            // Arrange
            var entity = CreateEntity((1, 1, 1));
            var iter = entity.GetEnumerator();

            // Act
            bool result = iter.MoveNext();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ToStringTest()
        {
            // Arrange
            const int from = -120;
            const int to = 50;
            const int step = 1;
            var entity = CreateEntity((from, to, step));

            // Act
            string result = entity.ToString();

            // Assert
            Assert.AreEqual(string.Format(
                Localized_Python3_Entities.Type_Range_ToString,
                from, to
            ), result);
        }

        [TestMethod]
        public void ToStringWithStepTest()
        {
            // Arrange
            const int from = -120;
            const int to = 50;
            const int step = -1;
            var entity = CreateEntity((from, to, step));

            // Act
            string result = entity.ToString();

            // Assert
            Assert.AreEqual(string.Format(
                Localized_Python3_Entities.Type_Range_ToString_Step,
                from, to, step
            ), result);
        }
    }
}