namespace Bakery.Service
{
    public interface IVoteService
    {
        void SetVote(string userId, int productId, byte value);

        double GetAverage(int productId);

        int GetValue(string userId, int productId);

    }
}
