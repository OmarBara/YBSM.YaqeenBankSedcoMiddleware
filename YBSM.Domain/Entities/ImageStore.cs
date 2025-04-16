using System;
using Core.Domain.Enum;

namespace Core.Domain.Entities
{
    public class ImageStore
    {
        public ImageStore()
        {
        }

        public Guid Id { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public ImageType ImageType { get; set; }
        public DateTime? CreatedOn { get; set; }

        public Guid OwnerId { get; set; }
    }
}