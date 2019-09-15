using System.Collections.Generic;
using System.Linq;
using LisasTours.Data;
using LisasTours.Models.Base;

namespace LisasTours.Application.Commands
{
    public class DbHelper
    {
        protected readonly ApplicationDbContext Context;

        public DbHelper(ApplicationDbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Получение коллекции сущностей по именам
        /// </summary>
        /// <typeparam name="T">Тип доменной модели</typeparam>
        /// <param name="names">Список имён</param>
        /// <returns>Коллекция сущностей с установленным Id. Если сущности с таким именем нет в баще, то Id = 0</returns>
        public IEnumerable<T> GetNamedCollection<T>(IEnumerable<string> names)
            where T : NamedEntity, new()
        {
            var newCollection = names
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(str => new T() { Name = str })
                .ToList();
            var collectionFromDb = Context.Set<T>().ToList();

            // колхоз для получения Id по строке названия
            foreach (var item in newCollection)
            {
                var bb = collectionFromDb.FirstOrDefault(_ => _.Name == item.Name);
                if (bb != null)
                {
                    item.Id = bb.Id;
                }
            }
            return newCollection;
        }
    }
}
