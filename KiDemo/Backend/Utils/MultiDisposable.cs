namespace KiDemo.Backend.Utils;

internal class MultiDisposable : IDisposable
{
    private readonly List<IDisposable> _disposables = new List<IDisposable>();
    private bool _disposed = false;

    public void Add(IDisposable disposable)
    {
        if (disposable == null) throw new ArgumentNullException(nameof(disposable));
        if (_disposed) throw new ObjectDisposedException(nameof(MultiDisposable));

        _disposables.Add(disposable);
    }

    public void Dispose()
    {
        if (_disposed) return;

        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }

        _disposables.Clear();
        _disposed = true;
    }
}

internal static class MultiDisposableExtensions
{
    public static void AddTo(this IDisposable disposable, MultiDisposable multiDisposable)
    {
        multiDisposable.Add(disposable);
    }
}