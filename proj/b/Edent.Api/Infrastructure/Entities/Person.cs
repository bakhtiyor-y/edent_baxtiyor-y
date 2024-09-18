using System;
using System.ComponentModel.DataAnnotations;
using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.Infrastructure.Entities
{
    public class Person : Entity
    {
        [StringLength(256)]
        public string FirstName { get; set; }

        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string Patronymic { get; set; }
        public DateTimeOffset BirthDate { get; set; }

        public Gender Gender { get; set; }
    }
}
