using System.Collections;
using Utils.Exceptions;

namespace Utils.AsyncOperations;

public class AsyncOperationInfo<T> : IEnumerable<T>
{
    private readonly float _maxTimeInSeconds;
    private readonly List<T> _states = new();
    private T _currentState;
    private Action<AsyncOperationInfo<T>> _finishCallback;
    public IReadOnlyList<T> States => _states;
    public T CurrentState
    {
        get => _currentState;
        set
        {
            if (OperationState != OperationState.Running)
                throw new AsyncOperationException("operation is not running");
            _currentState = value;
            _states.Add(value);
        }
    }

    public OperationState OperationState { get; private set; } = OperationState.NonStarted;

    public AsyncOperationInfo(float maxTimeInSeconds)
    {
        _maxTimeInSeconds = maxTimeInSeconds;
    }
    public async void Start(T state)
    {
        if (OperationState != OperationState.NonStarted)
            throw new AsyncOperationException("operation is already started");
        OperationState = OperationState.Running;
        CurrentState = state;
        await Task.Delay((int)(_maxTimeInSeconds * 1000)).ContinueWith(_ =>
        {
            if (OperationState == OperationState.Running)
                Finish();
        });
    }

    public void Finish()
    {
        if (OperationState != OperationState.Running)
            throw new AsyncOperationException("operation is not running");
        OperationState = OperationState.Finished;
        _finishCallback?.Invoke(this);
    }

    public void SetFinishCallback(Action<AsyncOperationInfo<T>> action)
    {
        _finishCallback = action;
    }
    public void Wait(Action<T> stateChange = null)
    {
        if (OperationState != OperationState.Running)
            throw new AsyncOperationException("operation is not running");
        if(stateChange == null) return;
        var index = 0;
        while (OperationState == OperationState.Running)
        {
            Thread.Sleep(1);
            for (; index < _states.Count; index++) stateChange?.Invoke(_states[index]);
        }
    }

    public IEnumerator<T> GetEnumerator() => new AsyncStateoperationIEnumerator(_states);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private class AsyncStateoperationIEnumerator : IEnumerator<T>
    {
        private readonly IReadOnlyList<T> _list;
        private int _index = -1;
        public AsyncStateoperationIEnumerator(IReadOnlyList<T> list)
        {
            _list = list;
        }

        public bool MoveNext()
        {
            _index++;
            return _index < _list.Count;
        }

        public void Reset()
        {
            _index = -1;
        }

        public T Current => _list[_index];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}
