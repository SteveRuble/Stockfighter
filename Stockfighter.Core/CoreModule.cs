using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Ninject;
using Ninject.Components;
using Ninject.Extensions.Conventions;
using Ninject.Infrastructure;
using Ninject.Modules;
using Ninject.Planning.Bindings;
using Ninject.Planning.Bindings.Resolvers;

namespace Stockfighter.Core
{
    public class CoreModule : NinjectModule
    {


        public override void Load()
        {
            BindMediatr();

            Kernel.Bind(scan => scan.FromThisAssembly()
                                    .SelectAllClasses()
                                    .InheritedFrom(typeof (IAsyncRequestHandler<,>))
                                    .BindAllInterfaces());
        }

        private void BindMediatr()
        {
            Kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();
            Kernel.Bind(scan => scan.FromAssemblyContaining<IMediator>()
                                    .SelectAllClasses()
                                    .BindDefaultInterface()
                                    .Configure(c => c.InSingletonScope()));

            Bind<TextWriter>().ToConstant(Console.Out);
            Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
            Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));
        }

        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
                                                                               {
                                                                                   ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                                                                                   Converters = {
                                                                                                    new Newtonsoft.Json.Converters.StringEnumConverter(),
                                                                                                }
                                                                               };
    }

    public class ContravariantBindingResolver : NinjectComponent, IBindingResolver
    {
        /// <summary>
        /// Returns any bindings from the specified collection that match the specified service.
        /// </summary>
        public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, Type service)
        {
            if (service.IsGenericType)
            {
                var genericType = service.GetGenericTypeDefinition();
                var genericArguments = genericType.GetGenericArguments();
                if (genericArguments.Count() == 1
                 && genericArguments.Single().GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant))
                {
                    var argument = service.GetGenericArguments().Single();
                    var matches = bindings.Where(kvp => kvp.Key.IsGenericType
                                                           && kvp.Key.GetGenericTypeDefinition().Equals(genericType)
                                                           && kvp.Key.GetGenericArguments().Single() != argument
                                                           && kvp.Key.GetGenericArguments().Single().IsAssignableFrom(argument))
                        .SelectMany(kvp => kvp.Value);
                    return matches;
                }
            }

            return Enumerable.Empty<IBinding>();
        }
    }
}
