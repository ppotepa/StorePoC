﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Core.Binders
{
    public interface IEntityBinder<TRequest> where TRequest : IBaseRequest
    {
        Task<Unit> Attach(TRequest request, CancellationToken cancelationToken);
    }
}
