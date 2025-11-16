using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlayon.Application.Model.BaseEntities;

namespace TaskPlayon.Core.Model;

public class Customer:AuditableEntity
{
   
    public string Name { get; set; }=string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public ICollection<Invoice> Invoices { get; set; }=new List<Invoice>();
}
