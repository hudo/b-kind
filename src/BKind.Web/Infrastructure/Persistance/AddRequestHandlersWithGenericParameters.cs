using System;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Infrastructure.Persistance.StandardHandlers;
using BKind.Web.Model;
using MediatR;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

namespace BKind.Web.Infrastructure.Persistance
{
    public class AddRequestHandlersWithGenericParameters : IRegistrationConvention
    {
        public void ScanTypes(TypeSet types, Registry registry)
        {
            foreach (var responseType in types.FindTypes(TypeClassification.Concretes))
            {
                if (typeof(Identity).IsAssignableFrom(responseType))
                {
                    Register(registry, typeof(GetByIdQuery<>), typeof(GetByIdHandler<>), responseType, responseType);
                    Register(registry, typeof(GetOneQuery<>), typeof(GetOneHandler<>), responseType, responseType);
                    Register(registry, typeof(GetAllQuery<>), typeof(GetAllHandler<>), responseType, typeof(PagedList<>).MakeGenericType(responseType));
                }
                else if (typeof(Entity).IsAssignableFrom(responseType))
                {
                    Register(registry, typeof(GetOneQuery<>), typeof(GetOneHandler<>), responseType, responseType);
                    Register(registry, typeof(GetAllQuery<>), typeof(GetAllHandler<>), responseType, typeof(PagedList<>).MakeGenericType(responseType));
                }
            }
        }

        private void Register(Registry registry, Type genericRequest, Type genericHandler, Type concreteClass, Type handlerOutputType)
        {
            var genericCommand = genericRequest.MakeGenericType(concreteClass);
            var interfaceHandlerType = typeof(IAsyncRequestHandler<,>).MakeGenericType(genericCommand, handlerOutputType);
            var concreteHandlerType = genericHandler.MakeGenericType(concreteClass);
            registry.For(interfaceHandlerType).Use(concreteHandlerType);
        }
    }
}