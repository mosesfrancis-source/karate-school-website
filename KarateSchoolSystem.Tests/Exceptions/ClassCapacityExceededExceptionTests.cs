using KarateSchoolSystem.Exceptions;

namespace KarateSchoolSystem.Tests.Exceptions;

[TestClass]
public class ClassCapacityExceededExceptionTests
{
    [TestMethod]
    public void ParameterlessConstructor_SetsDefaultMessage()
    {
        var ex = new ClassCapacityExceededException();
        Assert.AreEqual("This class has reached its maximum capacity.", ex.Message);
    }

    [TestMethod]
    public void MessageConstructor_SetsMessage()
    {
        var ex = new ClassCapacityExceededException("custom message");
        Assert.AreEqual("custom message", ex.Message);
    }

    [TestMethod]
    public void MessageAndInnerExceptionConstructor_SetsBoth()
    {
        var inner = new Exception("inner");
        var ex = new ClassCapacityExceededException("custom message", inner);
        Assert.AreEqual("custom message", ex.Message);
        Assert.AreSame(inner, ex.InnerException);
    }

    [TestMethod]
    public void IsInvalidOperationException()
    {
        var ex = new ClassCapacityExceededException();
        Assert.IsInstanceOfType<InvalidOperationException>(ex);
    }
}
