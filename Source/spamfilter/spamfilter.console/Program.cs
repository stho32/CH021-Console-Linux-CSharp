﻿using spamfilter.BL.Environment;
using spamfilter.BL.ExtensionMethods;
using spamfilter.BL.Rules;
using spamfilter.Infrastructure.Environment;
using spamfilter.Infrastructure.Repositories;

var environment = new EnvironmentFactory(
    new ConsoleLogger("Main"),
    new ConsoleConfiguration()
    );

var server = environment.GetConfiguration().GetConfigurationValue("Server");
var username = environment.GetConfiguration().GetConfigurationValue("Username");
var password = environment.GetConfiguration().GetConfigurationValue("Password");

var emailRepository = new ImapEmailRepository(server??"", username??"", password??"", environment);

var logger = environment.GetLogger("main");

while (true)
{
    logger.Log("Filter going to work...");

    var factory = new ExampleRuleFactory(emailRepository, environment);
    
    var rules = factory.Create();
    var actions = rules.Execute(emailRepository.GetInboxContent());
    actions.Execute(emailRepository);
    
    logger.Log("Waiting 5 Minutes to restart process...");
    System.Threading.Thread.Sleep(5000*60);
}
