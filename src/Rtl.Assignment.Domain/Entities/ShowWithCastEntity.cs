using System.Collections.Generic;

namespace Rtl.Assignment.Domain.Entities
{
    public class ShowWithCastEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<PersonEntity> Cast { get; set; }
    }
}