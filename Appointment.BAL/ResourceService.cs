using Appointment.DAL;
using Appointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.BAL
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepo _resourceRepo;

        public ResourceService(IResourceRepo resourceRepo)
        {
            _resourceRepo = resourceRepo;
        }
        public Resources CreateResource(Resources resource)
        {
            return _resourceRepo.CreateResource(resource);
        }

        public bool DeleteResource(int resourceId)
        {
            return _resourceRepo.DeleteResource(resourceId);
        }

        public List<Resources> GetAllResources()
        {
            return _resourceRepo.GetAllResources();
        }

        public Resources GetResourceById(int resourceId)
        {
            return _resourceRepo.GetResourceById(resourceId);
        }

        public Resources UpdateResource(Resources resource)
        {
            return _resourceRepo.UpdateResource(resource);
        }
    }
}
