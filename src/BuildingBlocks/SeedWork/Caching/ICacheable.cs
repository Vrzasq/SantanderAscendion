namespace SeedWork.MediatR.Caching;

public interface ICacheable
{
    string Key { get; }

    TimeSpan ExpirationTime { get; }
}
