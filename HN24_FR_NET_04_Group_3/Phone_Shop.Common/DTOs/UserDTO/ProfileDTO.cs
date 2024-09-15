namespace Phone_Shop.Common.DTOs.UserDTO
{
    public class ProfileDTO
    {
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
    }
}
