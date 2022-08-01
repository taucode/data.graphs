namespace TauCode.Data.Graphs
{
    internal static class GraphHelper
    {
        public static InvalidCastException CreateUnexpectedTypeException(
            string varName,
            Type actualType,
            Type expectedType)
        {
            var message = $"'{varName}' is of type '{actualType.FullName}'. Expected type: '{expectedType.FullName}'.";
            return new InvalidCastException(message);
        }
    }
}
