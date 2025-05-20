
namespace Core.DTOs;

public class CreateUserDto
{
    public required string name { get; set; }
    public required string last_name { get; set; }
    public required string identification_document { get; set; }
    public required string country { get; set; }
    public required string city { get; set; }
    public string? address { get; set; }
    public required string password { get; set; }
    public required string email { get; set; }
    public string? phone_number { get; set; }

}

public class UpdateUserDto
{
    public string? name { get; set; }
    public string? last_name { get; set; }
    public string? identification_document { get; set; }
    public string? country { get; set; }
    public string? city { get; set; }
    public string? address { get; set; }
    public string? phone_number { get; set; }

}

public class LoginUserDto
{
    public required string email { get; set; }
    public required string password { get; set; }
    public int? branchId { get; set; }

}

public class ResetPasswordDto
{
    public required string verification_code { get; set; }
    public required string password { get; set; }

}

public class ChangeStatusDto
{
    public required string status { get; set; }
}

public class ChangeRoleDto
{
    public required string role { get; set; }
}

public class VerifyUserDto
{
    public required string verification_code { get; set; }
    public required string email { get; set; }  

}