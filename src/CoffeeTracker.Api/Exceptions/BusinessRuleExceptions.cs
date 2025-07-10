namespace CoffeeTracker.Api.Exceptions;

/// <summary>
/// Exception thrown when business rules are violated
/// </summary>
public class BusinessRuleViolationException : Exception
{
    /// <summary>
    /// The business rule that was violated
    /// </summary>
    public string RuleName { get; }

    /// <summary>
    /// Initializes a new instance of the BusinessRuleViolationException class
    /// </summary>
    /// <param name="ruleName">The name of the violated business rule</param>
    /// <param name="message">The exception message</param>
    public BusinessRuleViolationException(string ruleName, string message) : base(message)
    {
        RuleName = ruleName;
    }

    /// <summary>
    /// Initializes a new instance of the BusinessRuleViolationException class
    /// </summary>
    /// <param name="ruleName">The name of the violated business rule</param>
    /// <param name="message">The exception message</param>
    /// <param name="innerException">The inner exception</param>
    public BusinessRuleViolationException(string ruleName, string message, Exception innerException) 
        : base(message, innerException)
    {
        RuleName = ruleName;
    }
}

/// <summary>
/// Exception thrown when daily entry limit is exceeded
/// </summary>
public class DailyEntryLimitExceededException : BusinessRuleViolationException
{
    /// <summary>
    /// Initializes a new instance of the DailyEntryLimitExceededException class
    /// </summary>
    /// <param name="currentCount">Current number of entries</param>
    /// <param name="maxAllowed">Maximum allowed entries</param>
    public DailyEntryLimitExceededException(int currentCount, int maxAllowed) 
        : base("DailyEntryLimit", $"Daily entry limit exceeded. Current: {currentCount}, Maximum allowed: {maxAllowed}")
    {
        CurrentCount = currentCount;
        MaxAllowed = maxAllowed;
    }

    /// <summary>
    /// Current number of entries for the day
    /// </summary>
    public int CurrentCount { get; }

    /// <summary>
    /// Maximum allowed entries per day
    /// </summary>
    public int MaxAllowed { get; }
}

/// <summary>
/// Exception thrown when daily caffeine limit is exceeded
/// </summary>
public class DailyCaffeineLimitExceededException : BusinessRuleViolationException
{
    /// <summary>
    /// Initializes a new instance of the DailyCaffeineLimitExceededException class
    /// </summary>
    /// <param name="currentCaffeine">Current caffeine amount in mg</param>
    /// <param name="additionalCaffeine">Additional caffeine being added in mg</param>
    /// <param name="maxAllowed">Maximum allowed caffeine in mg</param>
    public DailyCaffeineLimitExceededException(int currentCaffeine, int additionalCaffeine, int maxAllowed) 
        : base("DailyCaffeineLimit", $"Daily caffeine limit would be exceeded. Current: {currentCaffeine}mg, Adding: {additionalCaffeine}mg, Maximum allowed: {maxAllowed}mg")
    {
        CurrentCaffeine = currentCaffeine;
        AdditionalCaffeine = additionalCaffeine;
        MaxAllowed = maxAllowed;
    }

    /// <summary>
    /// Current caffeine amount for the day in mg
    /// </summary>
    public int CurrentCaffeine { get; }

    /// <summary>
    /// Additional caffeine being added in mg
    /// </summary>
    public int AdditionalCaffeine { get; }

    /// <summary>
    /// Maximum allowed caffeine per day in mg
    /// </summary>
    public int MaxAllowed { get; }
}

/// <summary>
/// Exception thrown when invalid timestamp is provided
/// </summary>
public class InvalidTimestampException : BusinessRuleViolationException
{
    /// <summary>
    /// Initializes a new instance of the InvalidTimestampException class
    /// </summary>
    /// <param name="timestamp">The invalid timestamp</param>
    public InvalidTimestampException(DateTime timestamp) 
        : base("InvalidTimestamp", $"Invalid timestamp provided: {timestamp}. Future dates are not allowed.")
    {
        Timestamp = timestamp;
    }

    /// <summary>
    /// The invalid timestamp that was provided
    /// </summary>
    public DateTime Timestamp { get; }
}
