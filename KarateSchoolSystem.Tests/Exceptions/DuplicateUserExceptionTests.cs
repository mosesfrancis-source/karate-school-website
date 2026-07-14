using KarateSchoolSystem.Exceptions;

namespace KarateSchoolSystem.Tests.Exceptions;

[TestClass]
public class DuplicateUserExceptionTests
{
    [TestMethod]
    public void ParameterlessConstructor_SetsDefaultMessage()
    {
        var ex = new DuplicateUserException();
        Assert.AreEqual("A user with this ID is already registered.", ex.Message);
    }

    [TestMethod]
    public void MessageConstructor_SetsMessage()
    {
        var ex = new DuplicateUserException("custom message");
        Assert.AreEqual("custom message", ex.Message);
    }

    [TestMethod]
    public void MessageAndInnerExceptionConstructor_SetsBoth()
    {
        var inner = new Exception("inner");
        var ex = new DuplicateUserException("custom message", inner);
        Assert.AreEqual("custom message", ex.Message);
        Assert.AreSame(inner, ex.InnerException);
    }

    [TestMethod]
    public void IsInvalidOperationException()
    {
        var ex = new DuplicateUserException();
        Assert.IsInstanceOfType<InvalidOperationException>(ex);
    }
}
