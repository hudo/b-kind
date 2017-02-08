using System;
using System.Linq.Expressions;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Core.StandardQueries
{
    public class GetOneQuery<T> : IRequest<T> where T : Entity
    {
        public GetOneQuery(Expression<Func<T, bool>> condition)
        {
            Condition = condition;
        }

        public Expression<Func<T, bool>> Condition { get; }
    }
}