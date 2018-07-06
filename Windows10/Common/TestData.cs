using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows10.Common
{
    public class TestData
    {
        /// <summary>
        /// 返回一个 Employee 数据集合，可用于测试
        /// </summary>
        public static ObservableCollection<Employee> GetEmployees(int employeeCount = 100)
        {
            var employees = new ObservableCollection<Employee>();

            for (int i = 0; i < employeeCount; i++)
            {
                employees.Add(
                    new Employee
                    {
                        Name = "Name " + i.ToString(),
                        Age = new Random(i).Next(20, 60),
                        IsMale = Convert.ToBoolean(i % 2)
                    });
            }

            return employees;
        }
    }
}
