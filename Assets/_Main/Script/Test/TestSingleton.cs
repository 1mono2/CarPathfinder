using System;
using NUnit.Framework;

[TestFixture]
public class SingletonTests
{
    private class TestSingleton : Singleton<TestSingleton>
    {
        public int Counter { get; set; }
    }

    [SetUp]
    public void Setup()
    {
        System.Console.WriteLine("Setup");
    }
    
    [Test]
    public void PITest()
    {
        // 値がほぼ同じかどうか
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(UnityEngine.Mathf.PI, 3.141593f);
    }

    [Test]
    public void Singleton_Always_ReturnsSameInstance()
    {
        // Arrange
        var instance1 = TestSingleton.I;
        var instance2 = TestSingleton.I;

        // Act & Assert
        Assert.That(instance1, Is.SameAs(instance2));
    }

    [Test]
    public void Singleton_CanBeDisposed()
    {
        // Arrange
        var instance = TestSingleton.I;

        // Act
        instance.Dispose();

        // Assert
        Assert.That(TestSingleton.I, Is.Not.SameAs(instance));
    }

    [Test]
    public void Singleton_CanBeDisposedMultipleTimes()
    {
        // Arrange
        var instance = TestSingleton.I;

        // Act
        instance.Dispose();
        instance.Dispose();
        instance.Dispose();

        // Assert
        Assert.That(TestSingleton.I, Is.Not.SameAs(instance));
    }

    [Test]
    public void Singleton_DisposedInstanceThrowsException()
    {
        // Arrange
        var instance = TestSingleton.I;
        instance.Dispose();

        // Act & Assert
        Assert.That(() => instance.Counter++, Throws.TypeOf<ObjectDisposedException>());
    }

    [Test]
    public void Singleton_ConstructorThrowsExceptionOnMultipleInstantiation()
    {
        // Arrange
        TestSingleton.I.Dispose();

        // Act & Assert
        Assert.That(() => new TestSingleton(), Throws.TypeOf<InvalidOperationException>());
    }
}