using System.Collections.Generic;

namespace PartialViewsExample.Services
{
    public class PersonProvider : IPersonProvider
    {
        public List<Person> PersonList { get; private set; }

        public PersonProvider()
        {
            PersonList = PersonInitializer();
        }

        private List<Person> PersonInitializer()
        {
            var personList = new List<Person>
            {
                new Person("Miguel", "Santos", "R. Falet, Rio de Janeiro", "+55-21-4647-82755"),
                new Person("Seo-yun", "Song", "Dorim-dong, Seoul", "+82-2-319-9284"),
                new Person("Berat", "Arslan", "Beştelsiz Mahallesi, Istanbul", " +90-211-229-82133"),
                new Person("Olivia", "Smith", "Oakwood Village, Toronto", " +1-416-336-87206"),
                new Person("Emily", "Jones", "Sycamore Ave, Los Angeles", "+1-213-416-11922"),
                new Person("Noam", "Cohen", "Shalom Aleichem, Tel Aviv", "+972-3-551-42811"),
                new Person("Lucas", "Martin", "Rue Christine, Brussels", "+32-2-261-44070"),
                new Person("Sofía", "Flores", "Los Capitanes, Santiago", "+56-2-2236-16694"),
                new Person("Louis", "Simon", "Rue Eugène Sue, Paris", "+33-1-423-005813"),
                new Person("Cheng", "Huáng", "Dong Lan Lu, Shanghai", "+86-591-2807-70245"),
                new Person("Oliver", "Taylor", "Cary St, Sydney", "+61-2-9241-02498"),
                new Person("Elias", "Meyer", "Neanderstraße, Hamburg", "+49-40-300-37-80992"),
                new Person("Maxim", "Petrov", "Leninsky Ave, Moscow", "+7-495-877-12-656"),
                new Person("William", "Jacobs", "Claremont, Cape Town", "+27-21-421-90168"),
                new Person("Alexander", "Jackson", "Walton St, New York", "+1-212-886-17581")
            };

            return personList;
        }
    }
}
