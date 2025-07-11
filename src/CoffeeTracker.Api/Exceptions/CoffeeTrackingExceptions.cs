namespace CoffeeTracker.Api.Exceptions;

/// <summary>
/// Base exception for all coffee tracking related exceptions
/// </summary>
public class CoffeeTrackingException : Exception
{
    /// <summary>
    /// Initializes a new instance of the CoffeeTrackingException class
    /// </summary>
    public CoffeeTrackingException() : base() { }

    /// <summary>
    /// Initializes a new instance of the CoffeeTrackingException class with a specified error message
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public CoffeeTrackingException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the CoffeeTrackingException class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public CoffeeTrackingException(string message, Exception innerException) 
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when validation fails
/// </summary>
public class ValidationException : CoffeeTrackingException
{
    /// <summary>
    /// Initializes a new instance of the ValidationException class
    /// </summary>
    public ValidationException() : base("A validation error occurred") { }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with a specified error message
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public ValidationException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public ValidationException(string message, Exception innerException) 
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when a session is not found
/// </summary>
public class SessionNotFoundException : CoffeeTrackingException
{
    /// <summary>
    /// The ID of the session that was not found
    /// </summary>
    public string SessionId { get; }

    /// <summary>
    /// Initializes a new instance of the SessionNotFoundException class with the specified session ID
    /// </summary>
    /// <param name="sessionId">The ID of the session that was not found</param>
    public SessionNotFoundException(string sessionId)
        : base($"Session with ID '{sessionId}' was not found")
    {
        SessionId = sessionId;
    }

    /// <summary>
    /// Initializes a new instance of the SessionNotFoundException class with a specified error message
    /// and session ID
    /// </summary>
    /// <param name="sessionId">The ID of the session that was not found</param>
    /// <param name="message">The message that describes the error</param>
    public SessionNotFoundException(string sessionId, string message)
        : base(message)
    {
        SessionId = sessionId;
    }

    /// <summary>
    /// Initializes a new instance of the SessionNotFoundException class with a specified error message,
    /// session ID, and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="sessionId">The ID of the session that was not found</param>
    /// <param name="message">The message that describes the error</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public SessionNotFoundException(string sessionId, string message, Exception innerException)
        : base(message, innerException)
    {
        SessionId = sessionId;
    }
}
