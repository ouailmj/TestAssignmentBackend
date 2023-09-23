namespace TestAssignment.Common.Exceptions;

/// <summary>
/// Exception class for handling errors related to token generation/validation.
/// </summary>
public class SecurityTokenException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityTokenException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    public SecurityTokenException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityTokenException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    /// <param name="innerException">The inner exception that is the cause of this exception.</param>
    public SecurityTokenException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityTokenException"/> class with no error message.
    /// </summary>
    public SecurityTokenException() { }
}