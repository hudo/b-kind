using System.Collections.Generic;
using MediatR;

namespace BKind.Web.Features.Stories.Contracts
{
    public class GetTagCloudQuery : IRequest<List<(string tag, int count)>>
    {
        
    }
}