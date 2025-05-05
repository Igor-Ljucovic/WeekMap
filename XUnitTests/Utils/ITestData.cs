namespace XUnitTests.TestData
{
    public interface ITestData<T>
    {
        abstract public IEnumerable<T> Valid { get; }
        abstract public IEnumerable<T> Invalid { get; }
    }
}
