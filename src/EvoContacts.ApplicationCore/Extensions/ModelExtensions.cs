using EvoContacts.ApplicationCore.Entities;
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
        public static bool MapUpdatesToEntity<TEntity>(this IBaseUpdateModel updateModel, ref TEntity entity)
        {
            bool isEntityModified = false;

            //loop through all properties in the UpdateModel and update Entity if required
            foreach (var modelProp in updateModel.GetType().GetProperties().Where(x => x.Name != "Id" && x.Name != "UpdatedUserId"))
            {
                var entityProp = entity.GetType().GetProperties().FirstOrDefault(x => x.Name == modelProp.Name);

                //same property exists on both UpdateModel and Entity
                if (entityProp != null)
                {
                    var previousValue = entityProp.GetValue(entity, null);
                    var updatedValue = modelProp.GetValue(updateModel, null);

                    //compare Entity property value to UpdateModel property value
                    if ((previousValue != null && updatedValue == null) || (previousValue == null && updatedValue != null) || (previousValue != null && !previousValue.Equals(updatedValue)))
                    {
                        entity.GetType().GetProperty(entityProp.Name).SetValue(entity, updatedValue);
                        isEntityModified = true;
                    }
                }
            }

            if (isEntityModified)
            {
                var entityUpdatedUserIdProp = entity.GetType().GetProperties().First(x => x.Name == "UpdatedUserId");

                var modelUpdatedUserIdProp = updateModel.GetType().GetProperties().FirstOrDefault(x => x.Name == entityUpdatedUserIdProp.Name);
                var updatedUserId = modelUpdatedUserIdProp.GetValue(updateModel, null);

                //set Entity.UpdatedUserId property value = UpdateModel.UpdatedUserId property value
                entity.GetType().GetProperty(entityUpdatedUserIdProp.Name).SetValue(entity, updatedUserId);
            }

            return isEntityModified;
        }
    }
}
