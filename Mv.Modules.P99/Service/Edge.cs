namespace Mv.Modules.P99.Service
{
    public struct Edge
    {
        public bool ValueChanged { get; private set; }
        private short _currentValue;
        public short CurrentValue
        {
            get => _currentValue;
            set
            {
                ValueChanged = ((_currentValue) != (value));
                _currentValue = value;
            }
        }
    }
}