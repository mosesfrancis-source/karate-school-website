using KarateSchoolSystem.Exceptions;

namespace KarateSchoolSystem.Tests.Exceptions;

[TestClass]
public class UnauthorizedActionExceptionTests
{
    [TestMethod]
    public void ParameterlessConstructor_SetsDefaultMessage()
    {
        var ex = new UnauthorizedActionException();
        Assert.AreEqual("This user is not authorized to perform this action.", ex.Message);
    }

    [TestMethod]
    public void MessageConstructor_SetsMessage()
    {
        var ex = new UnauthorizedActionException("custom message");
        Assert.AreEqual("custom message", ex.Message);
    }

    [TestMethod]
    public void MessageAndInnerExceptionConstructor_SetsBoth()
    {
        var inner = new Exception("inner");
        var ex = new UnauthorizedActionException("custom message", inner);
        Assert.AreEqual("custom message", ex.Message);
        Assert.AreSame(inner, ex.InnerException);
    }

    [TestMethod]
    public void IsInvalidOperationException()
    {
        var ex = new UnauthorizedActionException();
        Assert.IsInstanceOfType<InvalidOperationException>(ex);
    }
}
