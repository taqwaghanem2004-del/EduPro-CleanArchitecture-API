using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPro.Data.Entities
{
    public class CollectionFile
    {
        public int CollectionId { get; set; }
        public int EduFileId { get; set; }
        public int Order { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.Now;
        public virtual Collection Collection { get; set; }
        public virtual EduFile EduFile { get; set; }
    }
}
