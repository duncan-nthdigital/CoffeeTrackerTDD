using System.ComponentModel.DataAnnotations;

namespace CoffeeTracker.Api.Models;

/// <summary>
/// Represents a coffee shop where coffee can be purchased or consumed.
/// Used for tracking coffee source in anonymous coffee logging.
/// </summary>
public class CoffeeShop
{
    #region Constants
    
    /// <summary>
    /// Maximum length for coffee shop name field
    /// </summary>
    public const int NameMaxLength = 100;
    
    /// <summary>
    /// Maximum length for coffee shop address field
    /// </summary>
    public const int AddressMaxLength = 200;
    
    #endregion

    /// <summary>
    /// Gets or sets the unique identifier for the coffee shop.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the coffee shop.
    /// Must be between 1 and 100 characters in length.
    /// </summary>
    [Required]
    [StringLength(NameMaxLength, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address of the coffee shop.
    /// Optional field with maximum length of 200 characters.
    /// </summary>
    [StringLength(AddressMaxLength)]
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the coffee shop is active.
    /// Defaults to true when a new coffee shop is created.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the date and time when the coffee shop record was created.
    /// Automatically set to UTC time when the object is instantiated.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CoffeeShop"/> class.
    /// Sets the CreatedAt property to the current UTC time.
    /// </summary>
    public CoffeeShop()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
