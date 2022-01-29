using spamfilter.BL.Detectors;
using spamfilter.Interfaces;
using spamfilter.Interfaces.Environment;

namespace spamfilter.BL.Rules;

public class Rule : IRule
{
    private readonly IActionFactory _actionFactory;
    private readonly DetectorGroup _detectors;

    public Rule(
        IDetector[] detectors,
        IEnvironmentFactory environmentFactory,
        IActionFactory actionFactory)
    {
        _actionFactory = actionFactory;
        _detectors = new DetectorGroup(detectors, environmentFactory);
    }

    private IDetectionResult[] DetectInInbox(IEmail[] mails)
    {
        var result = _detectors.Filter(mails);

        return result;
    }

    public IAction[] Execute(IEmail[] emails)
    {
        var possiblyMatches = DetectInInbox(emails);
        var matches = possiblyMatches
            .Where(x => x.IsSpam)
            .Select(x=> x.Email).ToArray();

        var result = new List<IAction>();

        foreach(var match in matches) 
        {
            result.Add(_actionFactory.CreateFromEmail(match));        
        }

        return result.ToArray();
    }
}