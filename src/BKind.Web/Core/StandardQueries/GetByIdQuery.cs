using System;
using System.Linq.Expressions;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Core.StandardQueries
{
    public class GetByIdQuery<T> : IRequest<T> where T:Entity 
    {
        public GetByIdQuery(int id, Expression<Func<T, object>> include = null)
        {
            Id = id;
            Include = include;
        }

        public int Id { get; }

        public Expression<Func<T, object>> Include { get; set; }
    }
}