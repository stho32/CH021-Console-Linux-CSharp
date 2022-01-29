namespace spamfilter.Interfaces;

/// <summary>
/// Is this message spam or not?
/// </summary>
public interface IIsMatchOpinion
{
    int Priority { get; }
    string Reasoning { get; }
    string DetectorName { get; }
    bool IsSpam { get; }
}