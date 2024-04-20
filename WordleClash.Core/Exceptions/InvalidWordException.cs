namespace WordleClash.Core.Exceptions;

public class InvalidWordException(string input) : Exception($"{input} is not a valid word.");