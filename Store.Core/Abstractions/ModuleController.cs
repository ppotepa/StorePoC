﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Domain.Context;
using Store.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Utilities.Abstractions
{

    /// <summary>
    /// Abstract controller.
    /// Devires from ControllerBase.
    /// Is being used in each of entities Modules.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class ModuleController : ControllerBase
    {
        protected readonly IMediator mediator;
        protected readonly IMapper mapper;
        protected readonly IApplicationContext context;

        private Dictionary<ShouldRedirect, Func<BaseResponse, IActionResult>> Actions => new Dictionary<ShouldRedirect, Func<BaseResponse, IActionResult>>()
        {
            [ShouldRedirect.ShouldNotRedirect]  = (response) => new ObjectResult(response),
            [ShouldRedirect.ShoulRedirect]      = (response) => Redirect(response.RedirectUrl),
        };

        protected ModuleController(IMediator mediator, IMapper mapper, IApplicationContext context)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.context = context;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Handle(IBaseRequest request)
        {
            BaseResponse response = await this.mediator.Send(request) as BaseResponse;
            ShouldRedirect shouldRedirect = string.IsNullOrEmpty(response.RedirectUrl) is false 
                ? ShouldRedirect.ShoulRedirect 
                : ShouldRedirect.ShouldNotRedirect;

            return Actions[shouldRedirect](response);
        }
    }

    internal enum ShouldRedirect 
    {
        ShouldNotRedirect, 
        ShoulRedirect 
    };
}
