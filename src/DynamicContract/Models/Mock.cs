using System;
using System.Collections.Generic;

namespace DynamicContract.Models
{
    public class Mock
    {
        public static List<Person> Persons = new List<Person>
        {
            new Person
            {
                Id = 1,
                FirstName = "K . M. Mahabub Ali",
                LastName = "Majumder",
                DOB = new DateTime(1971, 12, 16)
            }
        };
    }
}
