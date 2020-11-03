namespace ContactViewAPI.Tests
{
    using ContactViewAPI.App.Controllers;
    using ContactViewAPI.App.Dtos.Contact;
    using ContactViewAPI.Data.Models;
    using ContactViewAPI.Service.Contact;
    using FakeItEasy;
    using FluentAssertions;
    using Xunit;

    public class When_Contact_Service_Is_Called
    {
        private IContactService fakeContactService = A.Fake<IContactService>();
        private ContactController fakeContactController = A.Fake<ContactController>();
        private Contact contact;
        private Contact contactToUse = new Contact 
        {
            Id = 1,
            Name = "TestUser",
            Company = "TestCompany",
            UserId = 1
        };

        private Contact existingContact = new Contact
        {
            Id = 2,
            Name = "TestUpdate",
            Company = "TestUpdateCompany",
            UserId = 2
        };

        private ContactCreateDto createDto = new ContactCreateDto
        {
            Name = "Test",
            Company = "TestCompany"
        };

        public class And_A_Contact_Is_Created : When_Contact_Service_Is_Called
        {
            public And_A_Contact_Is_Created()
            {
                A.CallTo(() => fakeContactService.CreateAsync(A<Contact>._, A<int>._))
                .Invokes((Contact con, int? i) => contact = contactToUse);

                fakeContactService.CreateAsync(contactToUse, 1);
            }

            [Fact]
            public void And_The_Contact_Is_Created()
            {
                A.CallTo(() => fakeContactService.CreateAsync(A<Contact>._, A<int>._)).MustHaveHappenedOnceExactly();
            }

            [Fact]
            public void And_The_Created_Contact_Has_A_Valid_Id()
            {
                contact.Id.Should().BeGreaterThan(0).And.NotBe(null, "We expect Id of a contact to not be null");
            }

            [Fact]
            public void And_The_Created_Contact_Has_A_UserId()
            {
                contact.UserId.Should().BeGreaterThan(0).And.NotBe(null, "We expect User Id of a contact to not be null");
            }          
        }

        public class And_A_Contact_Is_Updated : When_Contact_Service_Is_Called
        {
            public And_A_Contact_Is_Updated()
            {
                A.CallTo(() => fakeContactService.UpdateAsync(A<Contact>._, A<int>._))
                    .Invokes((Contact con, int? i) => existingContact = contactToUse);

                fakeContactService.UpdateAsync(contactToUse, 1);
            }

            [Fact]
            public void And_The_Contact_Is_Updated()
            {
                A.CallTo(() => fakeContactService.UpdateAsync(A<Contact>._, A<int>._)).MustHaveHappenedOnceExactly();
            }

            [Fact]
            public void And_The_Updated_Contact_Has_The_Correct_Name()
            {
                existingContact.Name.Should().Be(contactToUse.Name, "We expect the name to be updated");
            }

            [Fact]
            public void And_The_Updated_Contact_Has_The_Correct_Company()
            {
                existingContact.Company.Should().Be(contactToUse.Company, "We expect the company to be updated");
            }

            [Fact]
            public void And_The_Updated_Contact_Has_The_Correct_UserId()
            {
                existingContact.UserId.Should().Be(contactToUse.UserId, "We expect the user id to be updated");
            }
        }

        public class And_A_Contact_Is_Deleted : When_Contact_Service_Is_Called
        {
            public And_A_Contact_Is_Deleted()
            {
                A.CallTo(() => fakeContactService.DeleteAsync(A<int>._, A<int>._));

                fakeContactService.DeleteAsync(1, 1);
            }

            [Fact]
            public void And_The_Contact_Is_Deleted()
            {
                A.CallTo(() => fakeContactService.DeleteAsync(A<int>._, A<int>._)).MustHaveHappenedOnceExactly();
            }
        }
    }
}
