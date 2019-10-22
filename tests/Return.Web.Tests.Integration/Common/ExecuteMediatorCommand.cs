﻿// ******************************************************************************
//  © 2019 Sebastiaan Dammann | damsteen.nl
// 
//  File:           : ExecuteMediatorCommand.cs
//  Project         : Return.Web.Tests.Integration
// ******************************************************************************

namespace Return.Web.Tests.Integration.Common {
    using System;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Abstractions;
    using Application.Common.Models;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public static class TestServiceScopeUtilities {
        public static void SetAuthenticationInfo(this IServiceScope serviceScope, CurrentParticipantModel currentParticipant) {
            var currentParticipantService = (CurrentParticipantService)
                serviceScope.ServiceProvider.GetRequiredService<ICurrentParticipantService>();

            currentParticipantService.SetHttpContext(new DefaultHttpContext {
                User = new ClaimsPrincipal()
            });

            currentParticipantService.SetParticipant(currentParticipant);
        }

        public static void SetNoAuthenticationInfo(this IServiceScope serviceScope) {
            var currentParticipantService = (CurrentParticipantService)
                serviceScope.ServiceProvider.GetRequiredService<ICurrentParticipantService>();
            currentParticipantService.SetNoHttpContext();
        }

        public static Task<TResponse> Send<TResponse>(
            this IServiceScope serviceScope,
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default
        ) {
            IServiceProvider sp = serviceScope.ServiceProvider;
            return sp.Send(request, cancellationToken);
        }

        public static Task<TResponse> Send<TResponse>(
            this IServiceProvider serviceProvider,
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default
        ) {
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            return mediator.Send(request, cancellationToken);
        }
    }
}
