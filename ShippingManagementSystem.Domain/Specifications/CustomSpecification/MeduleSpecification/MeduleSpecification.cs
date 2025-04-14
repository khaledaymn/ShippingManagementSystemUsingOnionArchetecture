using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Specifications;
using System;
using System.Linq;

namespace ShippingManagementSystem.Domain.Specifications.MeduleSpecification
{
    public class MeduleSpecification : BaseSpecification<Medule>
    {
        public MeduleSpecification(MeduleParams param) : base(x =>
            (string.IsNullOrEmpty(param.Search) || x.Name.ToLower().Contains(param.Search.ToLower())))
        {
            // Apply sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "name_asc":
                        ApplyOrderBy(x => x.Name);
                        break;
                    case "name_desc":
                        ApplyOrderByDescending(x => x.Name);
                        break;
                    default:
                        ApplyOrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                ApplyOrderBy(x => x.Id);
            }

            // Apply pagination
            ApplyPagination(param.PageIndex, param.PageSize);
        }
        
        public MeduleSpecification(int id) : base(x => x.Id == id)
        {
            // Constructor for getting medule by ID
        }
        
        public MeduleSpecification(string userId) : base()
        {
            // This specification will include filtering by user ID through GroupMedules
            // We need to manually apply the filtering in the service layer
        }
    }
} 