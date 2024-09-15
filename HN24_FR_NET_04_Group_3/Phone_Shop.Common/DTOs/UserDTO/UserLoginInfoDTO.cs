namespace Phone_Shop.Common.DTOs.UserDTO
{
    public class UserLoginInfoDTO
    {
        public string Token { get; set; } = null!;
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public DateTime ExpireDate { get; set; }
    }
}
