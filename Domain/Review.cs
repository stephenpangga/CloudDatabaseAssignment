using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Review
    {
        public Guid ReviewId {get; set; }

        public Guid ProductId { get; set; }

        public string Comments { get; set; }

        //Nullable user id here 

        public string PartitionKey { get; set; }

        public Review()
        {

        }

    }
}
