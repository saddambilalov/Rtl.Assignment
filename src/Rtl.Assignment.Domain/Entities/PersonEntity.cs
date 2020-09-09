using System;

namespace Rtl.Assignment.Domain.Entities
{
    public class PersonEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Birthday { get; set; }
    }
}