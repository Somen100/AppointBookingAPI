using Appointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.DAL
{
    public interface IResourceRepo
    {
        List<Resources> GetAllResources();
        Resources GetResourceById(int resourceId);
        Resources CreateResource(Resources resource);
        Resources UpdateResource(Resources resource);
        bool DeleteResource(int resourceId);
    }
}
