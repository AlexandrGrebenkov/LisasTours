using System.Collections.Generic;
using System.Threading.Tasks;
using LisasTours.Models;

namespace LisasTours.Services
{
    public interface IExporter
    {
        Task<byte[]> GenerateCompaniesReport(IEnumerable<Company> companies);
    }
}