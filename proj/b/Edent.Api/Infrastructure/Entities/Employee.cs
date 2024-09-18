namespace Edent.Api.Infrastructure.Entities
{
    public class Employee : Person
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
