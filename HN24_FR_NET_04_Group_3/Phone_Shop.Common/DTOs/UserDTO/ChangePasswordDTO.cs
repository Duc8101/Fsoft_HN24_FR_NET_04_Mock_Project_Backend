namespace Phone_Shop.Common.DTOs.UserDTO
{
    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;
    }
}
