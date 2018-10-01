using AutoMapper;
using EvoContacts.ApplicationCore.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EvoContacts.ApplicationCore.Extensions
{
    public static class AutomapperExtensions
    {
        /// <summary>
        /// Automapper extension to ignore specified mapping 
        /// properties of the domain class, not available on the model.
        /// http://stackoverflow.com/a/16808867
        /// </summary>
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }

}
