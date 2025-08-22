namespace OnlineStore.API.Contracts.Review;

public record CreateReviewRequest(
        Guid ProductId,
        int Rating,
        string Comment);
