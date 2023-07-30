using Appointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.BAL
{
    public interface IResourceService
    {
        List<Resources> GetAllResources();
        Resources GetResourceById(int resourceId);
        Resources CreateResource(Resources resource);
        Resources UpdateResource(Resources resource);
        bool DeleteResource(int resourceId);
    }
}
