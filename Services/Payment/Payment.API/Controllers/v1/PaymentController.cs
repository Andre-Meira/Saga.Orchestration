﻿using Domain.Contracts.Extensions;
using Domain.Contracts.Payment;
using Domain.Core.Abstractions.Stream;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Payment.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Payment.API.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly ISendEndpointProvider _endpointProvider;
    private readonly IProcessEventStream<PaymentEventStream> _processEvent;

    public PaymentController(
        ISendEndpointProvider endpointProvider,
        IProcessEventStream<PaymentEventStream> processEvent)
    {
        _endpointProvider = endpointProvider;
        _processEvent = processEvent;
    }

    [HttpPost]
    public async Task<IActionResult> SendPayment([FromBody, Required] PaymentDTO payment)
    {
        Guid paymentId = Guid.NewGuid();

        PaymentCommand paymentcommand = new PaymentCommand(
            paymentId, payment.Payer,
            payment.Payee, payment.Value);

        ISendEndpoint sendEndpoint = await _endpointProvider.GetSendEndpoint(paymentcommand.GetExchange());
        await sendEndpoint.Send(paymentcommand).ConfigureAwait(false);

        return Ok(new
        {
            IdPayment = paymentId,
            Mensagem = "O pagamento esta em processo"
        });
    }


    [HttpGet("Status/{IdPayment}")]
    public async Task<PaymentEventStream> GetPayment(Guid IdPayment) 
        => await _processEvent.Process(IdPayment);

}

public record PaymentDTO
{
    public Guid Payer { get; set; }
    public Guid Payee { get; set; }
    public decimal Value { get; set; }
}
