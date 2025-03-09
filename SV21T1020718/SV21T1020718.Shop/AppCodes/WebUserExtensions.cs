using System.Security.Claims;

namespace SV21T1020718.Shop
{
    /// <summary>
    /// Tạo thêm phương thức (hàm) mở rộng cho Principal để lấy thông tin
    /// của người dùng dựa trên cookie
    /// </summary>
    public static class WebUserExtensions
    {
        public static WebUserData GetUserData(this ClaimsPrincipal pricipal)
        {
            try
            {
                if (pricipal == null || pricipal.Identity == null || !pricipal.Identity.IsAuthenticated)
                    return null;

                var userData = new WebUserData();

                userData.UserId = pricipal.FindFirstValue(nameof(userData.UserId)) ?? "";
                userData.UserName = pricipal.FindFirstValue(nameof(userData.UserName)) ?? "";
                userData.DisplayName = pricipal.FindFirstValue(nameof(userData.DisplayName)) ?? "";
                userData.Photo = pricipal.FindFirstValue(nameof(userData.Photo)) ?? "";

                userData.Roles = new List<string>();
                foreach (var item in pricipal.FindAll(ClaimTypes.Role))
                {
                    userData.Roles.Add(item.Value);
                }

                return userData;
            }
            catch
            {
                return null;
            }
        }
    }
}