namespace CharityShop.Contracts.Dtos;

/// <inheritdoc cref="MultipleProductsResult.HealthStatus" />
public class HealthStatus
{
    /// <summary>
    /// Default constructor for Successful request
    /// </summary>
    public HealthStatus()
    {
        Success = true;
    }
    /// <summary>
    /// Constructor for when an Error occurs
    /// </summary>
    /// <param name="errorMessage"></param>
    public HealthStatus(string errorMessage)
    {
        Success = false;
        ErrorMessage = errorMessage;
    }
    /// <summary>
    /// Message describing the problem, if one occured
    /// </summary>
    public string ErrorMessage { get; set; }
    /// <summary>
    /// Whether the request was successful
    /// </summary>
    public bool Success { get; set; } = true;
}