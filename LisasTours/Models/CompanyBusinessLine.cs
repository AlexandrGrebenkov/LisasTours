using LisasTours.Models.Base;

namespace LisasTours.Models
{
    /// <summary>
    /// Направление деятельности компании (для связи многие ко многим в БД)
    /// </summary>
    public class CompanyBusinessLine : Entity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int BusinessLineId { get; set; }
        public BusinessLine BusinessLine { get; set; }
    }
}
