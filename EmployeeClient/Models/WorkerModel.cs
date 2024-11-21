using EmployeeContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.Models
{
    public class WorkerModel
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime Birthday { get; set; }
        public string Sex { get; set; }
        public bool HasChildren { get; set; }

        public WorkerModel() { }

        public WorkerModel(WorkerMessage worker)
        {
            Id = worker.Id;
            LastName = worker.LastName;
            FirstName = worker.FirstName;
            MiddleName = worker.MiddleName;
            Birthday = DateTimeOffset.FromUnixTimeMilliseconds(worker.Birthday).DateTime;
            Sex = worker.Sex == EmployeeContracts.Sex.Male ? "Male" : "Female";
            HasChildren = worker.HasChildren;
        }

        public WorkerMessage ToGrpcMessage()
        {
            return new WorkerMessage
            {
                Id = Id,
                LastName = LastName,
                FirstName = FirstName,
                MiddleName = MiddleName,
                Birthday = new DateTimeOffset(Birthday).ToUnixTimeMilliseconds(),
                Sex = Sex == "Male" ? EmployeeContracts.Sex.Male : EmployeeContracts.Sex.Female,
                HasChildren = HasChildren
            };
        }
    }
}
