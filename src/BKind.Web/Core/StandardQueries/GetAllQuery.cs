using System;
using System.Linq.Expressions;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Core.StandardQueries
{
    public class GetAllQuery<T> : IRequest<T[]> where T : Entity
    {
        public GetAllQuery(Expression<Func<T, bool>> @where, PagedOptions<T> pageOption = null)
        {
            Where = @where;
            PageOption = pageOption;
        }

        public Expression<Func<T, bool>> Where { get; set; }
        public PagedOptions<T> PageOption { get; set; }
    }
}