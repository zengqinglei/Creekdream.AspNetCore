namespace Creekdream.DataFilter
{
    /// <summary>
    /// Data filter state
    /// </summary>
    public class DataFilterState
    {
        /// <summary>
        /// Is enable
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <inheritdoc />
        public DataFilterState(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// Clone
        /// </summary>
        public DataFilterState Clone()
        {
            return new DataFilterState(IsEnabled);
        }
    }
}
