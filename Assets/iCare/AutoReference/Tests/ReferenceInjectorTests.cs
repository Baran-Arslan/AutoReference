using iCare.AutoReference.Editor;
using iCare.AutoReference.Tests.TestMaterials;
using NUnit.Framework;
using UnityEngine;

namespace iCare.AutoReference.Tests {
    public sealed class ReferenceInjectorTests : ScriptableObject {
        [SerializeField] TestIntServiceSo intServiceValueOneIDOne;
        [SerializeField] TestIntServiceSo intServiceValueTwoIDTwo;
        [SerializeField] TestIntServiceSo intServiceValueThreeIDThree;

        [SerializeField] TestStringServiceSo stringServiceValueOneIDOne;
        [SerializeField] TestStringServiceSo stringServiceValueTwoIDTwo;
        [SerializeField] TestStringServiceSo stringServiceValueThreeIDThree;

        [SerializeField] TestServiceRequirerSo serviceRequirer;

        DependencyResolver _serviceResolver;

        [SetUp]
        public void Setup() {
            Debug.Assert(intServiceValueOneIDOne != null, "IntServiceValueOneIDOne != null");
            Debug.Assert(intServiceValueTwoIDTwo != null, "IntServiceValueTwoIDTwo != null");
            Debug.Assert(intServiceValueThreeIDThree != null, "IntServiceValueThreeIDThree != null");
            Debug.Assert(serviceRequirer != null, "serviceRequirer != null");
            Debug.Assert(stringServiceValueOneIDOne != null, "stringServiceValueOneIDOne != null");
            Debug.Assert(stringServiceValueTwoIDTwo != null, "stringServiceValueTwoIDTwo != null");
            Debug.Assert(stringServiceValueThreeIDThree != null, "stringServiceValueThreeIDThree != null");

            ResetRequirer();

            IProvideReference[] services = {
                intServiceValueOneIDOne,
                intServiceValueTwoIDTwo,
                intServiceValueThreeIDThree,
                stringServiceValueOneIDOne,
                stringServiceValueTwoIDTwo,
                stringServiceValueThreeIDThree
            };

            _serviceResolver = new DependencyResolver(services);
        }

        [Test]
        public void Inject_SingleIntFieldCorrectly_WithIntData() {
            // Arrange & Act
            ReferenceInjector.InjectWrapper(serviceRequirer, _serviceResolver);

            // Assert
            Assert.AreEqual(1, serviceRequirer.AutoIntOneWrapper);
        }

        [Test]
        public void Inject_SetsIntFieldsCorrectly_WithIntData() {
            // Arrange & Act
            ReferenceInjector.InjectWrapper(serviceRequirer, _serviceResolver);

            // Assert
            Assert.AreEqual(1, serviceRequirer.AutoIntOneWrapper);
            Assert.AreEqual(2, serviceRequirer.AutoIntTwo);
            Assert.AreEqual(3, serviceRequirer.AutoIntThree);
        }

        [Test]
        public void Inject_SetsIntArrayFieldCorrectly_WithArrayData() {
            // Arrange & Act
            ReferenceInjector.InjectWrapper(serviceRequirer, _serviceResolver);

            // Assert
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, serviceRequirer.AllAutoInts);
        }

        [Test]
        public void Inject_SetsStringFieldsCorrectly_WithStringData() {
            // Arrange & Act
            ReferenceInjector.InjectWrapper(serviceRequirer, _serviceResolver);

            // Assert
            Assert.AreEqual("One", serviceRequirer.AutoStringOne);
            Assert.AreEqual("Two", serviceRequirer.AutoStringTwo);
            Assert.AreEqual("Three", serviceRequirer.AutoStringThree);
        }

        [Test]
        public void Inject_SetsStringArrayFieldCorrectly_WithStringArrayData() {
            // Arrange & Act
            ReferenceInjector.InjectWrapper(serviceRequirer, _serviceResolver);

            // Assert
            CollectionAssert.AreEqual(new[] { "One", "Two", "Three" }, serviceRequirer.AllAutoStrings);
        }

        [Test]
        public void Inject_ResetsFieldsCorrectly_AfterInjection() {
            // Arrange & Act (Inject data)
            ReferenceInjector.InjectWrapper(serviceRequirer, _serviceResolver);

            // Assert (Reset should clear injected values)
            ResetRequirer();
            Assert.AreEqual(0, serviceRequirer.AutoIntOneWrapper);
            Assert.AreEqual(0, serviceRequirer.AutoIntTwo);
            Assert.AreEqual(0, serviceRequirer.AutoIntThree);
            Assert.IsNull(serviceRequirer.AutoStringOne);
            Assert.IsNull(serviceRequirer.AutoStringTwo);
            Assert.IsNull(serviceRequirer.AutoStringThree);
            Assert.IsNull(serviceRequirer.AllAutoInts);
            Assert.IsNull(serviceRequirer.AllAutoStrings);
        }

        [Test]
        public void Inject_SetsListFieldsCorrectly_WithListData() {
            ReferenceInjector.InjectWrapper(serviceRequirer, _serviceResolver);

            CollectionAssert.AreEqual(new[] { "One", "Two", "Three" }, serviceRequirer.StringListWrapper);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, serviceRequirer.IntListWrapper);
        }

        void ResetRequirer() {
            _serviceResolver = null;
            serviceRequirer.AllAutoInts = null;
            serviceRequirer.AutoIntOneWrapper = 0;
            serviceRequirer.AutoIntTwo = 0;
            serviceRequirer.AutoIntThree = 0;
            serviceRequirer.AutoStringOne = null;
            serviceRequirer.AutoStringTwo = null;
            serviceRequirer.AutoStringThree = null;
            serviceRequirer.AllAutoStrings = null;
        }
    }
}