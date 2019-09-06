using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LisasTours.Models
{
    /// <summary>
    /// Направление деятельности компании (для связи многие ко многим в БД)
    /// </summary>
    public class CompanyBusinessLine
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int BusinessLineId { get; set; }
        public BusinessLine BusinessLine { get; set; }
    }
}
