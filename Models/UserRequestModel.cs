namespace Models;

public record UserRequestModel(
    string Username,
    string PasswordHash);