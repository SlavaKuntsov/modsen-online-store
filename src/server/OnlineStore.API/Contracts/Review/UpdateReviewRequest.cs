namespace OnlineStore.API.Contracts.Review;

public record UpdateReviewRequest(
		int Rating,
		string Comment);