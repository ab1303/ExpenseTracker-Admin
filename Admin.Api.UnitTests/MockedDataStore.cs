using System;
using System.Collections.Generic;
using System.Text;

namespace Admin.Api.UnitTests
{
    public class MockedDataStore
    {
        public IEnumerable<Domain.Model.User> UsersData()
        {
            var userList = new List<Domain.Model.User>
            {

                new Domain.Model.User
                {
                    UserId = Guid.Parse("25d574b8-9ca0-4720-8637-3dc43839cb48"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:46"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:28"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("7bc4312e-8698-4b61-82b7-7078c03b35ed"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:56"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:56"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("89ff377a-77b8-4578-a335-fb13a83f29ed"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:45:45"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:45:45"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("d875200d-4f01-4643-95bb-e1a63d76660a"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("d8e28191-bc5c-4283-b691-ce6185d1c89a"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("db72cd59-f56c-4e03-aa86-5b421e198f57"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                },

                new Domain.Model.User
                {
                    UserId = Guid.Parse("35d574b8-9ca0-4720-8637-3dc43839cb48"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:46"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:28"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("3bc4312e-8698-4b61-82b7-7078c03b35ed"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:56"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:56"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("39ff377a-77b8-4578-a335-fb13a83f29ed"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:45:45"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:45:45"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("3875200d-4f01-4643-95bb-e1a63d76660a"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("38e28191-bc5c-4283-b691-ce6185d1c89a"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("3b72cd59-f56c-4e03-aa86-5b421e198f57"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                },

                new Domain.Model.User
                {
                    UserId = Guid.Parse("45d574b8-9ca0-4720-8637-3dc43839cb48"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:46"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:28"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("4bc4312e-8698-4b61-82b7-7078c03b35ed"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:56"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:56"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("49ff377a-77b8-4578-a335-fb13a83f29ed"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:45:45"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:45:45"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("4875200d-4f01-4643-95bb-e1a63d76660a"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("48e28191-bc5c-4283-b691-ce6185d1c89a"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                },
                new Domain.Model.User
                {
                    UserId = Guid.Parse("4b72cd59-f56c-4e03-aa86-5b421e198f57"),
                    CreatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                    UpdatedDate = DateTime.Parse("2019-06-11 01:46:38"),
                },
            };

            return userList;
        }

        public IEnumerable<(Guid, bool)> UsersBslStatusData()
        {
            var usersBslStatusList = new List<(Guid, bool)>
            {
                (Guid.Parse("c0e818ae-8131-4f2b-a3c9-ef843e8b7bcd"), false),
                (Guid.Parse("09cde8bc-bd59-40f6-9eb2-d02581b5fa61"), true),
                (Guid.Parse("fb4b1053-73f2-46d1-92e5-e41bb0170b2d"), true),
                (Guid.Parse("d8df9c0d-57df-4d46-975c-8dc363f1eb7e"), false),
                (Guid.Parse("1a2e7313-bb04-41b2-8605-2d2b6c2f8fe0"), true),
                (Guid.Parse("30e818ae-8131-4f2b-a3c9-ef843e8b7bcd"), false),
                (Guid.Parse("39cde8bc-bd59-40f6-9eb2-d02581b5fa61"), true),
                (Guid.Parse("3b4b1053-73f2-46d1-92e5-e41bb0170b2d"), false),
                (Guid.Parse("38df9c0d-57df-4d46-975c-8dc363f1eb7e"), false),
                (Guid.Parse("3a2e7313-bb04-41b2-8605-2d2b6c2f8fe0"), true),
                (Guid.Parse("40e818ae-8131-4f2b-a3c9-ef843e8b7bcd"), false),
                (Guid.Parse("49cde8bc-bd59-40f6-9eb2-d02581b5fa61"), true),
                (Guid.Parse("4b4b1053-73f2-46d1-92e5-e41bb0170b2d"), false),
                (Guid.Parse("48df9c0d-57df-4d46-975c-8dc363f1eb7e"), false),
                (Guid.Parse("4a2e7313-bb04-41b2-8605-2d2b6c2f8fe0"), true),
            };

            return usersBslStatusList;
        }
    }
}
