﻿using Domain.Contracts.Extensions;
using Domain.Contracts.Payment;
using Domain.Core.Abstractions.Stream;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Transfers;
using Payment.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Payment.API.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly ISendEndpointProvider _endpointProvider;
    private readonly IProcessEventStream<PaymentEventStream> _processEvent;

    public PaymentController(ISendEndpointProvider endpointProvider,
        IProcessEventStream<PaymentEventStream> processEvent)
    {
        _endpointProvider = endpointProvider;
        _processEvent = processEvent;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]    
    public async Task<PaymentResponse> SendPayment(
        [FromBody, Required] PaymentTransferCommand payment)
    {
        Guid paymentId = Guid.NewGuid();

        PaymentCommand paymentcommand = new PaymentCommand(paymentId, payment.Payer,
            payment.Payee, payment.Value);

        ISendEndpoint sendEndpoint = await _endpointProvider.GetSendEndpoint(paymentcommand.GetExchange());
        await sendEndpoint.Send(paymentcommand).ConfigureAwait(false);

        return new PaymentResponse(paymentId, "Pagamento está em processo");
    }


    [HttpGet("Status/{IdPayment}")]
    [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
    public async Task<PaymentStatus> GetPayment(Guid IdPayment)
    {
        PaymentEventStream payment = await _processEvent.Process(IdPayment);
        return new PaymentStatus(payment);
    }    
}


