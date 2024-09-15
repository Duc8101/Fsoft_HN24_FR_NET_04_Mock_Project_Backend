namespace Phone_Shop.Common.DTOs.UserDTO
{
    public class UserDetailDTO : ProfileDTO
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;
    }
}
