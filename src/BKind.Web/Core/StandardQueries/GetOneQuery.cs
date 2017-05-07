using System;
using System.Linq.Expressions;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Core.StandardQueries
{
    public class GetOneQuery<T> : IRequest<T> where T : Entity
    {
        public GetOneQuery(Expression<Func<T, bool>> condition, Expression<Func<T, object>> include = null)
        {
            Condition = condition;
            Include = include;
        }

        public Expression<Func<T, bool>> Condition { get; }

        public Expression<Func<T, object>> Include { get; set; }
    }
}