using NUnit.Framework;
using System;
using System.Collections.Generic;
using iCare.AutoReference.Editor;

namespace iCare.AutoReference.Tests {
    [TestFixture]
    public class FieldInjectorTests {
        sealed class TestObject {
            public int IntField;
            public string StringField;

            // ReSharper disable once CollectionNeverUpdated.Local
            public List<string> ListField;
            public int[] ArrayField;
            public double DoubleField;
        }

        [Test]
        public void Inject_SetsIntFieldCorrectly_WithIntData() {
            var obj = new TestObject();
            var fieldInfo = obj.GetType().GetField(nameof(TestObject.IntField));
            fieldInfo.Inject(obj, 100);

            Assert.AreEqual(100, obj.IntField);
        }

        [Test]
        public void Inject_SetsStringFieldCorrectly_WithStringData() {
            var obj = new TestObject();
            var fieldInfo = obj.GetType().GetField(nameof(TestObject.StringField));
            fieldInfo.Inject(obj, "Hello World");

            Assert.AreEqual("Hello World", obj.StringField);
        }

        [Test]
        public void Inject_SetsListFieldCorrectly_WithListData() {
            var obj = new TestObject();
            var fieldInfo = obj.GetType().GetField(nameof(TestObject.ListField));
            fieldInfo.Inject(obj, new List<string> { "One", "Two", "Three" });

            CollectionAssert.AreEqual(new List<string> { "One", "Two", "Three" }, obj.ListField);
        }

        [Test]
        public void Inject_SetsArrayFieldCorrectly_WithArrayData() {
            var obj = new TestObject();
            var fieldInfo = obj.GetType().GetField(nameof(TestObject.ArrayField));
            fieldInfo.Inject(obj, new[] { 1, 2, 3 });

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, obj.ArrayField);
        }

        [Test]
        public void Inject_ThrowsException_WhenFieldObjectIsNull() {
            var fieldInfo = typeof(TestObject).GetField("IntField");

            Assert.Throws<Exception>(() => fieldInfo.Inject(null, 42));
        }

        [Test]
        public void Inject_ThrowsException_WhenDataIsNotCompatibleWithFieldType() {
            var obj = new TestObject();
            var fieldInfo = obj.GetType().GetField(nameof(TestObject.IntField));

            Assert.Throws<Exception>(() => fieldInfo.Inject(obj, "NotAnInt"));
        }

        [Test]
        public void Inject_ThrowsException_WhenDataIsNotEnumerable_ForEnumerableField() {
            var obj = new TestObject();
            var fieldInfo = obj.GetType().GetField(nameof(TestObject.ListField));

            Assert.Throws<Exception>(() => fieldInfo.Inject(obj, 123));
        }

        [Test]
        public void Inject_SetsArrayFieldCorrectly_WhenDataIsList() {
            var obj = new TestObject();
            var fieldInfo = obj.GetType().GetField(nameof(TestObject.ArrayField));
            fieldInfo.Inject(obj, new List<int> { 1, 2, 3 });

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, obj.ArrayField);
        }

        [Test]
        public void Inject_SetsListFieldCorrectly_WhenDataIsArray() {
            var obj = new TestObject();
            var fieldInfo = obj.GetType().GetField(nameof(TestObject.ListField));
            fieldInfo.Inject(obj, new[] { "One", "Two", "Three" });

            CollectionAssert.AreEqual(new List<string> { "One", "Two", "Three" }, obj.ListField);
        }
    }
}