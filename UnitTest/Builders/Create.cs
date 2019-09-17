using LisasTours.Models;
using LisasTours.Models.Base;

namespace UnitTests.Builders
{
    /// <summary>Mother Of Objects</summary>
    static class Create
    {
        #region Builders

        public static CompanyBuilder Company(string name)
        {
            return new CompanyBuilder().CreateCompany(name);
        }

        #endregion

        #region Entites

        static int id;
        /// <summary>Создание сущности с уникальным Id</summary>
        public static T Entity<T>() where T : Entity, new()
        {
            return new T() { Id = ++id };
        }

        /// <summary>Создание уникальной сущности с именем</summary>
        public static T NamedEntity<T>(string name) where T : NamedEntity, new()
        {
            var entity = Entity<T>();
            entity.Name = name;
            return entity;
        }

        #endregion

        #region Extension methods

        public static Affiliate WithRegion(this Affiliate affiliate, Region region)
        {
            affiliate.Region = region;
            affiliate.RegionId = region.Id;
            return affiliate;
        }

        public static CompanyBusinessLine WithBusinessLine(this CompanyBusinessLine companyBusinessLine, BusinessLine businessLine)
        {
            companyBusinessLine.BusinessLine = businessLine;
            companyBusinessLine.BusinessLineId = businessLine.Id;
            return companyBusinessLine;
        }

        #endregion
    }
}
