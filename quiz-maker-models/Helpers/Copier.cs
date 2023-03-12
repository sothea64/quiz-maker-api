using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.Helpers
{
    public static class Copier
    {
        public static TDestination CopyTo<TSource, TDestination>(TSource source, TDestination destination)
        {
            var sources = source.GetType().GetProperties();
            var destinations = destination.GetType().GetProperties();
            foreach (var destPi in destinations
                .Where(x => sources.Any(s => s.Name == x.Name && s.PropertyType == x.PropertyType && x.GetSetMethod() != null)))
            {
                var sourceApi = sources.FirstOrDefault(x => x.Name == destPi.Name);
                var value = sourceApi.GetValue(source);
                destPi.SetValue(destination, value);
            }
            return destination;
        }

        public static TDestination CopyTo<TSource, TDestination>(TSource source, TDestination destination, params string[] excludeColums)
        {
            var sources = source.GetType().GetProperties();
            var destinations = destination.GetType().GetProperties();
            foreach (var destPi in destinations
                .Where(x => sources.Any(s => s.Name == x.Name && s.PropertyType == x.PropertyType && x.GetSetMethod() != null)))
            {
                if (excludeColums.Contains(destPi.Name))
                {
                    continue;
                }
                var sourceApi = sources.FirstOrDefault(x => x.Name == destPi.Name);
                var value = sourceApi.GetValue(source);
                destPi.SetValue(destination, value);
            }
            return destination;
        }

        //public static void CopyTo<T>(this T objSource, T objDes)
        //{
        //    if (objDes == null)
        //    {
        //        objDes = Activator.CreateInstance<T>();
        //    }
        //    // Retrieve the Type passed into the Method
        //    Type _impliedType = typeof(T);

        //    //Get an array of the Type’s properties
        //    PropertyInfo[] _propInfo = _impliedType.GetProperties();

        //    //Create the columns in the DataTable
        //    foreach (PropertyInfo pi in _propInfo)
        //    {
        //        pi.SetValue(objDes, pi.GetValue(objSource, null), null);
        //    }
        //}
    }
}
