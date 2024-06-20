namespace WordleClash.Core.Exceptions;

public class UserNotFoundException(string value, string column) : Exception($"No user found by {value} from {column}");