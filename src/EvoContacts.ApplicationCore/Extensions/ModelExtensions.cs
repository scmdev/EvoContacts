using EvoContacts.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvoContacts.ApplicationCore.Extensions
{
    public static class ModelExtensions
    {
        /// <summary>
        /// Compares values of all UpdateModel properties and updates Entity properties as necessary
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="updateModel"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool MapToEntity<TEntity>(this IBaseUpdateModel updateModel, ref TEntity entity)
        {
            bool isEntityModified = false;

            foreach (var entityProp in entity.GetType().GetProperties().Where(x => x.Name != "UpdatedUserId" && x.Name != "UpdatedDateTimeOffset"))
            {
                var modelProp = updateModel.GetType().GetProperties().FirstOrDefault(x => x.Name == entityProp.Name);

                if (modelProp != null)
                {
                    var previousValue = entityProp.GetValue(entity, null);
                    var updatedValue = modelProp.GetValue(updateModel, null);
                    if ((previousValue != null && updatedValue == null) || (previousValue == null && updatedValue != null) || (previousValue != null && !previousValue.Equals(updatedValue)))
                    {
                        entity.GetType().GetProperty(entityProp.Name).SetValue(entity, updatedValue);
                        isEntityModified = true;
                    }
                }
            }

            if (isEntityModified)
            {
                foreach (var entityProp in entity.GetType().GetProperties().Where(x => x.Name == "UpdatedUserId" || x.Name == "UpdatedDateTimeOffset"))
                {
                    var modelProp = updateModel.GetType().GetProperties().FirstOrDefault(x => x.Name == entityProp.Name);
                    if (modelProp != null)
                    {
                        var updatedValue = modelProp.GetValue(updateModel, null);

                        entity.GetType().GetProperty(entityProp.Name).SetValue(entity, updatedValue);
                    }
                }
            }

            return isEntityModified;
        }
    }
}
