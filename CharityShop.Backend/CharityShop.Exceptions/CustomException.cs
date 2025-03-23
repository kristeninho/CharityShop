namespace CharityShop.Exceptions;
/// <summary>
/// Exception wrapper for throwing and catching custom Exceptions
/// </summary>
public class CustomException(string message) : Exception(message);