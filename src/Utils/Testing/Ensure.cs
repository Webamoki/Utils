using System.Collections;
using NUnit.Framework;

namespace Webamoki.Utils.Testing;

public static class Ensure
{
    public static void Throws(Func<object> code, string? message = null)
    {
        Throws<Exception>(code, message);
    }
    
    public static void Throws<TError>(Func<object> code, string? message = null) where TError : Exception
    {
        try
        {
            code();
        }
        catch (TError ex)
        {
            if (message != null && message != ex.Message)
                throw new AssertionException("Expected exception with message: " + message, ex);
            return;
        }

        throw new AssertionException("Expected exception was not thrown.");
    }

    public static void Throws<TError>(TestDelegate code, string? message = null) where TError : Exception
    {
        try
        {
            code();
        }
        catch (TError ex)
        {
            if (message != null && message != ex.Message)
                throw new AssertionException("Expected exception with message: " + message, ex);
            return;
        }

        throw new AssertionException("Expected exception was not thrown.");
    }
    public static void Throws(TestDelegate code, string? message = null)
    {
        Throws<Exception>(code, message);
    }

    public static void Equal(object? expected, object? actual)
    {
        Assert.That(actual, Is.EqualTo(expected));
    }

    public static void NotEqual(object expected, object actual)
    {
        Assert.That(actual, Is.Not.EqualTo(expected));
    }

    public static void Order<T>(LinkedList<T> list, T[] expected)
    {
        var i = 0;
        for (var first = list.First; first != null; first = first.Next, i++)
        {
            if (expected[i] != null)
            {
            }

            Equal(expected[i] ?? throw new InvalidOperationException(),
                first.Value ?? throw new InvalidOperationException());
        }
    }

    public static void True(bool condition)
    {
        Assert.That(condition, Is.True);
    }
    public static void True(object? condition)
    {
        Assert.That(condition, Is.True);
    }

    public static void False(bool condition)
    {
        Assert.That(condition, Is.False);
    }
    
    public static void False(object? condition)
    {
        Assert.That(condition, Is.False);
    }

    public static void Null(object? anObject)
    {
        Assert.That(anObject, Is.Null);
    }

    public static void NotNull(object? anObject)
    {
        Assert.That(anObject, Is.Not.Null);
    }

    public static void Contains(string substring, string actual)
    {
        Assert.That(actual, Does.Contain(substring));
    }

    public static void DoesNotContain(string substring, string actual)
    {
        Assert.That(actual, Does.Not.Contain(substring));
    }

    public static void Matches(string pattern, string actual)
    {
        Assert.That(actual, Does.Match(pattern));
    }

    public static void IsInstanceOf(Type expectedType, object? actual)
    {
        Assert.That(actual, Is.InstanceOf(expectedType));
    }

    public static void IsAssignableFrom(Type expectedType, object? actual)
    {
        Assert.That(actual, Is.AssignableFrom(expectedType));
    }

    public static void Single(ICollection collection)
    {
        Assert.That(collection, Has.Exactly(1).Items);
    }

    public static void Empty(ICollection collection) { Count(collection, 0); }
    public static void Empty(string value) { Equal("",value); }

    public static void Count(ICollection collection, int expected)
    {
        Assert.That(collection.Count, Is.EqualTo(expected), $"Expected collection count to be equal to {expected}.");
    }
    
    
}