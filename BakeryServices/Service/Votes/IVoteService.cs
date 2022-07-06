namespace BakeryServices.Service.Votes
{
    public interface IVoteService
    {
        Task SetVote(string userId, int productId, byte value);

        double GetAverage(int productId);

        int GetValue(string userId, int productId);

    }
}
