using System.Reflection;
using System.Runtime.Serialization;
using StacyClouds.C4Sharp.Dsl;
using Xunit;

namespace StacyClouds.C4Sharp.Core.Tests.Dsl
{
    public class DslIdGeneratorTests
    {
        [Fact]
        public void GenerateId_uses_canonical_name_and_suffixes_duplicate_values()
        {
            DslIdGenerator generator = new DslIdGenerator();
            Person person = CreatePerson("Admin  User");

            Assert.Equal("person-admin-user", generator.GenerateId(person));
            Assert.Equal("person-admin-user-2", generator.GenerateId(person));
        }

        [Fact]
        public void Found_marks_an_id_as_unavailable_for_future_generation()
        {
            DslIdGenerator generator = new DslIdGenerator();
            Person person = CreatePerson("Admin  User");

            generator.Found("person-admin-user");

            Assert.Equal("person-admin-user-2", generator.GenerateId(person));
        }

        [Fact]
        public void GenerateId_uses_relationship_details_and_fallbacks()
        {
            DslIdGenerator generator = new DslIdGenerator();
            Relationship relationship = CreateRelationship("user-id", "system-id", "Uses");
            Relationship emptyRelationship = CreateRelationship(null, null, null);

            Assert.Equal("user-id-system-id-uses", generator.GenerateId(relationship));
            Assert.Equal("relationship", generator.GenerateId(emptyRelationship));
        }

        private static Person CreatePerson(string name)
        {
            Person person = (Person)FormatterServices.GetUninitializedObject(typeof(Person));
            SetProperty(person, "Name", name);
            return person;
        }

        private static Relationship CreateRelationship(string sourceId, string destinationId, string description)
        {
            Relationship relationship = (Relationship)FormatterServices.GetUninitializedObject(typeof(Relationship));

            if (sourceId != null)
            {
                Person source = CreatePerson("Source");
                SetProperty(source, "Id", sourceId);
                relationship.Source = source;
            }

            if (destinationId != null)
            {
                SoftwareSystem destination = (SoftwareSystem)FormatterServices.GetUninitializedObject(typeof(SoftwareSystem));
                SetProperty(destination, "Name", "Destination");
                SetProperty(destination, "Id", destinationId);
                relationship.Destination = destination;
            }

            if (description != null)
            {
                SetProperty(relationship, "Description", description);
            }

            return relationship;
        }

        private static void SetProperty(object target, string propertyName, object value)
        {
            PropertyInfo property = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            property.GetSetMethod(true).Invoke(target, new[] { value });
        }
    }
}
