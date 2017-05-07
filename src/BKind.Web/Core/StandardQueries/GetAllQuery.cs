using System;
using System.Linq.Expressions;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Core.StandardQueries
{
    public class GetAllQuery<T> : IRequest<PagedList<T>> where T : Entity
    {
        public GetAllQuery(Expression<Func<T, bool>> @where, PagedOptions<T> pageOption = null, Expression<Func<T, object>> include = null)
        {
            Where = @where;
            PageOption = pageOption;
            Include = include;
        }

        public GetAllQuery(PagedOptions<T> pageOption = null)
        {
            PageOption = pageOption;
        }

        public Expression<Func<T, bool>> Where { get; set; }
        public PagedOptions<T> PageOption { get; set; }
        public Expression<Func<T, object>> Include { get; set; }
    }
}