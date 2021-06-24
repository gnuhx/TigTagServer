using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApplicationCore.Extensions
{
    public static class DataReaderExtensions
    {
        public static List<T> MapToList<T>(this DbDataReader dr) where T : new()
        {
            var entities = new List<T>();
            if (dr != null && dr.HasRows)
            {
                var entity = typeof(T);

                var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var propDist = props.ToDictionary(p => p.Name.ToUpper(), p => p);
                while (dr.Read())
                {
                    T newObject = new T();
                    for (int index = 0; index < dr.FieldCount; index++)
                    {
                        if (propDist.ContainsKey(dr.GetName(index).ToUpper()))
                        {
                            var info = propDist[dr.GetName(index).ToUpper()];
                            if ((info != null) && info.CanWrite)
                            {
                                var val = dr.GetValue(index);
                                info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                            }
                        }
                    }
                    entities.Add(newObject);
                }
                return entities;
            }
            return entities;
        }
    }
}
