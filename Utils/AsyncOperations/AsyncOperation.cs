using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.AsyncOperations
{
    public delegate void Operation();
    public delegate void OperationFinishHandler(AsyncOperation operation); 
    public sealed class AsyncOperation
    {
        private readonly Operation _operation;
        private readonly OperationType _type;
        private readonly OperationFinishHandler? _finishHandler;
        private OperationStatus _status = OperationStatus.NotStarted;
        public OperationStatus Status { get => _status; }

        internal AsyncOperation(Operation operation, OperationType type, OperationFinishHandler? handler)
        {
            _operation = operation;
            _type = type;
            _finishHandler = handler;
            Start();
        }
        public static AsyncOperation Run(Operation operation, OperationType type, OperationFinishHandler? finishHandler = null) => new(operation, type, finishHandler);

        private void Start()
        {
            if (_type == OperationType.LongTime)
                new Thread(Do).Start();
            else if (_type == OperationType.LittleTime) Task.Run(Do);
        }
        private void Do()
        {
            _status = OperationStatus.Continues;
            _operation.Invoke();
            _finishHandler?.Invoke(this);
            _status = OperationStatus.Finished;
        }

        public void Wait()
        {
            while (Status != OperationStatus.Finished)
            {
                Thread.Sleep(0);
            }
        }
    }
}
