using WebResilencePolly.Models;

namespace WebResilencePolly.Service
{
    public interface IGithubService
    {
        Task<dynamic> GetUserByUsernameAsync(string username);

        Task<List<GithubUser>?> GetUserFromOrgAsync(string orgName);
    }
}