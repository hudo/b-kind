using System.Collections.Generic;
using MediatR;

namespace BKind.Web.Features.Stories.Queries
{
    public class GetTagCloudQuery : IRequest<List<(string tag, int count)>>
    {
        
    }
}