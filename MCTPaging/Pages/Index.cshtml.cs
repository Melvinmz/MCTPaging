using MCTPaging.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCTPaging.Model;
using System.Text;

namespace MCTPaging.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDBContext _db;
        public PagingData PagingData { get; set; }
        [BindProperty]
        public IList<Employee> Employees { get; set; }
        private const int PageSize =10;
    
        public IndexModel(ILogger<IndexModel> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public void OnGet(int PageNum = 1)
        {
            Employees = _db.Employee.ToList<Employee>();
            StringBuilder QParam = new StringBuilder();
            if (PageNum!=0)
            {
                QParam.Append($"/Index?PageNum=-");

            }
           
            if (Employees.Count > 0)
            {
                PagingData = new PagingData
                {
                    CurrentPage = PageNum,
                    RecordsPerPage = PageSize,
                    TotalRecords = Employees.Count(),
                    UrlParams = QParam.ToString(),
                    LinksPerPage =7
                };
                Employees = Employees.Skip((PageNum - 1) * PageSize)
               .Take(PageSize).ToList();
                
            }
        }
    }
}
