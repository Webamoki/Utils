using NUnit.Framework;
using System.Collections;
using Webamoki.Utils.Testing;

namespace Utils.Tests.Testing;

[TestFixture]
public class EnsureTest
{
    #region Throws Tests

    [Test]
    public void Throws_WithFunc_WhenExceptionThrown_ShouldPass() =>
        // Should not throw an assertion exception
        Ensure.Throws(() => throw new InvalidOperationException("Test exception"));

    [Test]
    public void Throws_WithFunc_WhenNoExceptionThrown_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() =>
            Ensure.Throws(() => "No exception"));
    }

    [Test]
    public void Throws_WithFuncAndMessage_WhenCorrectExceptionAndMessage_ShouldPass()
    {
        const string expectedMessage = "Test exception";
        Ensure.Throws(() => throw new InvalidOperationException(expectedMessage), expectedMessage);
    }

    [Test]
    public void Throws_WithFuncAndMessage_WhenCorrectExceptionButWrongMessage_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() =>
            Ensure.Throws(() => throw new InvalidOperationException("Wrong message"), "Expected message"));
    }

    [Test]
    public void ThrowsGeneric_WithFunc_WhenCorrectExceptionType_ShouldPass() => Ensure.Throws<ArgumentException>(() => throw new ArgumentException("Test"));

    [Test]
    public void ThrowsGeneric_WithTestDelegate_WhenCorrectExceptionType_ShouldPass() => Ensure.Throws<ArgumentException>(() => throw new ArgumentException("Test"));

    [Test]
    public void Throws_WithTestDelegate_WhenExceptionThrown_ShouldPass() => Ensure.Throws(() => throw new Exception("Test"));

    #endregion

    #region Equal/NotEqual Tests

    [Test]
    public void Equal_WhenValuesAreEqual_ShouldPass()
    {
        Ensure.Equal(5, 5);
        Ensure.Equal("test", "test");
        Ensure.Equal(null, null);
    }

    [Test]
    public void Equal_WhenValuesAreNotEqual_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() => Ensure.Equal(5, 10));
        _ = Assert.Throws<AssertionException>(() => Ensure.Equal("test", "different"));
    }

    [Test]
    public void NotEqual_WhenValuesAreDifferent_ShouldPass()
    {
        Ensure.NotEqual(5, 10);
        Ensure.NotEqual("test", "different");
    }

    [Test]
    public void NotEqual_WhenValuesAreEqual_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() => Ensure.NotEqual(5, 5));
        _ = Assert.Throws<AssertionException>(() => Ensure.NotEqual("test", "test"));
    }

    #endregion

    #region True/False Tests

    [Test]
    public void True_WithBool_WhenTrue_ShouldPass()
    {
        Ensure.True(true);
        Ensure.True(5 > 3);
    }

    [Test]
    public void True_WithBool_WhenFalse_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() => Ensure.True(false));
        _ = Assert.Throws<AssertionException>(() => Ensure.True(5 < 3));
    }

    [Test]
    public void True_WithObject_WhenTrue_ShouldPass()
    {
        Ensure.True((object)true);
        bool? nullableTrue = true;
        Ensure.True(nullableTrue);
    }

    [Test]
    public void False_WithBool_WhenFalse_ShouldPass()
    {
        Ensure.False(false);
        Ensure.False(5 < 3);
    }

    [Test]
    public void False_WithBool_WhenTrue_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() => Ensure.False(true));
        _ = Assert.Throws<AssertionException>(() => Ensure.False(5 > 3));
    }

    [Test]
    public void False_WithObject_WhenFalse_ShouldPass() => Ensure.False((object)false);

    #endregion

    #region Null/NotNull Tests

    [Test]
    public void Null_WhenObjectIsNull_ShouldPass()
    {
        Ensure.Null(null);
        string? nullString = null;
        Ensure.Null(nullString);
    }

    [Test]
    public void Null_WhenObjectIsNotNull_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() => Ensure.Null("not null"));
        _ = Assert.Throws<AssertionException>(() => Ensure.Null(42));
    }

    [Test]
    public void NotNull_WhenObjectIsNotNull_ShouldPass()
    {
        Ensure.NotNull("not null");
        Ensure.NotNull(42);
        Ensure.NotNull(new object());
    }

    [Test]
    public void NotNull_WhenObjectIsNull_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() => Ensure.NotNull(null));
        string? nullString = null;
        _ = Assert.Throws<AssertionException>(() => Ensure.NotNull(nullString));
    }

    #endregion

    #region String Tests

    [Test]
    public void Contains_WhenSubstringExists_ShouldPass()
    {
        Ensure.Contains("test", "This is a test string");
        Ensure.Contains("world", "Hello world!");
    }

    [Test]
    public void Contains_WhenSubstringDoesNotExist_ShouldThrowAssertionException() => _ = Assert.Throws<AssertionException>(() => Ensure.Contains("xyz", "This is a test string"));

    [Test]
    public void DoesNotContain_WhenSubstringDoesNotExist_ShouldPass()
    {
        Ensure.DoesNotContain("xyz", "This is a test string");
        Ensure.DoesNotContain("missing", "Hello world!");
    }

    [Test]
    public void DoesNotContain_WhenSubstringExists_ShouldThrowAssertionException() => _ = Assert.Throws<AssertionException>(() => Ensure.DoesNotContain("test", "This is a test string"));

    [Test]
    public void Matches_WhenPatternMatches_ShouldPass()
    {
        Ensure.Matches(@"\d+", "123");
        Ensure.Matches(@"^[A-Z][a-z]+$", "Hello");
    }

    [Test]
    public void Matches_WhenPatternDoesNotMatch_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() => Ensure.Matches(@"\d+", "abc"));
        _ = Assert.Throws<AssertionException>(() => Ensure.Matches(@"^[A-Z][a-z]+$", "hello"));
    }

    #endregion

    #region Type Tests

    [Test]
    public void IsInstanceOf_WhenCorrectType_ShouldPass()
    {
        Ensure.IsInstanceOf("test", typeof(string));
        Ensure.IsInstanceOf(42, typeof(int));
        Ensure.IsInstanceOf(new List<int>(), typeof(List<int>));
    }

    [Test]
    public void IsInstanceOf_WhenIncorrectType_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() => Ensure.IsInstanceOf("test", typeof(int)));
        _ = Assert.Throws<AssertionException>(() => Ensure.IsInstanceOf(42, typeof(string)));
    }

    [Test]
    public void IsAssignableFrom_WhenNotAssignable_ShouldThrowAssertionException() => _ = Assert.Throws<AssertionException>(() => Ensure.IsAssignableFrom("test", typeof(int)));

    #endregion

    #region Collection Tests

    [Test]
    public void Single_WhenCollectionHasOneItem_ShouldPass()
    {
        var singleItemList = new List<int> { 42 };
        Ensure.Single(singleItemList);

        var singleItemArray = new[] { "test" };
        Ensure.Single(singleItemArray);
    }

    [Test]
    public void Single_WhenCollectionIsEmpty_ShouldThrowAssertionException()
    {
        var emptyList = new List<int>();
        _ = Assert.Throws<AssertionException>(() => Ensure.Single(emptyList));
    }

    [Test]
    public void Single_WhenCollectionHasMultipleItems_ShouldThrowAssertionException()
    {
        var multipleItemsList = new List<int> { 1, 2, 3 };
        _ = Assert.Throws<AssertionException>(() => Ensure.Single(multipleItemsList));
    }

    [Test]
    public void Empty_WithCollection_WhenCollectionIsEmpty_ShouldPass()
    {
        var emptyList = new List<int>();
        Ensure.Empty(emptyList);

        var emptyArray = new int[0];
        Ensure.Empty(emptyArray);
    }

    [Test]
    public void Empty_WithCollection_WhenCollectionHasItems_ShouldThrowAssertionException()
    {
        var nonEmptyList = new List<int> { 1, 2, 3 };
        _ = Assert.Throws<AssertionException>(() => Ensure.Empty(nonEmptyList));
    }

    [Test]
    public void Empty_WithString_WhenStringIsEmpty_ShouldPass()
    {
        Ensure.Empty("");
        Ensure.Empty(string.Empty);
    }

    [Test]
    public void Empty_WithString_WhenStringIsNotEmpty_ShouldThrowAssertionException()
    {
        _ = Assert.Throws<AssertionException>(() => Ensure.Empty("not empty"));
        _ = Assert.Throws<AssertionException>(() => Ensure.Empty(" "));
    }

    [Test]
    public void Count_WhenCollectionHasExpectedCount_ShouldPass()
    {
        var list = new List<int> { 1, 2, 3 };
        Ensure.Count(list, 3);

        var array = new[] { "a", "b" };
        Ensure.Count(array, 2);

        var emptyList = new List<string>();
        Ensure.Count(emptyList, 0);
    }

    [Test]
    public void Count_WhenCollectionHasUnexpectedCount_ShouldThrowAssertionException()
    {
        var list = new List<int> { 1, 2, 3 };
        _ = Assert.Throws<AssertionException>(() => Ensure.Count(list, 5));
        _ = Assert.Throws<AssertionException>(() => Ensure.Count(list, 0));
    }

    #endregion

    #region Order Tests

    [Test]
    public void Order_WhenLinkedListMatchesExpectedOrder_ShouldPass()
    {
        var linkedList = new LinkedList<int>();
        _ = linkedList.AddLast(1);
        _ = linkedList.AddLast(2);
        _ = linkedList.AddLast(3);

        var expected = new[] { 1, 2, 3 };
        Ensure.Order(expected, linkedList);
    }

    [Test]
    public void Order_WhenLinkedListDoesNotMatchExpectedOrder_ShouldThrowAssertionException()
    {
        var linkedList = new LinkedList<int>();
        _ = linkedList.AddLast(1);
        _ = linkedList.AddLast(3);
        _ = linkedList.AddLast(2);

        var expected = new[] { 1, 2, 3 };
        _ = Assert.Throws<AssertionException>(() => Ensure.Order(expected, linkedList));
    }

    [Test]
    public void Order_WhenLinkedListIsEmpty_ShouldPass()
    {
        var linkedList = new LinkedList<string>();
        var expected = Array.Empty<string>();
        Ensure.Order(expected, linkedList);
    }

    [Test]
    public void Order_WithStringLinkedList_WhenOrderMatches_ShouldPass()
    {
        var linkedList = new LinkedList<string>();
        _ = linkedList.AddLast("first");
        _ = linkedList.AddLast("second");
        _ = linkedList.AddLast("third");

        var expected = new[] { "first", "second", "third" };
        Ensure.Order(expected, linkedList);
    }

    #endregion
}
