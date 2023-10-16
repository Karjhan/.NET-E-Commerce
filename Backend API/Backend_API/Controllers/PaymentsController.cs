using Backend_API.Entities;
using Backend_API.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Order = Backend_API.Entities.OrderAggregate.Order;

namespace Backend_API.Controllers;

public class PaymentsController : BaseAPIController
{
    private readonly IPaymentService _paymentService;
    
    private readonly ILogger<PaymentsController> _logger;

    private string WebhookSecret = "whsec_adceb14d1d0c0ddc65974dcb032a26455e85ad4de48a35d1264f6853a6932a4d";

    public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("{basketId}")]
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
    {
        var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
        if (basket == null)
        {
            return BadRequest(new APIResponse(400, "Problem with your basket"));
        }
        return Ok();
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WebhookSecret);
        PaymentIntent intent;
        Order order;
        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                intent = (PaymentIntent)stripeEvent.Data.Object;
                _logger.LogInformation("Payment succeeded: ", intent.Id);
                order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                _logger.LogInformation("Order updated, or payment succeeded!");
                break;
            case "payment_intent.payment_failed":
                intent = (PaymentIntent)stripeEvent.Data.Object;
                _logger.LogInformation("Payment failed: ", intent.Id);
                order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                _logger.LogInformation("Order updated, or payment failed!");
                break;
        }
        return new EmptyResult();
    }
}