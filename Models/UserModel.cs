using Models.Enums;

namespace Models;

public record UserModel(
    int Id,
    string Username,
    string PasswordHash,
    Role Role);