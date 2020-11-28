using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testTask
{
    public class Line
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public decimal Allowance1 { get; set; }
        public decimal Allowance2 { get; set; }
        public decimal Allowance3 { get; set; }
        public decimal Salary { get; set; }
        public decimal Sum
        {
            get
            {
                return Salary + Allowance1 + Allowance2 + Allowance3;
            }
        }
        public string Position { get; set; }
        public string StaffListNumber { get; set; }
        public string Something { get; set; }
    }
}
