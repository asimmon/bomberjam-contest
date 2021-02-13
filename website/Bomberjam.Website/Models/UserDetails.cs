namespace Bomberjam.Website.Models
{
    public class UserDetails : User
    {
        public UserDetails(User user, PaginationModel<GameInfo> games)
        {
            this.User = user;
            this.Games = games;
        }

        public User User { get; }

        public PaginationModel<GameInfo> Games { get; }
    }
}