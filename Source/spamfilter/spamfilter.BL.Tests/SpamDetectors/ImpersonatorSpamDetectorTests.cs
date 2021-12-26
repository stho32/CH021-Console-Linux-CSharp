using MailKit;
using spamfilter.BL.Entities;
using spamfilter.BL.SpamDetectors;
using spamfilter.Interfaces;
using Xunit;

namespace spamfilter.BL.Tests.SpamDetectors;

public class ImpersonatorSpamDetectorTests
{
    private ISpamDetector SpamDetector()
    {
        var detector = new ImpersonatorSpamDetector("Amazon", new string[]
        {
            "amazon.com",
            "amazon.de"
        });

        return detector;
    }
    
    [Fact]
    public void A_mail_from_amazon_is_ok_if_it_is_from_amazon_de()
    {
        var mail = new Email(
            "Amazon",
            "amazon@amazon.de",
            UniqueId.Invalid,
            "Your orders",
            "Some stuff");
        
        Assert.Empty(SpamDetector().GetOpinionsOn(mail));
    }
    
    [Fact]
    public void A_mail_from_amazon_is_ok_if_it_is_from_amazon_com()
    {
        var mail = new Email(
            "Amazon",
            "amazon@amazon.com",
            UniqueId.Invalid,
            "Your orders",
            "Some stuff");
        
        Assert.Empty(SpamDetector().GetOpinionsOn(mail));
    }
    
    [Fact]
    public void A_mail_from_amazon_is_NOT_ok_if_domain_is_wrong()
    {
        var mail = new Email(
            "Amazon",
            "amazon@someimpersonator.com",
            UniqueId.Invalid, 
            "Your orders",
            "some stuff");

        var result = SpamDetector().GetOpinionsOn(mail);
        Assert.Single(result);
        Assert.True(result[0].IsSpam);
    }
}