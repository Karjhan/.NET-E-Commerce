namespace Backend_API.Helpers;

public class StartupHosted : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        CommandRunner.StartCmd("D:\\Programs\\StripeCommandLine", "stripe listen -f https://localhost:5001/api/payments/webhook -e payment_intent.succeeded,payment_intent.payment_failed");
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        CommandRunner.StopCmd();
        
        return Task.CompletedTask;
    }
}