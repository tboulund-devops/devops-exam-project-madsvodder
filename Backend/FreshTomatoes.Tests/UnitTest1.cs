using Backend;
using Backend.Data;
using Backend.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Assert = Xunit.Assert;

namespace FreshTomatoes.Tests;

public class UnitTest1()
{

    public List<User> users = new List<User>
    {
        new User
        {
            Email = "madsvodder@gmail.com",
            PasswordHash = "1234",
            Username = "madsv7922"
        }
    };
    
    [Test]
    public async Task LoginAsync_FindsUser()
    {
        // Arrange
        var request = new User
        {
            Email = "madsvodder@gmail.com",
            PasswordHash = "1234",
            Username = "madsv7922"
        };

        // Act
        var user = users.SingleOrDefault(u => u.Email.ToLower() == request.Email.ToLower());

        // Assert
        Assert.Equal(request.Email, user.Email);
    }
}